using System;
using System.Windows;
using s3piwrappers.RigEditor.Geometry;

namespace s3piwrappers.RigEditor
{
    /// <summary>
    ///   Interaction logic for MatrixInputDialog.xaml
    /// </summary>
    public partial class MatrixInputDialog : Window
    {
        public MatrixInputDialog(Matrix context, String title) : this()
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
            DialogResult = true;
            Close();
        }

        public Matrix Value
        {
            get { return (Matrix) DataContext; }
        }
    }
}
