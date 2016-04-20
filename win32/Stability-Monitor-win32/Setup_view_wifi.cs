using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Stability_Monitor_win32
{
    public partial class Setup_view_wifi : Form
    {
        private int nmbr;
        private Main_view mv;

        public Setup_view_wifi(Main_view mv)
        {
            InitializeComponent();

            this.mv = mv;
            this.nmbr = mv.selected_index;
        }

        private void Add_test_bt_Click(object sender, EventArgs e)
        {
            IPAddress ip;
            int i;

            if (Filepath_tb.Text.Equals("") || Name_of_wifi_tb.Text.Equals("") || Ip_address_tb.Text.Equals("") || Port_tb.Text.Equals("")
                || !(IPAddress.TryParse(Ip_address_tb.Text, out ip)) || !(int.TryParse(Port_tb.Text, out i))) return;

            mv.add_new_test(nmbr, Filepath_tb.Text, Name_of_wifi_tb.Text, Ip_address_tb.Text, int.Parse(Port_tb.Text));
            this.Close();
        }

        private void Cancel_bt_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
