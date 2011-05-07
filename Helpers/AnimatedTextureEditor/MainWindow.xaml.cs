using System.Windows;
using System.Windows.Controls;
using s3piwrappers.ViewModels;

namespace s3piwrappers
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private readonly AnimViewModel mViewModel;
        internal MainWindow(AnimViewModel viewModel):this()
        {
            mViewModel = viewModel;
            this.DataContext = mViewModel;
        }

    }
}
