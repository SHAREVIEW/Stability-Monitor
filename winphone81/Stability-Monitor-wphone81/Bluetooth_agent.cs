using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth.Rfcomm;
using Windows.Devices.Enumeration;
using Windows.Networking;
using Windows.Networking.Proximity;
using Windows.Networking.Sockets;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.System.Threading;
using Windows.UI.Xaml;

namespace Stability_Monitor_wphone81
{
    class Bluetooth_agent : Agent
    {
        private PeerInformation _peer_info;
        private RfcommServiceProvider _rfcomm_provider;
        private StreamSocket _bluetooth_client;
        private StreamSocketListener _bluetooth_listener;
        private DataWriter _datawriter;
        private DataReader _datareader;
        private DataWriter _datawriter_foracks;
        private uint _length;
        private byte[] _buffer;
        private IBuffer _ibuffer;

        private bool _writing = false;
        private uint _transferspeed = 0;
        private ThreadPoolTimer _timer;
        private Stopwatch _stopwatch = new Stopwatch();
        private String _message;
        private int _counter_all_acks = 0;
        private int _old_counter_all_acks = 0;
        private ThreadPoolTimer _ack_timer;
        private int _counter_to_ack_error = 0;

        public Bluetooth_agent(String filepath, Agenttype agenttype, Callback_on_status_changed callback, Results results, Main_view main_view) : base(filepath, agenttype, callback, results, main_view) { }

        public override async void send_file(String devicename, String bluid, int not)
        {
            try {
                _stopwatch.Start();

                PeerFinder.AllowBluetooth = true;
                PeerFinder.AlternateIdentities["Bluetooth:SDP"] = "{" + bluid + "}";

                var peers = await PeerFinder.FindAllPeersAsync();

                foreach (var p in peers)
                {
                    if (p.DisplayName.Equals(devicename))
                    {
                        _peer_info = p;
                        break;
                    }
                }

                _bluetooth_client = new StreamSocket();

                if (_peer_info.ServiceName.Equals(""))
                {
                    await _bluetooth_client.ConnectAsync(_peer_info.HostName, "{" + bluid + "}");
                }
                else
                {
                    await _bluetooth_client.ConnectAsync(_peer_info.HostName, _peer_info.ServiceName);
                }

                StorageFolder folder = KnownFolders.PicturesLibrary;
                StorageFile file = await folder.GetFileAsync(this.filepath);

                IRandomAccessStreamWithContentType filestream = await file.OpenReadAsync();

                _length = (uint)filestream.Size;

                _ibuffer = new Windows.Storage.Streams.Buffer(_length);

                _datareader = new DataReader(filestream);
                await _datareader.LoadAsync(_length);
                _ibuffer = _datareader.ReadBuffer(_length);

                _datawriter = new DataWriter(_bluetooth_client.OutputStream);
                _datawriter.WriteBuffer(_ibuffer);
                await _datawriter.StoreAsync();

                filestream.Dispose();
                _datareader.Dispose();
                _datawriter.Dispose();
                               
                _datareader = new DataReader(_bluetooth_client.InputStream);
                _datareader.InputStreamOptions = InputStreamOptions.Partial;

                scan_received_acks();

                while (true)
                {
                    uint count = await _datareader.LoadAsync(4);
                    byte[] ack = new byte[count];

                    _datareader.ReadBytes(ack);

                    _counter_all_acks += BitConverter.ToInt32(ack, 0);

                    if ((uint)_counter_all_acks == _length) break;
                }

                _datareader.Dispose();
                _bluetooth_client.Dispose();

                _message = format_message(_stopwatch.Elapsed, "File Transfer", "OK", this.filepath);
                this.callback.on_file_received(_message, this.results);
                this.main_view.text_to_logs(_message.Replace("\t", " "));

                _ack_timer.Cancel();
                _stopwatch.Stop();
            }
            catch (Exception e)
            {
                append_error_tolog(e, _stopwatch.Elapsed, devicename);
            }
        }

        private void scan_received_acks()
        {
            _ack_timer = ThreadPoolTimer.CreatePeriodicTimer(check_received_acks, TimeSpan.FromSeconds(1));
        }

