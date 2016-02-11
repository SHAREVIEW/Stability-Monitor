using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stability_Monitor_win32
{
    class Bluetooth_agent : Agent
    {
        public Bluetooth_agent(String filepath, Agenttype agenttype, Callback_on_status_changed callback, Results results) : base(filepath, agenttype, callback, results)
        {

        }

        public override void send_file()
        {

        }

        public override void receive_file()
        {
            this.get_callback().on_file_received(get_filepath(), "subor uspesne prijaty", get_results());
        }
    }
}
