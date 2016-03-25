using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stability_Monitor_win32
{
    class Results
    {
        private String _logfilepath;
        private static object _locker = new Object();
        
        public Results(Testtype tt)
        {
            DateTime dt = DateTime.Now;
            this._logfilepath = "Logfile_" + tt.ToString() + "_" + dt.TimeOfDay.ToString("hh\\-mm\\-ss\\,ff") + ".txt";
        }

        public void append_to_log(String message)
        {
            lock (_locker)
            {
                File.AppendAllText(_logfilepath, message);
            }
        }
    }
}
