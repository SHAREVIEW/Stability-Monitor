using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using System.IO;
using Windows.Storage.Streams;
using Windows.Networking.Connectivity;
using Windows.UI.Xaml;
using Windows.System.Threading;
using System.Net.Http;

namespace Stability_Monitor_wphone81
{
    class Gsm_agent : Agent
    {
        private ThreadPoolTimer timer;
        private uint received_bytes = 0;
        private int i = 0;

        public Gsm_agent(String filepath, Agenttype agenttype, Callback_on_status_changed callback, Results results) : base(filepath, agenttype, callback, results)
        {

        }

        public override void send_file(String add, int not)
        {

        }

        public override async void receive_file(String add, int not)
        {
            HttpClientHandler hndlr = new HttpClientHandler();
            hndlr.ClientCertificateOptions = ClientCertificateOption.Automatic;                          
            HttpClient _httpclient = new HttpClient(hndlr);
            _httpclient.DefaultRequestHeaders.ExpectContinue = false;

            StorageFolder folder = KnownFolders.PicturesLibrary;
            StorageFile file = await folder.CreateFileAsync(get_filepath(), CreationCollisionOption.ReplaceExisting);

            IRandomAccessStream _filestream = await file.OpenAsync(FileAccessMode.ReadWrite);
            IOutputStream _filewriter = _filestream.GetOutputStreamAt(0);
            DataWriter _datawriter = new DataWriter(_filewriter);

            StorageFile file2 = await folder.CreateFileAsync("speed2.txt", CreationCollisionOption.ReplaceExisting);
            file2 = await folder.GetFileAsync("speed2.txt");

            await FileIO.AppendTextAsync(file2, "Time: " + DateTime.Now + "\r\n");

            var rsp = await _httpclient.GetAsync("http://cdl5.convert2mp3.net/download.php?id=youtube_dJC-UyGhGYs&key=YP4PAXS93j0m&d=y", HttpCompletionOption.ResponseHeadersRead);

            await FileIO.AppendTextAsync(file2, "Time: " + DateTime.Now + "\r\n");

            var s = await rsp.Content.ReadAsStreamAsync();
            IInputStream instr = s.AsInputStream();

            await FileIO.AppendTextAsync(file2, "Time: " + DateTime.Now + "\r\n");

            //manual_check_network_speed();
            //scan_network_speed();

            while (true)
            {
                var buffer = new Windows.Storage.Streams.Buffer(18000);

                await FileIO.AppendTextAsync(file2, "Time: " + DateTime.Now + "\r\n");
                
                var read = await instr.ReadAsync(buffer, buffer.Capacity, InputStreamOptions.None);

                await FileIO.AppendTextAsync(file2, "Time: " + DateTime.Now + "\r\n");

                if (read.Length == 0)
                {
                    break;
                }

                received_bytes += read.Length;

                await FileIO.AppendTextAsync(file2, "Time: " + DateTime.Now + " Received bytes: " + received_bytes + "\r\n");
                
                _datawriter.WriteBuffer(buffer, 0, read.Length);
                await _datawriter.StoreAsync();
            }
                       
            //timer.Cancel();

            _datawriter.Dispose();
            _filewriter.Dispose();
            _filestream.Dispose();
            s.Dispose();

            /*HttpRequestMessage message = new HttpRequestMessage();
            message.RequestUri = new Uri("http://www.fiit.stuba.sk/docs//Studium/SP2015-16.pdf");
            message.Method = 

            //HttpResponseMessage message = await _httpclient.GetAsync("http://www.coolhdwallpapersjar.com/wp-content/uploads/2015/11/best_full-_hd-_pictures.jpg");
            Stream http_stream = await _httpclient.GetStreamAsync("http://www.fiit.stuba.sk/docs//Studium/SP2015-16.pdf");

            scan_network_speed();

            StorageFolder folder = KnownFolders.PicturesLibrary;
            StorageFile file = await folder.CreateFileAsync(get_filepath(), CreationCollisionOption.ReplaceExisting);

            //byte[] file_buffer = await message.Content.ReadAsByteArrayAsync();


            IRandomAccessStream _filestream = await file.OpenAsync(FileAccessMode.ReadWrite);
            IOutputStream _filewriter = _filestream.GetOutputStreamAt(0);
            DataWriter _datawriter = new DataWriter(_filewriter);

            //Stream http_stream = await msg.Content.ReadAsStreamAsync().ConfigureAwait(false);

            byte[] _buffer = new byte[128];

            int _length;

            //_datareader.ReadBytes(_buffer);



            while (!((_length = http_stream.Read(_buffer, 0, _buffer.Length)) == 0))
            {
                received_bytes += _length;

                _datawriter.WriteBytes(_buffer);
                await _datawriter.StoreAsync();

            }

            _datawriter.Dispose();
            _filewriter.Dispose();
            _filestream.Dispose();

            http_stream.Dispose();

            await FileIO.WriteBytesAsync(file, file_buffer);*/



        }

        public void scan_network_speed()
        {

            timer = ThreadPoolTimer.CreatePeriodicTimer(check_network_speed, TimeSpan.FromSeconds(1));

        }

        public async void check_network_speed(object sender)
        {
            //StorageFolder folder = KnownFolders.PicturesLibrary;
            //StorageFile file = await folder.GetFileAsync("speed.txt");

            /*ConnectionProfile profile = NetworkInformation.GetInternetConnectionProfile();
                        
            var currtime = DateTime.Now;
            var starttime = currtime - TimeSpan.FromDays(10);

            var states = new NetworkUsageStates();
            states.Roaming = TriStates.Yes;
            var netusage = await profile.GetNetworkUsageAsync(starttime, currtime, DataUsageGranularity.Total, states);

            await FileIO.AppendTextAsync(file, i + ".\r\n");
            i++;
            await FileIO.AppendTextAsync(file, "Sending speed: " + netusage[0].BytesSent + "\r\n");
            await FileIO.AppendTextAsync(file, "Receiving speed: " + netusage[0].BytesReceived + "\r\n");
            */

           // await FileIO.AppendTextAsync(file, "Time: "+ DateTime.Now + " Receiving speed: " + received_bytes + "\r\n");
            
            received_bytes = 0;
        }

        public async void manual_check_network_speed()
        {
            //StorageFolder folder = KnownFolders.PicturesLibrary;
            //StorageFile file = await folder.CreateFileAsync("speed.txt", CreationCollisionOption.ReplaceExisting);
            //file = await folder.GetFileAsync("speed.txt");
                        
            //await FileIO.AppendTextAsync(file, "Time: " + DateTime.Now + " Receiving speed: " + received_bytes + "\r\n");
            //i++;
            received_bytes = 0;

        }
    }
}
