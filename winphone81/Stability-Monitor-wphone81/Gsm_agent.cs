using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Windows.Storage;
using System.IO;
using Windows.Storage.Streams;
using Windows.Networking.Connectivity;
using Windows.UI.Xaml;
using Windows.System.Threading;
using Windows.Web.Http;
using System.Diagnostics;

namespace Stability_Monitor_wphone81
{
    class Gsm_agent : Agent
    {
        private HttpClient _httpclient = new HttpClient();
        private HttpResponseMessage _httpresponse;
        private Progress<HttpProgress> _httpprogress;
        private Uri _httpurl;
        private IBuffer _buffer;
        private DataWriter _datawriter;

        private ThreadPoolTimer _timer;
        private ulong _transferspeed = 0;
        private Boolean _write;
        private Stopwatch _stopwatch = new Stopwatch();
        private String _message;

        public Gsm_agent(String filepath, Agenttype agenttype, Callback_on_status_changed callback, Results results) : base(filepath, agenttype, callback, results)
        {

        }

        public override void send_file(String devicename, String add, int not)
        {

        }
        
        public override async void receive_file(String add, int not)
        {
            try
            {
                scan_network_speed();
                _stopwatch.Start();

                _httpurl = new Uri(add);
                _httpprogress = new Progress<HttpProgress>(ProgressHandler);
                _httpresponse = await _httpclient.GetAsync(_httpurl).AsTask(new CancellationToken(), _httpprogress);

                StorageFolder folder = KnownFolders.PicturesLibrary;
                StorageFile file = await folder.CreateFileAsync(this._filepath, CreationCollisionOption.ReplaceExisting);
                IRandomAccessStream filestream = await file.OpenAsync(FileAccessMode.ReadWrite);
                IOutputStream filewriter = filestream.GetOutputStreamAt(0);
                _datawriter = new DataWriter(filewriter);

                _timer.Cancel();

                _transferspeed /= 1024;
                _message = format_message(_stopwatch.Elapsed, "Transferspeed", _transferspeed.ToString(), " kB/s");
                this._callback.on_transfer_speed_change(_message, this._results);

                _stopwatch.Stop();

                _buffer = await _httpresponse.Content.ReadAsBufferAsync();

                _datawriter.WriteBuffer(_buffer);
                await _datawriter.StoreAsync();

                _datawriter.Dispose();
                filewriter.Dispose();
                filestream.Dispose();

                _httpclient.Dispose();

                _message = format_message(_stopwatch.Elapsed, "File Transfer", "OK", this._filepath);
                this._callback.on_file_received(_message, this._results);
            }
            catch (Exception e)
            {
                append_error_tolog(e, _stopwatch.Elapsed, "");
            }            
        }

        private void ProgressHandler(HttpProgress obj)
        {

            _transferspeed += obj.BytesReceived;

            if (_write)
            {
                String time = _stopwatch.Elapsed.ToString("mm\\:ss\\.ff");

                _transferspeed /= 1024;
                _message = format_message(_stopwatch.Elapsed, "Transferspeed", _transferspeed.ToString(), " kB/s");
                this._callback.on_transfer_speed_change(_message, this._results);

                _transferspeed = 0;
                _write = false;
            }

        }

        public void scan_network_speed()
        {
            _timer = ThreadPoolTimer.CreatePeriodicTimer(update_network_speed, TimeSpan.FromSeconds(1));
        }

        public void update_network_speed(object sender)
        {
            _write = true;
        }        
    }
}
