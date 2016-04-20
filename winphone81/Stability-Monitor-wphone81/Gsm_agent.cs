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
using System.Net;
using Windows.Web.Http.Filters;

namespace Stability_Monitor_wphone81
{
    class Gsm_agent : Agent
    {
        private HttpClient _httpclient;
        private HttpResponseMessage _httpresponse;
        private Progress<HttpProgress> _httpprogress;
        private CancellationTokenSource _cancel_token_source;
        private CancellationToken _cancel_token;
        private Uri _httpurl;
        private IBuffer _buffer;
        private DataWriter _datawriter;

        private ThreadPoolTimer _timer;
        private ulong _transferspeed = 0;
        private ulong _old_bytes_received = 0;
        private int _times_count = 0;
        private Boolean _write = false;
        private Stopwatch _stopwatch = new Stopwatch();
        private String _message;

        public Gsm_agent(String filepath, Agenttype agenttype, Callback_on_status_changed callback, Results results, Main_view main_view) : base(filepath, agenttype, callback, results, main_view)
        {

        }

        public override void send_file(String devicename, String add, int not)
        {

        }
        
        public override async void receive_file(String devicename, String add, int not)
        {
            try
            {
                _httpurl = new Uri(add);
                _httpprogress = new Progress<HttpProgress>(ProgressHandler);

                HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("GET"), _httpurl);
                                
                HttpBaseProtocolFilter filter = new HttpBaseProtocolFilter();
                filter.CacheControl.ReadBehavior = HttpCacheReadBehavior.MostRecent;
                filter.CacheControl.WriteBehavior = HttpCacheWriteBehavior.NoCache;

                _httpclient = new HttpClient(filter);

                _cancel_token_source = new CancellationTokenSource();
                _cancel_token = _cancel_token_source.Token;

                scan_network_speed();
                _stopwatch.Start();

                _httpresponse = await _httpclient.SendRequestAsync(request).AsTask(_cancel_token, _httpprogress);

                StorageFolder folder = KnownFolders.PicturesLibrary;
                StorageFile file = await folder.CreateFileAsync(this.filepath, CreationCollisionOption.ReplaceExisting);
                IRandomAccessStream filestream = await file.OpenAsync(FileAccessMode.ReadWrite);
                IOutputStream filewriter = filestream.GetOutputStreamAt(0);
                _datawriter = new DataWriter(filewriter);

                _timer.Cancel();

                _transferspeed /= 1024;
                _message = format_message(_stopwatch.Elapsed, "Transferspeed", _transferspeed.ToString(), " kB/s");
                this.callback.on_transfer_speed_change(_message, this.results);
                this.main_view.text_to_logs(_message.Replace("\t", " "));

                _stopwatch.Stop();

                _buffer = await _httpresponse.Content.ReadAsBufferAsync();

                _datawriter.WriteBuffer(_buffer);
                await _datawriter.StoreAsync();

                _datawriter.Dispose();
                filewriter.Dispose();
                filestream.Dispose();

                _httpresponse.Content.Dispose();
                _httpresponse.Dispose();
                _httpclient.Dispose();

                _message = format_message(_stopwatch.Elapsed, "File Transfer", "OK", this.filepath);
                this.callback.on_file_received(_message, this.results);
                this.main_view.text_to_logs(_message.Replace("\t", " "));
            }
            catch (Exception e)
            {
                append_error_tolog(e, _stopwatch.Elapsed, "");
            }            
        }

        private void ProgressHandler(HttpProgress obj)
        {
            _transferspeed += obj.BytesReceived - _old_bytes_received;
            _old_bytes_received = obj.BytesReceived;

            if (_write && _transferspeed != 0)
            {
                _write = false;
                _transferspeed /= (ulong) (1024 * _times_count);
                _times_count = 0;
                _message = format_message(_stopwatch.Elapsed, "Transferspeed", _transferspeed.ToString(), " kB/s");
                this.callback.on_transfer_speed_change(_message, this.results);
                this.main_view.text_to_logs(_message.Replace("\t", " "));

                _transferspeed = 0;                
            }

        }

        public void scan_network_speed()
        {
            _timer = ThreadPoolTimer.CreatePeriodicTimer(update_network_speed, TimeSpan.FromSeconds(1));
        }

        public void update_network_speed(object sender)
        {
            _times_count++;
            _write = true;
        }
        
        public void stop_scanning()
        {
            if (_cancel_token.CanBeCanceled)
            {
                _cancel_token_source.Cancel();
                _timer.Cancel();
            }          
        }        
    }
}
