using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Stability_Monitor_win32
{
    class Wifi_agent : Agent
    {
        private TcpListener _tcplistener = new TcpListener(IPAddress.Any, 5000);
        private TcpClient _tcpclient = new TcpClient();
        private UdpClient _udpclient = new UdpClient();
        private NetworkStream _netstream;
        private Stream _stream;
        private Byte[] _buffer;
        private byte[] _tosend;
        private int _length;
        
        public Wifi_agent(String filepath, Agenttype agenttype, Callback_on_status_changed callback, Results results) : base(filepath, agenttype, callback, results)
        {
            
        }

        public override void send_file()
        {
            
        }

        public override void receive_file()
        {  
            //this.get_callback().on_file_received(get_filepath(), "subor uspesne prijaty", get_results());
        }

        private void send_file_TCP()
        {            
            _tcpclient.Connect(IPAddress.Parse("10.10.10.1"), 5000);
            _netstream = _tcpclient.GetStream();

            _tosend = File.ReadAllBytes(get_filepath());

            _netstream.Write(_tosend, 0, _tosend.Length);
            _netstream.Flush();

            _netstream.Dispose();
            _netstream.Close();

            _tcpclient.Close();
            
        }

        private void receive_file_TCP()
        {
            _buffer = new byte[1500];

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

        private void send_file_UDP()
        {
            IPEndPoint ipep = new IPEndPoint(IPAddress.Parse("10.10.10.1"), 10000);

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

        private void receive_file_UDP()
        {
            UdpClient udpclient = new UdpClient(10000);
            IPEndPoint ipep = new IPEndPoint(IPAddress.Any, 10000);

            _stream = new FileStream(get_filepath(), FileMode.Create, FileAccess.ReadWrite);

            while ((_length = (_buffer = _udpclient.Receive(ref ipep)).Length) != 0)
            {
                _stream.Write(_buffer, 0, _length);
            }

            _stream.Dispose();
            _stream.Close();

            udpclient.Close();
        }


    }
}
