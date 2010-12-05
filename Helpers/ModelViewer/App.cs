using System;
using System.IO;
using System.Windows;

namespace s3piwrappers.ModelViewer
{
    public class App
    {
        [STAThread()]
        static void Main(params String[] args)
        {
            Stream s = File.OpenRead(args[0]);
            // Create the application.
            Application app = new Application();
            // Create the main window.
            MainWindow win = new MainWindow(s);
            // Launch the application and show the main window.
            app.Run(win);
        }
    }
}