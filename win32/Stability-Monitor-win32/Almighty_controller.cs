using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Stability_Monitor_win32
{
    public class Almighty_controller
    {
        public void shedule_tests(List<Test> tests)
        {
            foreach (Test t in tests)
            {
                t.start_test();
            }
        }

        public void stop_tests(Main_view mv)
        {
            foreach (Test t in mv.tests)
            {
                t.stop_test();
            }

            mv.tests.Clear();
        }
    }
}
