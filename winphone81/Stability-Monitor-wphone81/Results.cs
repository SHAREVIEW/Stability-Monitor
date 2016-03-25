using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Windows.Storage;
using System.Threading;

namespace Stability_Monitor_wphone81
{
    class Results
    {
        private String _logfilepath;
        private bool _created = false;
        private StorageFolder _folder = KnownFolders.PicturesLibrary;
        private StorageFile _file;
        private readonly SemaphoreSlim _lock = new SemaphoreSlim(1);

        public Results(Testtype tt)
        {
            DateTime dt = DateTime.Now;
            this._logfilepath = "Logfile_" + tt.ToString() + "_" + dt.TimeOfDay.ToString("hh\\-mm\\-ss\\,ff") + ".txt";
        }

        public async void append_to_log(String message)
        {
            await _lock.WaitAsync();

            if (!_created)
            {
                _file = await _folder.CreateFileAsync(_logfilepath, CreationCollisionOption.GenerateUniqueName);
                _logfilepath = _file.Name;
                _file = await _folder.GetFileAsync(_logfilepath);
                await FileIO.AppendTextAsync(_file, message);

                _created = true;
            }
            else
            {
                _file = await _folder.GetFileAsync(_logfilepath);
                await FileIO.AppendTextAsync(_file, message);
            }

            _lock.Release();                        
        }
    }
}
