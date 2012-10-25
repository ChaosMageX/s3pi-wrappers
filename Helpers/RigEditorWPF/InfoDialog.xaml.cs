using System;
using System.Windows;

namespace s3piwrappers.RigEditor
{
    /// <summary>
    ///   Interaction logic for InfoDialog.xaml
    /// </summary>
    public partial class InfoDialog : Window
    {
        public string Info { get; set; }

        public InfoDialog(String info, String title) : this()
        {
            Title = title;
            Info = info;
        }

        public InfoDialog()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
