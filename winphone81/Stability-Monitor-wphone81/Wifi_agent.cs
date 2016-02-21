using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking;
using Windows.Networking.Sockets;
using Windows.Storage;
using Windows.Storage.Streams;

namespace Stability_Monitor_wphone81
{
    class Wifi_agent : Agent
    {
        private StreamSocketListener _tcplistener = new StreamSocketListener();
        private StreamSocket _tcpclient;
        private DatagramSocket _udpclient;
        private DataReader _datareader;
        private DataWriter _datawriter;
        private byte[] _tosend;
        private uint _length;
        private byte[] _buffer;
        private byte[] _filebuffer;

        public Wifi_agent(String filepath, Agenttype agenttype, Callback_on_status_changed callback, Results results) : base(filepath, agenttype, callback, results) { }

        public override void send_file(String ipadd, int port)
        {
            send_file_tcp(ipadd, port);
        }

        public override void receive_file(String ipadd, int port)
        {
            receive_file_tcp(port);
        }

        private async void send_file_tcp(String ipadd, int port)
        {
            _tcpclient = new StreamSocket();

            await _tcpclient.ConnectAsync(new HostName(ipadd), port.ToString());
            _datawriter = new DataWriter(_tcpclient.OutputStream);

            StorageFolder folder = KnownFolders.PicturesLibrary;
            StorageFile file = await folder.GetFileAsync(get_filepath());

            IRandomAccessStreamWithContentType filestream = await file.OpenReadAsync();
            _tosend = new byte[filestream.Size];

            _datareader = new DataReader(filestream);
            await _datareader.LoadAsync((uint) filestream.Size);
            _datareader.ReadBytes(_tosend);

            _datawriter.WriteBytes(_tosend);
            await _datawriter.StoreAsync();

            _datareader.Dispose();
            _datawriter.Dispose();

            _tcpclient.Dispose();            
        }

        private async void receive_file_tcp(int port)
        {
            _tcplistener.ConnectionReceived += _tcp_ConnectionReceived;
            await _tcplistener.BindServiceNameAsync(port.ToString());
        }

        private async void _tcp_ConnectionReceived(StreamSocketListener sender, StreamSocketListenerConnectionReceivedEventArgs args)
        {
            _datareader = new DataReader(args.Socket.InputStream);
            _datareader.InputStreamOptions = InputStreamOptions.Partial;

            StorageFolder folder = KnownFolders.PicturesLibrary;
            StorageFile file = await folder.CreateFileAsync(get_filepath(), CreationCollisionOption.ReplaceExisting);

            IRandomAccessStream _filestream = await file.OpenAsync(FileAccessMode.ReadWrite);
            IOutputStream _filewriter = _filestream.GetOutputStreamAt(0);

            _datawriter = new DataWriter(_filewriter);
                        
            while (!((_length = await _datareader.LoadAsync(1500)) == 0))
            {
                _buffer = new byte[_length];
                _datareader.ReadBytes(_buffer);

                _datawriter.WriteBytes(_buffer);
                await _datawriter.StoreAsync();
                                             
            }
                                   
            _datawriter.Dispose();
            _filewriter.Dispose();
            _filestream.Dispose();
            _datareader.Dispose();

            sender.Dispose();           
        }
    }
}
