using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stability_Monitor_wphone81
{
    public enum Testtype{Test_1, Test_2, Test_3}

    class Test
    {
        private Testtype _testtype { get; set; }
        private Results _results { get; set; } = new Results();
        private List<Agent> _test_agents { get; set; } = new List<Agent>();
        private List<Task> _tasks { get; set; } = new List<Task>();

        public Test(Testtype ttype)
        {
            switch (ttype)
            {
                case Testtype.Test_1:
                    {
                        _test_agents.Add(new Bluetooth_agent("test3.mp3", Agenttype.Bluetooth_agent, new Callback_Instance(), _results));
                        _tasks.Add(new Task(() =>
                        {
                            _test_agents.ElementAt(0).send_file("TOM", "34B1CF4D-1069-4AD6-89B6-E161D79BE4D8", 5000);
                        }
                        ));

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
            foreach (Task t in _tasks)
            {
                t.Start();
            }
        }                
    }
}
