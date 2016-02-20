using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stability_Monitor_wphone81
{
    public enum Agenttype { Wifi_agent, Bluetooth_agent, Gsm_agent, Rfid_agent, Nfc_agent };

    abstract class Agent
    {
        private String _filepath;
        private Agenttype _agenttype;
        private Callback_on_status_changed _callback;
        private Results _results;

        protected Agent(String filepath, Agenttype agenttype, Callback_on_status_changed callback, Results results)
        {
            set_filepath(filepath);
            set_agenttype(agenttype);
            set_callback(callback);
            set_results(results);
        }

        public abstract void send_file(String address, int port);
        public abstract void receive_file(String address, int port);

        public String get_filepath()
        {
            return _filepath;
        }

        public void set_filepath(String fp)
        {
            _filepath = fp;
        }

        public Agenttype get_agenttype()
        {
            return _agenttype;
        }

        public void set_agenttype(Agenttype at)
        {
            _agenttype = at;
        }

        public Callback_on_status_changed get_callback()
        {
            return _callback;
        }

        public void set_callback(Callback_on_status_changed cb)
        {
            _callback = cb;
        }

        public Results get_results()
        {
            return _results;
        }

        public void set_results(Results rs)
        {
            _results = rs;
        }
    }
}
