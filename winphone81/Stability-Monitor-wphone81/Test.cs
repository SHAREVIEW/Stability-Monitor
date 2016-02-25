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
        private Testtype _testtype;
        private Results _results = new Results();
        private List<Agent> _test_agents = new List<Agent>();
        private List<Task> _tasks = new List<Task>();

        public Test(Testtype ttype)
        {
            switch (ttype)
            {
                case Testtype.Test_1:
                    {
                        _test_agents.Add(new Bluetooth_agent("test1.txt", Agenttype.Bluetooth_agent, new Callback_Instance(), _results));

                        _tasks.Add(new Task(() =>
                        {
                            _test_agents.ElementAt(0).send_file("34B1CF4D-1069-4AD6-89B6-E161D79BE4D8", 5000);
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
            _tasks.ElementAt(0).Start();
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
