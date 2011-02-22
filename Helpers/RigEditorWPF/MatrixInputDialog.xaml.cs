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
    /// Interaction logic for MatrixInputDialog.xaml
    /// </summary>
    public partial class MatrixInputDialog : Window
    {
        public MatrixInputDialog(Geometry.Matrix context, String title):this()
        {
            Title = title;
            DataContext = context;
        }
        public MatrixInputDialog()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            Close();
        }
        public Geometry.Matrix Value { get { return (Geometry.Matrix)DataContext; } }
    }
}
