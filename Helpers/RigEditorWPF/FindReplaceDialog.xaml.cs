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
    public partial class FindReplaceDialog : Window
    {
        public String Find { get; set; }
        public String Replace { get; set; }

        public FindReplaceDialog(String title):this()
        {
            Title = title;
        }
        public FindReplaceDialog()
        {
            InitializeComponent();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(Find))
            {
                DialogResult = true;
                this.Close();
            }
        }

    }
}
