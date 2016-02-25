using System;
using System.Collections.Generic;
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
using Windows.UI.Xaml;

namespace Stability_Monitor_wphone81
{
    class Bluetooth_agent : Agent
    {
        private RfcommDeviceService _rfcomm_service;
        private StreamSocket _bluetooth_client;
        private DataWriter _datawriter;
        private DataReader _datareader;
        private byte[] _tosend;

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

        public override void receive_file(String bluid, int not)
        {

        }
    }
}
