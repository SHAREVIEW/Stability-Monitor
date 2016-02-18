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
        private Stream _stream;
        private Byte[] _buffer;
        private byte[] _tosend;
        private int _length;

        private WlanClient _wlanclient = new WlanClient();
        private uint _signalquality = 0;
        private int _rssi = 0;
        private System.Timers.Timer _timer_sq;

        private long _transferspeed = 0;
        private ulong oldseconds = 0;
        private ulong oldmicroseconds = 0;          

        public Wifi_agent(String filepath, Agenttype agenttype, Callback_on_status_changed callback, Results results) : base(filepath, agenttype, callback, results)
        {
            
        }

        public override void send_file(IPAddress ipadd, int port)
        {
            scan_signal_quality_and_rssi();            
            
            send_file_tcp(ipadd, port);

        }

        public override void receive_file(int port)
        {

            receive_file_tcp(port);
            
            //this.get_callback().on_file_received(get_filepath(), "subor uspesne prijaty", get_results());
        }

        private void send_file_tcp(IPAddress ipadd, int port)
        {            
            _tcpclient.Connect(ipadd, port);
            _netstream = _tcpclient.GetStream();

            _tosend = File.ReadAllBytes(get_filepath());

            _netstream.Write(_tosend, 0, _tosend.Length);
            _netstream.Flush();

            _netstream.Dispose();
            _netstream.Close();

            _tcpclient.Close();
            
        }

        private void receive_file_tcp(int port)
        {
            _buffer = new byte[1500];

            _tcplistener = new TcpListener(IPAddress.Any, port);
            _tcplistener.Start();
            _tcpclient = _tcplistener.AcceptTcpClient();
            _netstream = _tcpclient.GetStream();
            _stream = new FileStream(get_filepath(), FileMode.Create, FileAccess.ReadWrite);

            while ((_length = _netstream.Read(_buffer, 0, _buffer.Length)) != 0)
            {
                _stream.Write(_buffer, 0, _length);
            }

            _stream.Dispose();
            _stream.Close();

            _netstream.Dispose();
            _netstream.Close();

            _tcpclient.Close();
            _tcplistener.Stop();
        }

        private void send_file_udp(IPAddress ipadd, int port)
        {
            IPEndPoint ipep = new IPEndPoint(ipadd, 10000);

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

        private void receive_file_udp(int port)
        {
            UdpClient udpclient = new UdpClient(port);
            IPEndPoint ipep = new IPEndPoint(IPAddress.Any, port);

            _stream = new FileStream(get_filepath(), FileMode.Create, FileAccess.ReadWrite);

            while ((_length = (_buffer = _udpclient.Receive(ref ipep)).Length) != 0)
            {
                _stream.Write(_buffer, 0, _length);
            }

            _stream.Dispose();
            _stream.Close();

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
                        _signalquality = wlanIface.CurrentConnection.wlanAssociationAttributes.wlanSignalQuality;
                        get_callback().on_signal_intensity_or_rssi_change("Actual signal quality of Wifi:\t" + _signalquality + " %", get_results());
                    }

                    Wlan.WlanBssEntry[] wlanbssentries = wlanIface.GetNetworkBssList();

                    foreach (Wlan.WlanBssEntry network in wlanbssentries)
                    {
                        if (wlanIface.CurrentConnection.wlanAssociationAttributes.dot11Ssid.SSID == network.dot11Ssid.SSID)
                        {
                            if (network.rssi != _rssi) 
                            {
                                _rssi = network.rssi;
                                get_callback().on_signal_intensity_or_rssi_change("Actual RSSI of Wifi:\t" + _rssi + " dBm", get_results());
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
            Stopwatch stopwatch_sq = new Stopwatch();
            stopwatch_sq.Start();

            _timer_sq.Elapsed += new ElapsedEventHandler(check_signal_quality_and_rssi);
            _timer_sq.Interval = 100;
            _timer_sq.Start();
        } 

        private void scan_transfer_speed()
        {
            CaptureDeviceList devices = CaptureDeviceList.Instance;
            WinPcapDevice device = null;
            
            foreach (WinPcapDevice d in devices)
            {
                if (d.ToString().Contains("WiFi"))
                {
                    device = d;
                    break;
                }
            }

            device.OnPcapStatistics += new StatisticsModeEventHandler(update_statistics);
            device.Open(DeviceMode.Promiscuous, 1000);
            device.Filter = "tcp or udp";
            device.Mode = CaptureMode.Statistics;
            device.StartCapture();
        }

        private void update_statistics(object sender, StatisticsModeEventArgs e)
        {
            StatisticsModePacket statistics = e.Statistics;

            long delay = (long)((statistics.Timeval.Seconds - oldseconds) * 1000000 
                + (statistics.Timeval.MicroSeconds - oldmicroseconds));
            _transferspeed = (statistics.RecievedBytes * 8 * 1000000) / delay;

            String time = (statistics.Timeval.Seconds + statistics.Timeval.MicroSeconds).ToString();

            get_callback().on_transfer_speed_change(time, _transferspeed.ToString(), get_results());

            oldseconds = statistics.Timeval.Seconds;
            oldmicroseconds = statistics.Timeval.MicroSeconds;
        }
    }
}
