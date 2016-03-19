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
        private RfcommDeviceService _rfcomm_service;
        private RfcommServiceProvider _rfcomm_provider;
        private StreamSocket _bluetooth_client;
        private StreamSocketListener _bluetooth_listener;
        private DataWriter _datawriter;
        private DataReader _datareader;
        private uint _length;
        private byte[] _buffer;
        private IBuffer _ibuffer;

        private bool _write = false;
        private uint _transferspeed = 0;
        private ThreadPoolTimer timer;
        private Stopwatch _stopwatch = new Stopwatch();
        private String _message;

        public Bluetooth_agent(String filepath, Agenttype agenttype, Callback_on_status_changed callback, Results results) : base(filepath, agenttype, callback, results) { }

        public override async void send_file(String devicename, String bluid, int not)
        {
            try
            {
                var devices = await DeviceInformation.FindAllAsync(
                    RfcommDeviceService.GetDeviceSelector(RfcommServiceId.FromUuid(new Guid(bluid))));

                foreach (var d in devices)
                {
                    if (d.Name.Equals(devicename))
                    {
                        _rfcomm_service = await RfcommDeviceService.FromIdAsync(d.Id);
                        break;
                    }
                }

                _bluetooth_client = new StreamSocket();
                _bluetooth_client.Control.NoDelay = false;

                await _bluetooth_client.ConnectAsync(_rfcomm_service.ConnectionHostName, "{" + bluid + "}");

                StorageFolder folder = KnownFolders.PicturesLibrary;
                StorageFile file = await folder.GetFileAsync(this._filepath);

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

                int all_acks = 0;
                _datareader = new DataReader(_bluetooth_client.InputStream);
                _datareader.InputStreamOptions = InputStreamOptions.Partial;

                while (true)
                {
                    uint count = await _datareader.LoadAsync(4);
                    byte[] ack = new byte[count];

                    _datareader.ReadBytes(ack);

                    all_acks += BitConverter.ToInt32(ack, 0);

                    if ((uint)all_acks == _length) break;
                }

                _datareader.Dispose();
                _bluetooth_client.Dispose();
            }
            catch (Exception e)
            {

            }
        }

        public override async void receive_file(String bluid, int not)
        {
            try
            {
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

            }
        }

        private async void _bluetooth_connection_received(StreamSocketListener sender, StreamSocketListenerConnectionReceivedEventArgs args)
        {
            try
            {
                scan_transfer_speed();
                _stopwatch.Start();

                _datareader = new DataReader(args.Socket.InputStream);
                _datareader.InputStreamOptions = InputStreamOptions.Partial;

                StorageFolder folder = KnownFolders.PicturesLibrary;
                StorageFile file = await folder.CreateFileAsync(this._filepath, CreationCollisionOption.ReplaceExisting);

                IRandomAccessStream filestream = await file.OpenAsync(FileAccessMode.ReadWrite);
                IOutputStream filewriter = filestream.GetOutputStreamAt(0);

                _datawriter = new DataWriter(filewriter);

                while (!((_length = await _datareader.LoadAsync(65000)) == 0))
                {
                    _buffer = new byte[_length];
                    _datareader.ReadBytes(_buffer);

                    _transferspeed += _length;

                    if (_write)
                    {
                        _transferspeed /= 1024;
                        _message = format_message(_stopwatch.Elapsed, "Transferspeed", _transferspeed.ToString(), "kB/s");
                        this._callback.on_transfer_speed_change(_message, this._results);

                        _transferspeed = 0;
                        _write = false;
                    }

                    _datawriter.WriteBytes(_buffer);
                    await _datawriter.StoreAsync();
                }

                _datawriter.Dispose();
                filewriter.Dispose();
                filestream.Dispose();
                _datareader.Dispose();

                timer.Cancel();

                _transferspeed /= 1024;
                _message = format_message(_stopwatch.Elapsed, "Transferspeed", _transferspeed.ToString(), " kB/s");
                this._callback.on_transfer_speed_change(_message, this._results);

                _stopwatch.Stop();

                sender.Dispose();
                _bluetooth_listener.Dispose();
            }
            catch (Exception e)
            {

            }
        }

        private void scan_transfer_speed()
        {
            timer = ThreadPoolTimer.CreatePeriodicTimer(update_network_speed, TimeSpan.FromSeconds(1));
        }

        private void update_network_speed(ThreadPoolTimer timer)
        {
            _write = true;
        }
    }
}
