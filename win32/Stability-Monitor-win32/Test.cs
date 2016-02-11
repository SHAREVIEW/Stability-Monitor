using System;
using System.Collections.Generic;
using System.Linq;
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


        public Test(Testtype ttype)
        {
            switch(ttype)
            {
                case Testtype.Test_1:
                    {
                        add_agent(new Wifi_agent("some filepath for Wifi", new Agenttype(), new Callback_Instance(), _results));
                        add_agent(new Bluetooth_agent("some filepath for Bluetooth", new Agenttype(), new Callback_Instance(), _results));

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
            List<Thread> threads = new List<Thread>();

            for (int i = 0; i < _test_agents.Count(); i++)
            {
                threads.Add(new Thread(()=>{
                    Monitor.Enter(_results);
                    _test_agents.ElementAt(i).receive_file();
                    Monitor.Exit(_results);
                }));
            }

            foreach(Thread t in threads)
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
