using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Stability_Monitor_win32
{
    public enum Agenttype { Wifi_agent, Bluetooth_agent, Gsm_agent, Rfid_agent, Nfc_agent };

    abstract class Agent
    {
        public String _filepath { get; set; }
        public Agenttype _agenttype { get; set; }
        public Callback_on_status_changed _callback { get; set; }
        public Results _results { get; set; }

        protected Agent(String filepath, Agenttype agenttype ,Callback_on_status_changed callback, Results results)
        {
            this._filepath = filepath;
            this._agenttype = agenttype;
            this._callback = callback;
            this._results = results;
        }
                
        public abstract void send_file(String devicename, String address, int port);
        public abstract void receive_file(String devicename, String address, int port);

        public String format_message(TimeSpan time, String parameter, String value, String unit)
        {
            return this._agenttype.ToString() + "\t" + time.ToString("mm\\:ss\\.ff") + "\t" + parameter + "\t" + "=" + "\t" + value + "\t" + unit +"\r\n";
        }
    }
}