        private void check_received_acks(ThreadPoolTimer timer)
        {
            if (_old_counter_all_acks != _counter_all_acks)
            {
                _counter_to_ack_error = 0;
                _old_counter_all_acks = _counter_all_acks;
            }
            else
            {
                _counter_to_ack_error++;
            }

            if (_counter_to_ack_error > 20)
            {
                _bluetooth_client.Dispose();
                timer.Cancel();

                _message = format_message(_stopwatch.Elapsed, "File Transfer", "NOK", "Timeout error when receiving acks.");
                this.callback.on_file_received(_message, this.results);
                this.main_view.text_to_logs(_message.Replace("\t", " "));
            }
        }

        public override async void receive_file(String devicename, String bluid, int not)
        {
            try {
                _stopwatch.Start();

                _rfcomm_provider = await RfcommServiceProvider.CreateAsync(
                    RfcommServiceId.FromUuid(new Guid(bluid)));

                _bluetooth_listener = new StreamSocketListener();
                _bluetooth_listener.ConnectionReceived += _bluetooth_connection_received;

                await _bluetooth_listener.BindServiceNameAsync(_rfcomm_provider.ServiceId.AsString(),
                    SocketProtectionLevel.BluetoothEncryptionAllowNullAuthentication);

                _rfcomm_provider.StartAdvertising(_bluetooth_listener);
            }
            catch (Exception e)
            {
                append_error_tolog(e, _stopwatch.Elapsed, "");
            }
        }

        private async void _bluetooth_connection_received(StreamSocketListener sender, StreamSocketListenerConnectionReceivedEventArgs args)
        {
            try
            {
                scan_transfer_speed();
                
                _datareader = new DataReader(args.Socket.InputStream);
                _datareader.InputStreamOptions = InputStreamOptions.Partial;

                _datawriter_foracks = new DataWriter(args.Socket.OutputStream);

                StorageFolder folder = KnownFolders.PicturesLibrary;
                StorageFile file = await folder.CreateFileAsync(this.filepath, CreationCollisionOption.ReplaceExisting);

                IRandomAccessStream filestream = await file.OpenAsync(FileAccessMode.ReadWrite);
                IOutputStream filewriter = filestream.GetOutputStreamAt(0);

                _datawriter = new DataWriter(filewriter);

                while (!((_length = await _datareader.LoadAsync(65000)) == 0))
                {
                    while (_writing) { }

                    _transferspeed += _length;

                    _buffer = new byte[_length];
                    _datareader.ReadBytes(_buffer);

                    _datawriter.WriteBytes(_buffer);
                    await _datawriter.StoreAsync();
                }

                _datawriter.Dispose();
                filewriter.Dispose();
                filestream.Dispose();
                _datareader.Dispose();

                _transferspeed /= 1024;
                _message = format_message(_stopwatch.Elapsed, "Transferspeed", _transferspeed.ToString(), "kB/s");
                this.callback.on_transfer_speed_change(_message, this.results);
                this.main_view.text_to_logs(_message.Replace("\t", " "));

                _timer.Cancel();
                
                sender.Dispose();
                _bluetooth_listener.Dispose();

                _message = format_message(_stopwatch.Elapsed, "File Transfer", "OK", this.filepath);
                this.callback.on_file_received(_message, this.results);
                this.main_view.text_to_logs(_message.Replace("\t", " "));

                _stopwatch.Stop();
            }
            catch (Exception e)
            {
                append_error_tolog(e, _stopwatch.Elapsed, "");
            }
        }

        private void scan_transfer_speed()
        {
            _timer = ThreadPoolTimer.CreatePeriodicTimer(update_network_speed, TimeSpan.FromSeconds(1));
        }

        private async void update_network_speed(ThreadPoolTimer timer)
        {
            try {
                _writing = true;

                if (_datawriter_foracks != null)
                {
                    byte[] ack = BitConverter.GetBytes(_transferspeed);
                    _datawriter_foracks.WriteBytes(ack);
                    await _datawriter_foracks.StoreAsync();
                }

                _transferspeed /= 1024;
                _message = format_message(_stopwatch.Elapsed, "Transferspeed", _transferspeed.ToString(), "kB/s");
                this.callback.on_transfer_speed_change(_message, this.results);
                this.main_view.text_to_logs(_message.Replace("\t", " "));

                _transferspeed = 0;

                _writing = false;
            }
            catch (Exception e)
            {
                append_error_tolog(e, _stopwatch.Elapsed, "");
            }
        }

        public void stop_scanning()
        {
            _ack_timer.Cancel();
            _timer.Cancel();
        }
    }
}
