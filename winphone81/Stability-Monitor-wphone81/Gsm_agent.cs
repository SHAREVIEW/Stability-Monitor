using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using System.IO;
using Windows.Storage.Streams;
using Windows.Networking.Connectivity;
using Windows.UI.Xaml;
using Windows.System.Threading;
using System.Net.Http;

namespace Stability_Monitor_wphone81
{
    class Gsm_agent : Agent
    {

        public Gsm_agent(String filepath, Agenttype agenttype, Callback_on_status_changed callback, Results results) : base(filepath, agenttype, callback, results)
        {

        }

        public override void send_file(String add, int not)
        {

        }

        public override void receive_file(String add, int not)
        {
        }
    }
}
