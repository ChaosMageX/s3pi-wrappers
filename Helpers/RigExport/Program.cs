using System;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace RigExport
{
    internal static class Program
    {
        /// <summary>
        ///   The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main(params string[] args)
        {
            IWin32Window window = null;
            String filename = String.Empty;
            if (args.Length == 0 || !File.Exists(args[0]))
            {
                var filePicker = new OpenFileDialog {Filter = "Rig files (*.grannyrig)|*.grannyrig|All files (*.*)|*.*", Multiselect = false, CheckFileExists = true, Title = "Select a rig file..."};
                DialogResult result = filePicker.ShowDialog();
                if (result == DialogResult.OK)
                {
                    filename = filePicker.FileName;
                }
            }
            else
            {
                filename = args[0];
            }
            if (!String.IsNullOrEmpty(filename))
            {
                using (FileStream stream = File.OpenRead(filename))
                {
                    Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
                    var dialog = new BonePicker(stream);
                    Application.Run(dialog);
                }
            }
        }
    }
}
