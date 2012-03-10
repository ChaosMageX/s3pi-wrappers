using System;
using System.IO;
using System.Windows;
using System.Collections.Generic;
using System.Security.Cryptography;

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
            for (int i = 1; i < args.Length; i++)
            {
                switch (args[i])
                {
                    case "-texture":
                        win.TextureSource = args[++i];
                        break;
                    case "-title":
                        win.Title += " - " + args[++i];
                        break;
                    default:
                        break;
                }

            }
            app.Run(win);


        }
    }
}