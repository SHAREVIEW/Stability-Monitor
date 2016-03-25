using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stability_Monitor_wphone81
{
    public enum Agenttype { Wifi_agent, Bluetooth_agent, Gsm_agent, Rfid_agent, Nfc_agent };

    abstract class Agent
    {
        public String _filepath { get; set; }
        public Agenttype _agenttype { get; set; }
        public Callback_on_status_changed _callback { get; set; }
        public Results _results { get; set; }

        protected Agent(String filepath, Agenttype agenttype, Callback_on_status_changed callback, Results results)
        {
            this._filepath = filepath;
            this._agenttype = agenttype;
            this._callback = callback;
            this._results = results;
        }

        public abstract void send_file(String devicename, String address, int port);
        public abstract void receive_file(String address, int port);

        public String format_message(TimeSpan time, String parameter, String value, String unit)
        {
            return this._agenttype.ToString() + "\t" + time.ToString("mm\\:ss\\.ff") + "\t" + parameter + "\t" + "=" + "\t" + value + "\t" + unit + "\r\n";
        }

        public void append_error_tolog(Exception e, TimeSpan ts, String devicename)
        {
            String _message;

            if (e is NullReferenceException)
            {
                _message = format_message(ts, "File Transfer", "NOK", "Connection error.");
                this._callback.on_file_received(_message, this._results);
            }
            else if (e is FileNotFoundException)
            {
                _message = format_message(ts, "File Transfer", "NOK", "Error '" + this._filepath + "' not found.");
                this._callback.on_file_received(_message, this._results);
            }
            else if (e is ArgumentNullException && !devicename.Equals(""))
            {
                _message = format_message(ts, "File Transfer", "NOK", "Error '" + devicename + "' not found.");
                this._callback.on_file_received(_message, this._results);
            }
            else
            {
                _message = format_message(ts, "File Transfer", "NOK", "Unknown error.");
                this._callback.on_file_received(_message, this._results);
            }
        }
    }
}
