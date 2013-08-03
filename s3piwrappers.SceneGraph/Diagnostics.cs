using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using s3pi.Interfaces;
using System.Windows.Forms;

namespace s3piwrappers.SceneGraph
{
    static class Diagnostics
    {
        private static bool popups = false;
        public static bool Popups { get { return popups; } set { popups = value; } }
        public static void Show(string value, string title = "")
        {
            string msg = value.Trim('\n');
            if (msg == "") return;
            if (title == "")
            {
                Debug.WriteLine(msg);
                if (popups) CopyableMessageBox.Show(msg);
            }
            else
            {
                Debug.WriteLine(String.Format("{0}: {1}", title, msg));
                if (popups) CopyableMessageBox.Show(msg, title);
            }
            if (logging) Log(value);
        }

#if DEBUG
        private static bool logging = true;
#else
        private static bool logging = false;
#endif
        private static StreamWriter logFile = null;
        static void OpenLogFile()
        {
            string filename = Path.Combine(Path.GetTempPath(), 
                string.Concat("s3piwrappers.SceneGraph-", 
                DateTime.UtcNow.ToString("s").Replace(":", ""), ".log"));
            logFile = new StreamWriter(new FileStream(filename, FileMode.Create));
        }
        public static bool Logging
        {
            get { return logging; }
            set
            {
                logging = value;
                if (!logging) { if (logFile != null) { logFile.Close(); logFile = null; } }
            }
        }
        public static void Log(string value)
        {
            string msg = value.Trim('\n');
            if (msg == "") return;
            if (!popups) Debug.WriteLine(msg);
            if (logging)
            { 
                if (logFile == null) OpenLogFile(); 
                logFile.WriteLine(string.Concat(DateTime.UtcNow.ToString("s"), ": ", msg)); 
                logFile.Flush(); 
            }
        }
    }
}
