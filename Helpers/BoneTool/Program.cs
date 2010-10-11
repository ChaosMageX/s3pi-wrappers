﻿using System;
using System.Windows.Forms;
using s3pi.DemoPlugins;

namespace s3piwrappers.BoneTool
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static int Main(params string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            return RunHelper.Run(typeof(MainForm),args);
        }
    }
}
