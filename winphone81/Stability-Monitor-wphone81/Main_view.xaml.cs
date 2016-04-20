using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace Stability_Monitor_wphone81
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Main_view : Page
    {
        public Main_view()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;            
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: Prepare page for display here.

            // TODO: If your application contains multiple pages, ensure that you are
            // handling the hardware Back button by registering for the
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed event.
            // If you are using the NavigationHelper provided by some templates,
            // this event is handled for you.
        }

        public Almighty_controller almighty_controller = new Almighty_controller();
        public List<Test> tests = new List<Test>();
        public int selected_index;

        private void Start_tests_bt_Click(object sender, RoutedEventArgs e)
        {
            if (tests.Count != 0)
            {
                Logs_tb.Text = "Tests started...\r\n";
                Logs_tb.Text += " " + " " + "Time" + " " + "Parameter" + " " + "=" + " " + "Value" + " " + "Unit" + "\r\n";
                Add_new_test_cb.SelectedIndex = -1;
                almighty_controller.shedule_tests(tests);
            }
        }

        private void Stop_tests_bt_Click(object sender, RoutedEventArgs e)
        {
            if (tests.Count != 0)
            {                                
                almighty_controller.stop_tests(this);
                Logs_tb.Text += "Tests stopped.\r\n";
                List_of_tests_tb.Text = "";
            }
        }

        private void Add_new_test_cb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (Add_new_test_cb.SelectedIndex)
            {
                case -1:
                    {
                        Add_new_test_cb.PlaceholderText = "        ADD NEW TEST";

                        break;
                    }
                case 0:
                    {
                        selected_index = 1;
                        Frame.Navigate(typeof(Setup_view_wifi), this);
                        
                        break;
                    }
                case 1:
                    {
                        selected_index = 2;
                        Frame.Navigate(typeof(Setup_view_wifi), this);

                        break;
                    }
                case 2:
                    {
                        selected_index = 3;
                        Frame.Navigate(typeof(Setup_view_bluetooth), this);

                        break;
                    }
                case 3:
                    {
                        selected_index = 4;
                        Frame.Navigate(typeof(Setup_view_bluetooth), this);

                        break;
                    }
                case 4:
                    {
                        selected_index = 5;
                        Frame.Navigate(typeof(Setup_view_gsm), this);

                        break;
                    }
            }
        }

        private void Clear_list_of_tests_bt_Click(object sender, RoutedEventArgs e)
        {
            List_of_tests_tb.Text = "";
            tests.Clear();
        }

        public void add_new_test(int i, String filepath, String wifi_or_device_name, String ip_address_or_uuid_or_url, int port)
        {
            switch (i)
            {
                case 1:
                    {
                        tests.Add(new Test(Testtype.Test_1, filepath, "", ip_address_or_uuid_or_url, port, this));
                        List_of_tests_tb.Text += "Test 1 - Wi-Fi/Send\n";
                        Add_new_test_cb.PlaceholderText = "        ADD NEW TEST";

                        break;
                    }
                case 2:
                    {
                        tests.Add(new Test(Testtype.Test_2, filepath, "", ip_address_or_uuid_or_url, port, this));
                        List_of_tests_tb.Text += "Test 2 - Wi-Fi/Receive\n";
                        Add_new_test_cb.PlaceholderText = "        ADD NEW TEST";

                        break;
                    }
                case 3:
                    {
                        tests.Add(new Test(Testtype.Test_3, filepath, wifi_or_device_name, ip_address_or_uuid_or_url, 0, this));
                        List_of_tests_tb.Text += "Test 3 - Bluetooth/Send\n";
                        Add_new_test_cb.PlaceholderText = "        ADD NEW TEST";

                        break;
                    }
                case 4:
                    {
                        tests.Add(new Test(Testtype.Test_4, filepath, wifi_or_device_name, ip_address_or_uuid_or_url, 0, this));
                        List_of_tests_tb.Text += "Test 4 - Bluetooth/Receive\n";
                        Add_new_test_cb.PlaceholderText = "        ADD NEW TEST";

                        break;
                    }

                case 5:
                    {
                        tests.Add(new Test(Testtype.Test_5, filepath, "", ip_address_or_uuid_or_url, 0, this));
                        List_of_tests_tb.Text += "Test 5 - Gsm/Receive\n";
                        Add_new_test_cb.PlaceholderText = "        ADD NEW TEST";

                        break;
                    }
            }
        }

        public async void text_to_logs(String text)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal,
                () =>
                {
                    Logs_tb.Text += text;
                }).AsTask();
        }        
    }
}
