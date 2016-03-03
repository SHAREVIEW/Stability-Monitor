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
        private byte[] _tosend;
        private uint _length;
        private byte[] _buffer;

        private bool _write = false;
        private uint _transferspeed = 0;
        private ThreadPoolTimer timer;
        private Stopwatch _stopwatch = new Stopwatch();

        public Bluetooth_agent(String filepath, Agenttype agenttype, Callback_on_status_changed callback, Results results) : base(filepath, agenttype, callback, results) { }

        public override async void send_file(String bluid, int not)
        {
            var devices = await DeviceInformation.FindAllAsync(
                RfcommDeviceService.GetDeviceSelector(RfcommServiceId.FromUuid(new Guid(bluid))));

                    _rfcomm_service = await RfcommDeviceService.FromIdAsync(devices[0].Id);

            _bluetooth_client = new StreamSocket();
            await _bluetooth_client.ConnectAsync(_rfcomm_service.ConnectionHostName, _rfcomm_service.ConnectionServiceName);

            _datawriter = new DataWriter(_bluetooth_client.OutputStream);

            StorageFolder folder = KnownFolders.PicturesLibrary;
            StorageFile file = await folder.GetFileAsync(get_filepath());

            IRandomAccessStreamWithContentType filestream = await file.OpenReadAsync();
            _tosend = new byte[filestream.Size];

            _datareader = new DataReader(filestream);
            await _datareader.LoadAsync((uint)filestream.Size);
            _datareader.ReadBytes(_tosend);

            _datawriter.WriteBytes(_tosend);
            await _datawriter.StoreAsync();
            await _datawriter.FlushAsync();            

            byte[] endmsg = new byte[3];
            endmsg[0] = 69;
            endmsg[1] = 78;
            endmsg[2] = 68;

            _datawriter.WriteBytes(endmsg);
            await _datawriter.StoreAsync();
            await _datawriter.FlushAsync();

            _datawriter.Dispose();
            _datareader.Dispose();
                        
            await _bluetooth_client.OutputStream.FlushAsync();                 
                        
        }

        public override async void receive_file(String bluid, int not)
        {
            _rfcomm_provider = await RfcommServiceProvider.CreateAsync(
                RfcommServiceId.FromUuid( new Guid(bluid)));

            _bluetooth_listener = new StreamSocketListener();
            _bluetooth_listener.ConnectionReceived += _bluetooth_connection_received;

            await _bluetooth_listener.BindServiceNameAsync(_rfcomm_provider.ServiceId.AsString(),
                SocketProtectionLevel.BluetoothEncryptionAllowNullAuthentication);

            _rfcomm_provider.StartAdvertising(_bluetooth_listener);
        }

        private async void _bluetooth_connection_received(StreamSocketListener sender, StreamSocketListenerConnectionReceivedEventArgs args)
        {
            scan_transfer_speed();
            _stopwatch.Start();

            _datareader = new DataReader(args.Socket.InputStream);
            _datareader.InputStreamOptions = InputStreamOptions.Partial;

            StorageFolder folder = KnownFolders.PicturesLibrary;
            StorageFile file = await folder.CreateFileAsync(get_filepath(), CreationCollisionOption.ReplaceExisting);

            IRandomAccessStream _filestream = await file.OpenAsync(FileAccessMode.ReadWrite);
            IOutputStream _filewriter = _filestream.GetOutputStreamAt(0);

            _datawriter = new DataWriter(_filewriter);

            while (!((_length = await _datareader.LoadAsync(65000)) == 0))
            {
                _buffer = new byte[_length];
                _datareader.ReadBytes(_buffer);

                _transferspeed += _length;

                if (_write)
                {
                    String time = _stopwatch.Elapsed.ToString("mm\\:ss\\.ff");

                    _transferspeed /= 1024;

                    get_callback().on_transfer_speed_change(time + " " + _transferspeed.ToString() + " kB/s", get_results());

                    _transferspeed = 0;
                    _write = false;
                }

                _datawriter.WriteBytes(_buffer);
                await _datawriter.StoreAsync();
            }

            _datawriter.Dispose();
            _filewriter.Dispose();
            _filestream.Dispose();
            _datareader.Dispose();

            timer.Cancel();

            _stopwatch.Stop();

            String lasttime = _stopwatch.Elapsed.ToString("mm\\:ss\\.ff");
            get_callback().on_transfer_speed_change(lasttime + " " + _transferspeed.ToString() + " kB/s", get_results());

            sender.Dispose();
            _bluetooth_listener.Dispose();
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
