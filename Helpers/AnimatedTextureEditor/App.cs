using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Linq;
using System.Text;
using s3pi.GenericRCOLResource;
using s3piwrappers.ViewModels;

namespace s3piwrappers
{
    class App
    {
        [STAThread()]
        static void Main(params String[] args)
        {
            byte[] buffer = null;
            using (Stream s = File.OpenRead(args[0]))
            {
                buffer = new byte[s.Length];
                s.Read(buffer, 0, buffer.Length);
            }
            var app = new Application();
            var rig = new GenericRCOLResource(0, new MemoryStream(buffer));
            var viewModel = new AnimViewModel(rig);

            var win = new MainWindow(viewModel);
            app.Run(win);

            if (viewModel.IsSaving)
            {
                byte[] output =rig.AsBytes;
                using (var s = File.Create(args[0]))
                {
                    s.Write(output, 0, output.Length);
                }
            }
        }



       
    }
}
