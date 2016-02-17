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
        private String _logfilepath = "C:\\Stability-Monitor-win32\\logfile.txt";

        public void append_to_log(String message)
        {
            File.AppendAllText(_logfilepath, message);
        }
    }
}
