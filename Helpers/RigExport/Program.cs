using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using s3pi.Helpers;

namespace RigExport
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static int Main(params string[] args)
        {

            System.Threading.Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            return RunHelper.Run(typeof(BonePicker), args);
        }
    }
}
