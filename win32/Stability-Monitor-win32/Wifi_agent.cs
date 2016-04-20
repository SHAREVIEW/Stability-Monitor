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
using System.Security.Cryptography;

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
        private ulong _oldseconds = 0;
        private ulong _oldmicroseconds = 0;
        private String _message;
        private WinPcapDevice _device = null;

        public Wifi_agent(String filepath, Agenttype agenttype, Callback_on_status_changed callback, Results results, Main_view mv) : base(filepath, agenttype, callback, results, mv) { }

        public override void send_file(String devicename, String ipadd, int port)
        {
            _stopwatch.Start();
            scan_transfer_speed(devicename, ipadd);
            scan_signal_quality_and_rssi();

            if (port < 10000)
            {
                send_file_tcp(ipadd, port);
            }
            else
            {
                send_file_udp(ipadd, port);
            }    
                   
            _timer_sq_rssi.Stop();
            if (_device.Started) _device.Close();
            _stopwatch.Stop();
        }

        public override void receive_file(String devicename, String ipadd, int port)
        {
            _stopwatch.Start();
            scan_transfer_speed(devicename, ipadd);
            scan_signal_quality_and_rssi();

            if (port < 10000)
            {
                receive_file_tcp(devicename, ipadd, port);
            }
            else
            {
                receive_file_udp(devicename, ipadd, port);
            }

            _timer_sq_rssi.Stop();
            if (_device.Started) _device.Close();
            _stopwatch.Stop();
        }

        private void send_file_tcp(String ipadd, int port)
        {
            try
            {
                _tcpclient.Connect(IPAddress.Parse(ipadd), port);
                _netstream = _tcpclient.GetStream();

                _filestream = File.OpenRead(this.filepath);

                _tosend = new byte[63 * 1024];

                while ((_length = _filestream.Read(_tosend, 0, 63 * 1024)) != 0)
                {
                    _netstream.Write(_tosend, 0, _length);
                }

                _netstream.Dispose();
                _netstream.Close();

                _tcpclient.Close();

                System.Threading.Thread.Sleep(1000);

                _message = format_message(_stopwatch.Elapsed, "File Transfer", "OK", this.filepath);
                this.callback.on_file_received(_message, this.results);
                this.main_view.text_to_logs(_message);                
            }
            catch (Exception e)
            {
                append_error_tolog(e, _stopwatch.Elapsed, "");
            }       
        }

        private void receive_file_tcp(String devicename, String ipadd, int port)
        {
            try
            {
                _buffer = new byte[63 * 1024];

                _tcplistener = new TcpListener(IPAddress.Any, port);
                _tcplistener.Start();
                _tcpclient = _tcplistener.AcceptTcpClient();
                _netstream = _tcpclient.GetStream();
                _filestream = new FileStream(this.filepath, FileMode.Create, FileAccess.ReadWrite);

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

                System.Threading.Thread.Sleep(1000);

                _message = format_message(_stopwatch.Elapsed, "File Transfer", "OK", this.filepath);
                this.callback.on_file_received(_message, this.results);
                this.main_view.text_to_logs(_message);
            }
            catch (Exception e)
            {
                append_error_tolog(e, _stopwatch.Elapsed, devicename);
            }
        }

        private void send_file_udp(String ipadd, int port)
        {
            try
            {
                IPEndPoint ipep = new IPEndPoint(IPAddress.Parse(ipadd), port);
                byte[] hash_bytes;

                _tosend = File.ReadAllBytes(this.filepath);

                hash_bytes = new MD5CryptoServiceProvider().ComputeHash(_tosend);
                _udpclient.Send(hash_bytes, hash_bytes.Length, ipep);

                System.Threading.Thread.Sleep(1000);

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

                _message = format_message(_stopwatch.Elapsed, "File Transfer", "OK", this.filepath);
                this.callback.on_file_received(_message, this.results);
                this.main_view.text_to_logs(_message);
            }
            catch (Exception e)
            {
                append_error_tolog(e, _stopwatch.Elapsed, "");
            }
        }

        private void receive_file_udp(String devicename, String ipadd, int port)
        {
            try
            {
                _udpclient = new UdpClient(port);
                IPEndPoint ipep = new IPEndPoint(IPAddress.Any, port);
                byte[] hash_bytes = new byte[16];
                int hash_need = 16;

                _filestream = new FileStream(this.filepath, FileMode.Create, FileAccess.ReadWrite);

                while (true)
                {
                    _length = (_buffer = _udpclient.Receive(ref ipep)).Length;

                    if (_length == hash_need)
                    {
                        Array.Copy(_buffer, 0, hash_bytes, 16 - hash_need, hash_need);
                                                
                        break;
                    }
                    else if (_length < hash_need)
                    {
                        Array.Copy(_buffer, 0, hash_bytes, 16 - hash_need, _length);
                        hash_need -= _length;
                    }
                }

                while ((_length = (_buffer = _udpclient.Receive(ref ipep)).Length) != 0)
                {
                    _filestream.Write(_buffer, 0, _length);
                }

                _filestream.Dispose();
                _filestream.Close();

                byte[] receivedbytes = File.ReadAllBytes(this.filepath);
                byte[] hash_receivedbytes = new MD5CryptoServiceProvider().ComputeHash(receivedbytes);

                _udpclient.Close();

                if (!(hash_bytes.SequenceEqual(hash_receivedbytes)))
                {
                    _message = format_message(_stopwatch.Elapsed, "File Transfer", "NOK", "Different hash");
                    this.callback.on_file_received(_message, this.results);
                    this.main_view.text_to_logs(_message);
                }
                else
                {
                    _message = format_message(_stopwatch.Elapsed, "File Transfer", "OK", this.filepath);
                    this.callback.on_file_received(_message, this.results);
                    this.main_view.text_to_logs(_message);
                }    
            }
            catch (Exception e)
            {
                append_error_tolog(e, _stopwatch.Elapsed, devicename);
            }
        }

        private void check_signal_quality_and_rssi(object sender, EventArgs e)
        {
            foreach (WlanClient.WlanInterface wlanIface in _wlanclient.Interfaces)
            {
                if (wlanIface.InterfaceState.ToString().Equals("Connected"))
                {                    
                    Wlan.WlanBssEntry[] wlanbssentries = wlanIface.GetNetworkBssList();

                    foreach (Wlan.WlanBssEntry network in wlanbssentries)
                    {
                        if (wlanIface.CurrentConnection.wlanAssociationAttributes.dot11Ssid.SSID.SequenceEqual(network.dot11Ssid.SSID))
                        {
                            if (network.linkQuality != _signalquality)
                            {
                                String time = _stopwatch.Elapsed.ToString("mm\\:ss\\.ff");

                                _signalquality = network.linkQuality;

                                _message = format_message(_stopwatch.Elapsed, "Quality of Signal", _signalquality.ToString(), "%");
                                this.callback.on_signal_intensity_or_rssi_change(_message, this.results);
                                this.main_view.text_to_logs(_message);
                            }

                            if (network.rssi != _rssi)
                            {
                                String time = _stopwatch.Elapsed.ToString("mm\\:ss\\.ff");

                                _rssi = network.rssi;

                                _message = format_message(_stopwatch.Elapsed, "RSSI", _rssi.ToString(), "dBm");
                                this.callback.on_signal_intensity_or_rssi_change(_message, this.results);
                                this.main_view.text_to_logs(_message);
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
            _timer_sq_rssi.Interval = 1000;
            _timer_sq_rssi.Start();
        }

        private void scan_transfer_speed(String devicename, String ipadd)
        {
            CaptureDeviceList devices = CaptureDeviceList.Instance;

            foreach (WinPcapDevice d in devices)
            {
                if (d.Interface.FriendlyName == devicename)
                {
                    _device = d;
                    break;
                }
            }

            if (_device != null)
            {
                _device.OnPcapStatistics += new StatisticsModeEventHandler(update_statistics);
                _device.Open(DeviceMode.Promiscuous, 1000);
                _device.Filter = "(tcp or udp) and host " + ipadd;
                _device.Mode = CaptureMode.Statistics;
                _device.StartCapture();
            }
        }

        private void update_statistics(object sender, StatisticsModeEventArgs e)
        {
            StatisticsModePacket statistics = e.Statistics;

            long delay = (long)((statistics.Timeval.Seconds - _oldseconds) * 1000000
                + (statistics.Timeval.MicroSeconds - _oldmicroseconds));

            _transferspeed = (statistics.RecievedBytes * 1000000) / delay / 1024;

            _message = format_message(_stopwatch.Elapsed, "Transferspeed", _transferspeed.ToString(), "kB/s");
            this.callback.on_transfer_speed_change(_message, this.results);
            this.main_view.text_to_logs(_message);

            _oldseconds = statistics.Timeval.Seconds;
            _oldmicroseconds = statistics.Timeval.MicroSeconds;
        }

        public void stop_scanning()
        {
            _timer_sq_rssi.Stop();
            if (_device.Started) _device.Close();
            _stopwatch.Stop();
        }
    }
}
