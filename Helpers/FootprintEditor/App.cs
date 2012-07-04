using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using FootprintViewer.Models;
using s3pi.GenericRCOLResource;

namespace FootprintViewer
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

            if (viewModel.IsIsSaving)
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
