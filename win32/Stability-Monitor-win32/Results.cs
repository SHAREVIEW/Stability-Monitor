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
        private bool _created = false;
        private FileStream _filestream;

        public void append_to_log(String message)
        {
            if (!_created)
            {
                _created = true;

                _filestream = File.Create(_logfilepath);
                _filestream.Close();

                File.AppendAllText(_logfilepath, message);
            }
            else
            {                
                File.AppendAllText(_logfilepath, message);
            }

            
        }
    }
}
