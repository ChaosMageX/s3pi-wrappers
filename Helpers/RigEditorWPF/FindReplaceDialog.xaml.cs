using System;
using System.Windows;
using System.Windows.Input;
using s3piwrappers.RigEditor.Commands;

namespace s3piwrappers.RigEditor
{
    public partial class FindReplaceDialog : Window
    {
        public String Find { get; set; }
        public String Replace { get; set; }
        public ICommand AcceptInputCommand { get; private set; }

        public FindReplaceDialog(String title) : this()
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
