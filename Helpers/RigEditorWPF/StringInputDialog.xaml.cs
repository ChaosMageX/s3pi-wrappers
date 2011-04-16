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
    /// <summary>
    /// Interaction logic for StringInputDialog.xaml
    /// </summary>
    public partial class StringInputDialog : Window
    {
        public String Value { get; set; }
        public ICommand AcceptInputCommand { get; private set; }

        public StringInputDialog(String title):this()
        {
            Title = title;
        }
        public StringInputDialog()
        {
            AcceptInputCommand = new UserCommand<StringInputDialog>(CanExecuteAcceptInput, ExecuteAcceptInput);
            InitializeComponent();
        }

        private static bool CanExecuteAcceptInput(StringInputDialog x)
        {
            return x != null && !String.IsNullOrEmpty(x.Value);
        }

        private static void ExecuteAcceptInput(StringInputDialog dialog)
        {
            dialog.DialogResult = true;
            dialog.Close();
        }

    }
}
