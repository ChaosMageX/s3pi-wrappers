using System;
using System.IO;
using System.Windows;
using s3pi.GenericRCOLResource;
using s3piwrappers.Models;

namespace s3piwrappers
{
    class App
    {
        [STAThread]
        static void Main(string[] args)
        {


            byte[] buffer = null;
            using (Stream s = File.OpenRead(args[0]))
            {
                buffer = new byte[s.Length];
                s.Read(buffer, 0, buffer.Length);
            }
            var app = new Application();
            var rcol = new GenericRCOLResource(0, new MemoryStream(buffer));
            var viewModel = new FootprintEditorViewModel(rcol);
            var win = new MainWindow(viewModel);
            app.Run(win);

            if (viewModel.IsSaving)
            {
                byte[] output = rcol.AsBytes;
                using (var s = File.Create(args[0]))
                {
                    s.Write(output, 0, output.Length);
                }
            }
            
        }
    }
}
