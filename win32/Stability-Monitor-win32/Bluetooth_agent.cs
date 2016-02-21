using InTheHand.Net;
using InTheHand.Net.Sockets;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Stability_Monitor_win32
{
    class Bluetooth_agent : Agent
    {
        private BluetoothClient _bluetooth_client;
        private BluetoothAddress _bluetooth_adress;
        private NetworkStream _netstream;
        private Guid _bluetooth_guid;
        private Stream _filestream;
        private BluetoothListener _bluetooth_listener;

        private System.Timers.Timer _timer_ts = new System.Timers.Timer();
        private int _receivebytes = 0;
        private Stopwatch _stopwatch_ts = new Stopwatch();

        public Bluetooth_agent(String filepath, Agenttype agenttype, Callback_on_status_changed callback, Results results) : base(filepath, agenttype, callback, results) { }

        public override void send_file(String bluid, int not)
        {
            _bluetooth_client = new BluetoothClient();

            BluetoothDeviceInfo[] devinfos = _bluetooth_client.DiscoverDevices();

            _bluetooth_adress = devinfos[0].DeviceAddress;

            _bluetooth_guid = Guid.Parse(bluid);

            _bluetooth_client.Connect(_bluetooth_adress, _bluetooth_guid);

            _netstream = _bluetooth_client.GetStream();

            byte[] dataToSend = File.ReadAllBytes(get_filepath());

            _netstream.Write(dataToSend, 0, dataToSend.Length);
            _netstream.Flush();

            _netstream.Dispose();
            _netstream.Close();

            _bluetooth_client.Dispose();
            _bluetooth_client.Close();
        }

        public override void receive_file(String bluid, int not)
        {            
            _bluetooth_guid = Guid.Parse(bluid);
            _bluetooth_listener = new BluetoothListener(_bluetooth_guid);
            _bluetooth_listener.Start();

            _bluetooth_client = _bluetooth_listener.AcceptBluetoothClient();
            _netstream = _bluetooth_client.GetStream();
                        
            _filestream = new FileStream(get_filepath(), FileMode.Create, FileAccess.ReadWrite);

            int length;
            Byte[] bytes = new Byte[256];

            scan_transfer_speed();
            _stopwatch_ts.Start();

            while ((length = _netstream.Read(bytes, 0, bytes.Length)) != 0)
            {
                _receivebytes += length;

                _filestream.Write(bytes, 0, length);
            }
            
            _filestream.Dispose();
            _filestream.Close();

            _netstream.Dispose();
            _netstream.Close();

            _bluetooth_client.Dispose();
            _bluetooth_client.Close();
        }

        private void check_transfer_speed(object sender, EventArgs e)
        {
            String time = _stopwatch_ts.Elapsed.ToString("mm\\:ss\\.ff");

            int transferspeed = _receivebytes / 1024;

            get_callback().on_transfer_speed_change(time + " " + transferspeed.ToString() + " kB/s", get_results());

        }

        private void scan_transfer_speed()
        {
            _timer_ts.Interval = 1000;
            _timer_ts.Elapsed += new ElapsedEventHandler(check_transfer_speed);
            _timer_ts.Start();
        }
    }
}
