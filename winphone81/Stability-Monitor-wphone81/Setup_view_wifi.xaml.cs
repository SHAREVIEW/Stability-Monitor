using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Stability_Monitor_wphone81
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Setup_view_wifi : Page
    {
        private int nmbr;
        private Main_view mv;

        public Setup_view_wifi()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.mv = (Main_view)e.Parameter;
            this.nmbr = mv.selected_index;
        }

        private void Cancel_bt_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }

        private void Add_test_bt_Click(object sender, RoutedEventArgs e)
        {            
            int i;

            if (Filename_tb.Text.Equals("") || Ip_address_tb.Text.Equals("") || Port_tb.Text.Equals("")
                || !(int.TryParse(Port_tb.Text, out i))) return;

            mv.add_new_test(nmbr, Filename_tb.Text, "", Ip_address_tb.Text, int.Parse(Port_tb.Text));
            Frame.GoBack();
        }
    }
}
