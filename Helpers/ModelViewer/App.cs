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
            Application app = new Application();
            MainWindow win = new MainWindow(s);
            app.Run(win);


        }
    }
}