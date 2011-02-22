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

namespace s3piwrappers.RigEditor
{
    /// <summary>
    /// Interaction logic for StringInputDialog.xaml
    /// </summary>
    public partial class StringInputDialog : Window
    {
        public String Value { get; set; }

        public StringInputDialog(String title):this()
        {
            Title = title;
        }
        public StringInputDialog()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(Value))
            {
                DialogResult = true;
                this.Close();
            }
        }

    }
}
