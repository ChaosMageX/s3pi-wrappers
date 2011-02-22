using System.Windows;

namespace s3piwrappers.RigEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(RigViewModel viewModel):this()
        {
            DataContext = viewModel;
        }
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BoneTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            mBonePanel.Visibility = mBoneTreeView.SelectedItem != null? System.Windows.Visibility.Visible:System.Windows.Visibility.Hidden;
        }
    }
}
