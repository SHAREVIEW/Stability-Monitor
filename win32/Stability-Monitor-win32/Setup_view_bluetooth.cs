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
    public partial class Setup_view_bluetooth : Form
    {
        private int nmbr;
        private Main_view mv;

        public Setup_view_bluetooth(Main_view mv)
        {
            InitializeComponent();

            this.mv = mv;
            this.nmbr = mv.selected_index;
        }
                
        private void Add_test_bt_Click(object sender, EventArgs e)
        {
            if (Filepath_tb.Text.Equals("") || Name_of_device_tb.Text.Equals("") || Uuid_tb.Text.Equals("")) return;

            mv.add_new_test(nmbr, Filepath_tb.Text, Name_of_device_tb.Text, Uuid_tb.Text, 0);
            this.Close();
        }

        private void Cancel_bt_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
