using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stability_Monitor_win32
{
    class Nfc_agent : Agent
    {
        public Nfc_agent(String filepath, Agenttype agenttype, Callback_on_status_changed callback, Results results, Main_view mv) : base(filepath, agenttype, callback, results, mv)
        {

        }

        public override void send_file(String devicename, String ipadd, int not)
        {

        }

        public override void receive_file(String devicename, String ipadd, int not)
        {

        }
    }
}
