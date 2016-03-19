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
        private String _logfilepath = "C:\\Stability-Monitor-win32\\logfile";
        private bool _created = false;
        private int _logfile_number = 1;
        private static object _locker = new Object();
        
        public void append_to_log(String message)
        {
            lock (_locker) {

                if (!_created)
                {
                    while (File.Exists(_logfilepath + _logfile_number + ".txt"))
                    {
                        _logfile_number++;
                    }

                    _logfilepath = _logfilepath + _logfile_number + ".txt";
                    File.AppendAllText(_logfilepath, message);
                    _created = true;
                }
                else
                {
                    File.AppendAllText(_logfilepath, message);
                }   
            }
        }
    }
}
