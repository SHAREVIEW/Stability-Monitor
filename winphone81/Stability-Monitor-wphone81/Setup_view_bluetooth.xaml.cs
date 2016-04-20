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
    public sealed partial class Setup_view_bluetooth : Page
    {
        private int nmbr;
        private Main_view mv;

        public Setup_view_bluetooth()
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
            if (Filename_tb.Text.Equals("") || Name_of_device_tb.Text.Equals("")) return;

            mv.add_new_test(nmbr, Filename_tb.Text, Name_of_device_tb.Text, Uuid_cb.SelectedValue.ToString(), 0);
            Frame.GoBack();
        }
    }
}
