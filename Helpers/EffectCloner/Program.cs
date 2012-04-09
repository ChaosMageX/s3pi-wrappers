using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace s3piwrappers.EffectCloner
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
#if DEBUG
            Application.Run(new MainForm(args));
#else
            try
            {
                Application.Run(new MainForm(args));
            }
            catch (Exception ex)
            {
                MainForm.ShowException(ex);
            }
#endif
        }
    }
}
