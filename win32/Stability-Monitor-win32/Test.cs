using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Stability_Monitor_win32
{
    public enum Testtype{Test_1, Test_2, Test_3}

    class Test
    {
        private Testtype _testtype { get; set; }
        private Results _results { get; set; } = new Results();
        private List<Agent> _test_agents { get; set; } = new List<Agent>();
        private List<Thread> _threads { get; set; } = new List<Thread>();

        public Test(Testtype ttype)
        {
            switch(ttype)
            {
                case Testtype.Test_1:
                    {
                        _test_agents.Add(new Bluetooth_agent(@"C:\Games\test3.mp3", Agenttype.Bluetooth_agent, new Callback_Instance(), _results));

                        _threads.Add(new Thread(() =>
                        {
                            _test_agents.ElementAt(0).receive_file("", "34B1CF4D-1069-4AD6-89B6-E161D79BE4D8", 5000);

                        }));

                        break;
                    }

                case Testtype.Test_2:
                    {

                        break;
                    }

                case Testtype.Test_3:
                    {

                        break;
                    }
            }
        }

        public void start_test()
        {
            foreach (Thread t in _threads)
            {
                t.Start();
            }
        }       
    }       
}
