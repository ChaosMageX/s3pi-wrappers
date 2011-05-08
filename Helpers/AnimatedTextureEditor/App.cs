using System;
using System.IO;
using System.Windows;
using s3pi.GenericRCOLResource;
using s3piwrappers.AnimatedTextureEditor.ViewModels;

namespace s3piwrappers.AnimatedTextureEditor
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
            var resource = new GenericRCOLResource(0, new MemoryStream(buffer));
            var viewModel = new AnimViewModel(resource);
            var win = new MainWindow(viewModel);
            app.Run(win);
            if (viewModel.IsSaving)
            {
                byte[] output =resource.AsBytes;
                using (var s = File.Create(args[0]))
                {
                    s.Write(output, 0, output.Length);
                }
            }
        }



       
    }
}
