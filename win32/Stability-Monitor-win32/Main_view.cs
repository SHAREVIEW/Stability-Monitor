using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Stability_Monitor_win32
{
    public partial class Main_view : Form
    {
        public Main_view()
        {
            InitializeComponent();                        
        }

        public Almighty_controller almighty_controller = new Almighty_controller();
        public List<Test> tests = new List<Test>();
        public int selected_index;

        private void Start_tests_bt_Click(object sender, EventArgs e)
        {
            if (tests.Count != 0)
            {
                Logs_tb.Text = "";
                Logs_tb.AppendText("Tests started...\r\n");
                Logs_tb.AppendText(" " + "\t" + "Time" + "\t" + "Parameter" + "\t" + "=" + "\t" + "Value" + "\t" + "Unit" + "\r\n");
                Logs_tb.Refresh();
                almighty_controller.shedule_tests(tests);
            }
        }

        private void Stop_tests_bt_Click(object sender, EventArgs e)
        {
            if (tests.Count != 0)
            {                
                almighty_controller.stop_tests(this);

                Logs_tb.AppendText("Tests stopped.\r\n");
                Logs_tb.Refresh();

                List_of_tests_tb.Text = "";
                List_of_tests_tb.Refresh();
            }
        }

        private void Add_new_test_cb_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (Add_new_test_cb.SelectedIndex)
            {
                case 0:
                    {
                        selected_index = 1;
                        Setup_view_wifi sv = new Setup_view_wifi(this);                        
                        sv.Show();

                        break;
                    }
                case 1:
                    {
                        selected_index = 2;
                        Setup_view_wifi sv = new Setup_view_wifi(this);
                        sv.Show();

                        break;
                    }
                case 2:
                    {
                        selected_index = 3;
                        Setup_view_bluetooth sv = new Setup_view_bluetooth(this);
                        sv.Show();

                        break;
                    }
                case 3:
                    {
                        selected_index = 4;
                        Setup_view_bluetooth sv = new Setup_view_bluetooth(this);
                        sv.Show();

                        break;
                    }
            }
        }

        private void Clear_list_of_tests_bt_Click(object sender, EventArgs e)
        {
            List_of_tests_tb.Text = "";
            tests.Clear();
        }

        public void add_new_test(int i, String filepath, String wifi_or_device_name, String ip_address_or_uuid, int port)
        {
            switch (i)
            {
                case 1:
                    {
                        tests.Add(new Test(Testtype.Test_1, filepath, wifi_or_device_name, ip_address_or_uuid, port, this));
                        List_of_tests_tb.Text += "Test 1 - Wi-Fi/Send\r\n";

                        break;
                    }
                case 2:
                    {
                        tests.Add(new Test(Testtype.Test_2, filepath, wifi_or_device_name, ip_address_or_uuid, port, this));
                        List_of_tests_tb.Text += "Test 2 - Wi-Fi/Receive\r\n";

                        break;
                    }
                case 3:
                    {
                        tests.Add(new Test(Testtype.Test_3, filepath, wifi_or_device_name, ip_address_or_uuid, 0, this));
                        List_of_tests_tb.Text += "Test 3 - Bluetooth/Send\r\n";

                        break;
                    }
                case 4:
                    {
                        tests.Add(new Test(Testtype.Test_4, filepath, wifi_or_device_name, ip_address_or_uuid, 0, this));
                        List_of_tests_tb.Text += "Test 4 - Bluetooth/Receive\r\n";

                        break;
                    }
            }
        }

        public void text_to_logs(string text)
        {
            this.Invoke((MethodInvoker)delegate ()
            {
                this.Logs_tb.AppendText(text);
                this.Logs_tb.Refresh();
            });
        }

        public void clear_list_of_tests()
        {
            this.Invoke((MethodInvoker)delegate ()
            {
                this.List_of_tests_tb.AppendText("");
                this.List_of_tests_tb.Refresh();
            });
        }
    }
}
