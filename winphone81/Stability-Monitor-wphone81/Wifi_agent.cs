using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking;
using Windows.Networking.Sockets;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.System.Threading;

namespace Stability_Monitor_wphone81
{
    class Wifi_agent : Agent
    {
        private StreamSocketListener _tcplistener = new StreamSocketListener();
        private StreamSocket _tcpclient;
        private DataReader _datareader;
        private DataWriter _datawriter;
        private byte[] _tosend;
        private uint _length;
        private byte[] _buffer;

        private bool _write = false;
        private uint _transferspeed = 0;
        private ThreadPoolTimer _timer;
        private Stopwatch _stopwatch = new Stopwatch();
        private String _message;

        public Wifi_agent(String filepath, Agenttype agenttype, Callback_on_status_changed callback, Results results) : base(filepath, agenttype, callback, results) { }

        public override void send_file(String devicename, String ipadd, int port)
        {
            send_file_tcp(ipadd, port);
        }

        public override void receive_file(String ipadd, int port)
        {
            receive_file_tcp(port);
        }

        private async void send_file_tcp(String ipadd, int port)
        {
            try {
                _tcpclient = new StreamSocket();

                await _tcpclient.ConnectAsync(new HostName(ipadd), port.ToString());
                _datawriter = new DataWriter(_tcpclient.OutputStream);

                StorageFolder folder = KnownFolders.PicturesLibrary;
                StorageFile file = await folder.GetFileAsync(this._filepath);

                IRandomAccessStreamWithContentType filestream = await file.OpenReadAsync();
                _tosend = new byte[filestream.Size];

                _datareader = new DataReader(filestream);
                await _datareader.LoadAsync((uint)filestream.Size);
                _datareader.ReadBytes(_tosend);

                _datawriter.WriteBytes(_tosend);
                await _datawriter.StoreAsync();

                filestream.Dispose();
                _datareader.Dispose();
                _datawriter.Dispose();

                _tcpclient.Dispose();
            }
            catch (Exception e)
            {

            }            
        }

        private async void receive_file_tcp(int port)
        {
            _tcplistener.ConnectionReceived += _tcp_ConnectionReceived;
            await _tcplistener.BindServiceNameAsync(port.ToString());            
        }

        private async void _tcp_ConnectionReceived(StreamSocketListener sender, StreamSocketListenerConnectionReceivedEventArgs args)
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
                        _message = format_message(_stopwatch.Elapsed, "Transferspeed", _transferspeed.ToString(), " kB/s");
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

                _timer.Cancel();

                _transferspeed /= 1024;
                _message = format_message(_stopwatch.Elapsed, "Transferspeed", _transferspeed.ToString(), " kB/s");
                this._callback.on_transfer_speed_change(_message, this._results);

                _stopwatch.Stop();

                sender.Dispose();
            }
            catch (Exception e)
            {

            }
        }
        
        private void scan_transfer_speed()
        {
            _timer = ThreadPoolTimer.CreatePeriodicTimer(update_network_speed, TimeSpan.FromSeconds(1));
        }

        private void update_network_speed(ThreadPoolTimer timer)
        {
            _write = true;
        }
    }
}
