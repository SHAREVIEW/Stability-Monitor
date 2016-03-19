using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stability_Monitor_win32
{
    interface Callback_on_status_changed
    {
        void on_filetransfer_started(String report, Results results);
        void on_file_received(String report, Results results);
        void on_file_transfer_error(String report, Results results);
        void on_signal_intensity_or_rssi_change(String report, Results results);
        void on_transfer_speed_change(String report, Results results);
        
    }

    class Callback_Instance : Callback_on_status_changed
    {
        public void on_filetransfer_started(String report, Results results)
        {

        }

        public void on_file_received(String report, Results results)
        {
            results.append_to_log(report);
        }

        public void on_file_transfer_error(String report, Results results)
        {

        }

        public void on_signal_intensity_or_rssi_change(String report, Results results)
        {
            results.append_to_log(report);
        }

        public void on_transfer_speed_change(String report, Results results)
        {
            results.append_to_log(report);           
        }
    } 
}
