using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stability_Monitor_wphone81
{
    class Rfid_agent : Agent
    {


        public Rfid_agent(String filepath, Agenttype agenttype, Callback_on_status_changed callback, Results results, Main_view main_view) : base(filepath, agenttype, callback, results, main_view)
        {

        }

        public override void send_file(String devicename, String add, int not)
        {

        }

        public override void receive_file(String devicename, String add, int not)
        {

        }
    }
}
