using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace Stability_Monitor_wphone81
{
    public enum Testtype{Test_1, Test_2, Test_3, Test_4, Test_5}

    public class Test
    {
        private Testtype _testtype { get; set; }
        private Results _results { get; set; }
        private List<Agent> _test_agents { get; set; }
        private Task _task { get; set; } 
        private Main_view _main_view { get; set; }
        private CancellationTokenSource _cancel_token_source { get; set; }
        private CancellationToken _cancel_token { get; set; }

        public Test(Testtype ttype, String filepath, String wifi_or_device_name, String ip_address_or_uuid_or_url, int port, Main_view mv)
        {
            _results = new Results(ttype);
            _main_view = mv;                       

            _test_agents = new List<Agent>();            

            _cancel_token_source = new CancellationTokenSource();
            _cancel_token = _cancel_token_source.Token;            

            switch (ttype)
            {   
                case Testtype.Test_1:
                    {
                        _test_agents.Add(new Wifi_agent(filepath, Agenttype.Wifi_agent, new Callback_Instance(), _results, _main_view));

                        _task = new Task(() =>
                        {
                            _test_agents.ElementAt(0).send_file(wifi_or_device_name, ip_address_or_uuid_or_url, port);
                            
                        }, _cancel_token);

                        break;
                    }

                case Testtype.Test_2:
                    {
                        _test_agents.Add(new Wifi_agent(filepath, Agenttype.Wifi_agent, new Callback_Instance(), _results, _main_view));

                        _task = new Task(() =>
                        {
                            _test_agents.ElementAt(0).receive_file(wifi_or_device_name, ip_address_or_uuid_or_url, port);

                        }, _cancel_token);

                        break;
                    }

                case Testtype.Test_3:
                    {
                        _test_agents.Add(new Bluetooth_agent(filepath, Agenttype.Bluetooth_agent, new Callback_Instance(), _results, _main_view));

                        _task = new Task(() =>
                        {
                            _test_agents.ElementAt(0).send_file(wifi_or_device_name, ip_address_or_uuid_or_url, 0);

                        }, _cancel_token);

                        break;
                    }

                case Testtype.Test_4:
                    {
                        _test_agents.Add(new Bluetooth_agent(filepath, Agenttype.Bluetooth_agent, new Callback_Instance(), _results, _main_view));

                        _task = new Task(() =>
                        {
                            _test_agents.ElementAt(0).receive_file(wifi_or_device_name, ip_address_or_uuid_or_url, 0);

                        }, _cancel_token);

                        break;
                    }

                case Testtype.Test_5:
                    {
                        _test_agents.Add(new Gsm_agent(filepath, Agenttype.Gsm_agent, new Callback_Instance(), _results, _main_view));

                        _task = new Task(() =>
                        {
                            _test_agents.ElementAt(0).receive_file("", ip_address_or_uuid_or_url, 5000);

                        }, _cancel_token);

                        break;
                    }
            }
        }

        public void start_test()
        {
            _results.append_to_log(" " + "\t" + "Time" + "\t" + "Parameter" + "\t" + "=" + "\t" + "Value" + "\t" + "Unit" + "\r\n");

            _task.Start();         
        }

        public void stop_test()
        {
            if (_task != null && _cancel_token.CanBeCanceled)
            {
                if (_test_agents.ElementAt(0).agenttype == Agenttype.Wifi_agent)
                {
                    Wifi_agent wa = (Wifi_agent)_test_agents.ElementAt(0);
                    wa.stop_scanning();
                }
                else if (_test_agents.ElementAt(0).agenttype == Agenttype.Bluetooth_agent)
                {
                    Bluetooth_agent ba = (Bluetooth_agent)_test_agents.ElementAt(0);
                    ba.stop_scanning();
                }
                else if (_test_agents.ElementAt(0).agenttype == Agenttype.Gsm_agent)
                {
                    Gsm_agent ga = (Gsm_agent)_test_agents.ElementAt(0);
                    ga.stop_scanning();
                }

                _cancel_token_source.Cancel();
            }
        }
    }
}
