using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using s3pi.DemoPlugins;

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

            return RunHelper.Run(typeof(BonePicker), args);
        }
    }
}
