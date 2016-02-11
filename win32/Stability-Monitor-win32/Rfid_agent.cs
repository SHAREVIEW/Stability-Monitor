using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stability_Monitor_win32
{
    class Rfid_agent : Agent
    {
        public Rfid_agent(String filepath, Agenttype agenttype, Callback_on_status_changed callback) : base(filepath, agenttype, callback)
        {

        }

        public override void send_file()
        {

        }

        public override void receive_file()
        {
            
        }
    }
}
