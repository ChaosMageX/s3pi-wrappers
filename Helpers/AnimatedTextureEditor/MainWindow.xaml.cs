using System.Windows;
using s3piwrappers.AnimatedTextureEditor.ViewModels;

namespace s3piwrappers.AnimatedTextureEditor
{
    /// <summary>
    ///   Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private readonly AnimViewModel mViewModel;

        internal MainWindow(AnimViewModel viewModel) : this()
        {
            mViewModel = viewModel;
            DataContext = mViewModel;
        }
    }
}
