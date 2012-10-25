using System;
using System.IO;
using System.Windows;
using s3piwrappers.RigEditor.ViewModels;

namespace s3piwrappers.RigEditor
{
    internal class App
    {
        [STAThread]
        private static void Main(string[] args)
        {
            byte[] buffer = null;
            using (Stream s = File.OpenRead(args[0]))
            {
                buffer = new byte[s.Length];
                s.Read(buffer, 0, buffer.Length);
            }
            var app = new Application();
            var rig = new RigResource.RigResource(0, new MemoryStream(buffer));
            var viewModel = new RigEditorViewModel(rig);
            var win = new MainWindow(viewModel);
            app.Run(win);

            if (viewModel.IsSaving)
            {
                byte[] output = rig.AsBytes;
                using (FileStream s = File.Create(args[0]))
                {
                    s.Write(output, 0, output.Length);
                }
            }
        }
    }
}
