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
        private Testtype _testtype;
        private Results _results = new Results();
        private List<Agent> _test_agents = new List<Agent>();
        private List<Thread> _threads = new List<Thread>();

        public Test(Testtype ttype)
        {
            switch(ttype)
            {
                case Testtype.Test_1:
                    {
                        add_agent(new Wifi_agent(@"C:\Games\test3.mp3", Agenttype.Wifi_agent, new Callback_Instance(), _results));

                        _threads.Add(new Thread(() =>
                        {
                            _test_agents.ElementAt(0).receive_file("WiFi", "192.168.5.100", 5000);

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

        public Testtype get_testtype()
        {
            return _testtype;
        }

        public void set_testtype(Testtype tt)
        {
            _testtype = tt;
        }

        public Results get_results()
        {
            return _results;
        }

        public void set_results(Results rs)
        {
            _results = rs;
        }

        public List<Agent> get_agents()
        {
            return _test_agents;
        }

        public void set_agents(List<Agent> ta)
        {
            _test_agents = ta;
        }

        public void add_agent(Agent a)
        {
            _test_agents.Add(a);
        }
    }       
}
