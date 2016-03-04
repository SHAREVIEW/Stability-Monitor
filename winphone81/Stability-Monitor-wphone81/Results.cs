using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Windows.Storage;

namespace Stability_Monitor_wphone81
{
    class Results
    {
        private String _logfile = "logfile.txt";
        private bool _created = false;
        private StorageFolder _folder = KnownFolders.PicturesLibrary;
        private StorageFile _file;

        public async void append_to_log(String message)
        {
            if (!_created)
            {
                _file = await _folder.CreateFileAsync(_logfile, CreationCollisionOption.ReplaceExisting);

                _file = await _folder.GetFileAsync(_logfile);

                await FileIO.AppendTextAsync(_file, message);

                _created = true;
            }
            else
            {
                _file = await _folder.GetFileAsync(_logfile);

                await FileIO.AppendTextAsync(_file, message);
            }
        }
    }
}
