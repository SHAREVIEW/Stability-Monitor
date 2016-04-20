using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Stability_Monitor_win32
{
    public enum Agenttype { Wifi_agent, Bluetooth_agent, Gsm_agent, Rfid_agent, Nfc_agent };

    abstract class Agent
    {
        public String filepath { get; set; }
        public Agenttype agenttype { get; set; }
        public Callback_on_status_changed callback { get; set; }
        public Results results { get; set; }
        public Main_view main_view;

        protected Agent(String filepath, Agenttype agenttype, Callback_on_status_changed callback, Results results, Main_view mv)
        {
            this.filepath = filepath;
            this.agenttype = agenttype;
            this.callback = callback;
            this.results = results;
            this.main_view = mv;
        }
                
        public abstract void send_file(String devicename, String address, int port);
        public abstract void receive_file(String devicename, String address, int port);

        public String format_message(TimeSpan time, String parameter, String value, String unit)
        {
            return this.agenttype.ToString() + "\t" + time.ToString("mm\\:ss\\.ff") + "\t" + parameter + "\t" + "=" + "\t" + value + "\t" + unit +"\r\n";
        }

        public void append_error_tolog(Exception e, TimeSpan ts, String devicename)
        {
            String _message;

            if (e is SocketException)
            {
                _message = format_message(ts, "File Transfer", "NOK", "Connection error.");
                this.callback.on_file_transfer_error(_message, this.results);
                this.main_view.text_to_logs(_message);
            }
            else if (e is FileNotFoundException)
            {
                _message = format_message(ts, "File Transfer", "NOK", "Error '" + this.filepath + "' not found.");
                this.callback.on_file_transfer_error(_message, this.results);
                this.main_view.text_to_logs(_message);
            }
            else if (e is ArgumentNullException && !devicename.Equals(""))
            {
                _message = format_message(ts, "File Transfer", "NOK", "Error '" + devicename + "' not found.");
                this.callback.on_file_transfer_error(_message, this.results);
                this.main_view.text_to_logs(_message);
            }
            else if (e is PlatformNotSupportedException)
            {
                _message = format_message(ts, "File Transfer", "NOK", "Error adapter is not connected.");
                this.callback.on_file_transfer_error(_message, this.results);
                this.main_view.text_to_logs(_message);
            }
            else
            {
                _message = format_message(ts, "File Transfer", "NOK", "Unknown error.");
                this.callback.on_file_transfer_error(_message, this.results);
                this.main_view.text_to_logs(_message);
            }
        }
    }
}
