using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using NativeWifi;
using System.Timers;
using System.Diagnostics;
using System.Threading;
using SharpPcap;
using SharpPcap.WinPcap;

namespace Stability_Monitor_win32
{
    class Wifi_agent : Agent
    {
        private TcpListener _tcplistener;
        private TcpClient _tcpclient = new TcpClient();
        private UdpClient _udpclient = new UdpClient();
        private NetworkStream _netstream;
        private Stream _filestream;
        private Byte[] _buffer;
        private byte[] _tosend;
        private int _length;

        private WlanClient _wlanclient = new WlanClient();
        private uint _signalquality = 0;
        private int _rssi = 0;
        private System.Timers.Timer _timer_sq_rssi = new System.Timers.Timer();
        private Stopwatch _stopwatch = new Stopwatch();
        
        private long _transferspeed = 0;
        private ulong oldseconds = 0;
        private ulong oldmicroseconds = 0;          

        public Wifi_agent(String filepath, Agenttype agenttype, Callback_on_status_changed callback, Results results) : base(filepath, agenttype, callback, results) { }

        public override void send_file(String devicename, String ipadd, int port)
        {
            scan_transfer_speed(devicename, ipadd);
            scan_signal_quality_and_rssi();
            _stopwatch.Start();

            send_file_tcp(ipadd, port);

            _timer_sq_rssi.Stop();
            _stopwatch.Stop();
        }

        public override void receive_file(String devicename, String ipadd, int port)
        {
            scan_transfer_speed(devicename, ipadd);
            scan_signal_quality_and_rssi();
            _stopwatch.Start();

            receive_file_tcp(devicename, ipadd, port);

            System.Threading.Thread.Sleep(1100);

            _timer_sq_rssi.Stop();
            _stopwatch.Stop();
        }

        private void send_file_tcp(String ipadd, int port)
        {               
            _tcpclient.Connect(IPAddress.Parse(ipadd), port);
            _netstream = _tcpclient.GetStream();

            _tosend = File.ReadAllBytes(get_filepath());

            _netstream.Write(_tosend, 0, _tosend.Length);
            _netstream.Flush();

            _netstream.Dispose();
            _netstream.Close();

            _tcpclient.Close();
            
        }

        private void receive_file_tcp(String devicename, String ipadd, int port)
        {
            _buffer = new byte[1500];

            _tcplistener = new TcpListener(IPAddress.Any, port);
            _tcplistener.Start();
            _tcpclient = _tcplistener.AcceptTcpClient();
            _netstream = _tcpclient.GetStream();
            _filestream = new FileStream(get_filepath(), FileMode.Create, FileAccess.ReadWrite);

            while ((_length = _netstream.Read(_buffer, 0, _buffer.Length)) != 0)
            {
                _filestream.Write(_buffer, 0, _length);
            }

            _filestream.Dispose();
            _filestream.Close();

            _netstream.Dispose();
            _netstream.Close();

            _tcpclient.Close();
            _tcplistener.Stop();
        }

        private void send_file_udp(String ipadd, int port)
        {
            IPEndPoint ipep = new IPEndPoint(IPAddress.Parse(ipadd), 10000);

            _tosend = File.ReadAllBytes(get_filepath());

            int index = 0;

            _buffer = new byte[65507];

            while ((index + 65507) < _tosend.Length)
            {
                Array.Copy(_tosend, index, _buffer, 0, 65507);
                _udpclient.Send(_buffer, 65507, ipep);
                index += 65507;
            }

            if (index != _tosend.Length)
            {
                Array.Copy(_tosend, index, _buffer, 0, (_tosend.Length - index));
                _udpclient.Send(_buffer, (_tosend.Length - index), ipep);
            }

            _udpclient.Send(_buffer, 0, ipep);

            _udpclient.Close();

        }

        private void receive_file_udp(String devicename, String ipadd, int port)
        {
            UdpClient udpclient = new UdpClient(port);
            IPEndPoint ipep = new IPEndPoint(IPAddress.Any, port);

            _filestream = new FileStream(get_filepath(), FileMode.Create, FileAccess.ReadWrite);

            scan_transfer_speed(devicename, ipadd);

            while ((_length = (_buffer = _udpclient.Receive(ref ipep)).Length) != 0)
            {
                _filestream.Write(_buffer, 0, _length);
            }

            _filestream.Dispose();
            _filestream.Close();

            udpclient.Close();
        }

        private void check_signal_quality_and_rssi(object sender, EventArgs e)
        {
            foreach (WlanClient.WlanInterface wlanIface in _wlanclient.Interfaces)
            {
                if (wlanIface.InterfaceState.ToString() == "Connected")
                {
                    if (wlanIface.CurrentConnection.wlanAssociationAttributes.wlanSignalQuality != _signalquality)
                    {
                        String time = _stopwatch.Elapsed.ToString("mm\\:ss\\.ff");

                        _signalquality = wlanIface.CurrentConnection.wlanAssociationAttributes.wlanSignalQuality;

                        get_callback().on_signal_intensity_or_rssi_change(get_agenttype() + " " + time + " " + _signalquality + " % \r\n", get_results());
                    }

                    Wlan.WlanBssEntry[] wlanbssentries = wlanIface.GetNetworkBssList();

                    foreach (Wlan.WlanBssEntry network in wlanbssentries)
                    {
                        if (wlanIface.CurrentConnection.wlanAssociationAttributes.dot11Ssid.SSID == network.dot11Ssid.SSID)
                        {
                            if (network.rssi != _rssi) 
                            {
                                String time = _stopwatch.Elapsed.ToString("mm\\:ss\\.ff");

                                _rssi = network.rssi;

                                get_callback().on_signal_intensity_or_rssi_change(get_agenttype() + " " + time + " " + _rssi + " dBm \r\n", get_results());
                                break;
                            }
                        }
                    }
                    break;                  
                }
            }            
        }

        private void scan_signal_quality_and_rssi()
        {   
            _timer_sq_rssi.Elapsed += new ElapsedEventHandler(check_signal_quality_and_rssi);
            _timer_sq_rssi.Interval = 100;
            _timer_sq_rssi.Start();
        } 

        private void scan_transfer_speed(String devicename, String ipadd)
        {
            CaptureDeviceList devices = CaptureDeviceList.Instance;
            WinPcapDevice device = null;
            
            foreach (WinPcapDevice d in devices)
            {
                if (d.Interface.FriendlyName == devicename)
                {
                    device = d;
                    break;
                }
            }

            device.OnPcapStatistics += new StatisticsModeEventHandler(update_statistics);
            device.Open(DeviceMode.Promiscuous, 1000);
            device.Filter = "(tcp or udp) and host " + ipadd;
            device.Mode = CaptureMode.Statistics;
            device.StartCapture();
        }

        private void update_statistics(object sender, StatisticsModeEventArgs e)
        {
            StatisticsModePacket statistics = e.Statistics;

            long delay = (long)((statistics.Timeval.Seconds - oldseconds) * 1000000 
                + (statistics.Timeval.MicroSeconds - oldmicroseconds));

                _transferspeed = (statistics.RecievedBytes * 1000000) / delay / 1024;

            String time = _stopwatch.Elapsed.ToString("mm\\:ss\\.ff");

            get_callback().on_transfer_speed_change(get_agenttype() + " " + time + " " + _transferspeed.ToString() + " kB/s \r\n", get_results());

            oldseconds = statistics.Timeval.Seconds;
            oldmicroseconds = statistics.Timeval.MicroSeconds;
        }
    }
}
