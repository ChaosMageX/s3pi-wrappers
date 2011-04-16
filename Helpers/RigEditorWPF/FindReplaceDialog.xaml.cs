using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using s3piwrappers.RigEditor.Commands;

namespace s3piwrappers.RigEditor
{
    public partial class FindReplaceDialog : Window
    {
        public String Find { get; set; }
        public String Replace { get; set; }
        public ICommand AcceptInputCommand { get; private set; }
        public FindReplaceDialog(String title):this()
        {
            Title = title;
        }
        public FindReplaceDialog()
        {
            AcceptInputCommand = new UserCommand<FindReplaceDialog>(CanExecuteAcceptInput, ExecuteAcceptInput);
            InitializeComponent();
        }

        private static bool CanExecuteAcceptInput(FindReplaceDialog x)
        {
            return x != null && !String.IsNullOrEmpty(x.Find);
        }

        private static void ExecuteAcceptInput(FindReplaceDialog dialog)
        {
            dialog.DialogResult = true;
            dialog.Close();
        }

    }
}
