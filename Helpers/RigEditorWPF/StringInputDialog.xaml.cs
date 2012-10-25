using System;
using System.Windows;
using System.Windows.Input;
using s3piwrappers.RigEditor.Commands;

namespace s3piwrappers.RigEditor
{
    /// <summary>
    ///   Interaction logic for StringInputDialog.xaml
    /// </summary>
    public partial class StringInputDialog : Window
    {
        public String Value { get; set; }
        public ICommand AcceptInputCommand { get; private set; }

        public StringInputDialog(String title) : this()
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
