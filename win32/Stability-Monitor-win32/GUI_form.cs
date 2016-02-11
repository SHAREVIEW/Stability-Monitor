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
    public partial class GUI_form : Form
    {
        public GUI_form()
        {
            InitializeComponent();
        }

        private Almighty_controller ac = new Almighty_controller();

        private void Test_1_runner_Click(object sender, EventArgs e)
        {
            ac.shedule_test();

        }
    }
}
