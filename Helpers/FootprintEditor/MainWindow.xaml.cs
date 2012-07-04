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
using System.Windows.Navigation;
using System.Windows.Shapes;
using FootprintViewer.Models;
using System.Diagnostics;

namespace FootprintViewer
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

        private FootprintEditorViewModel viewModel;
        public MainWindow(FootprintEditorViewModel vm)
            : this()
        {
            this.viewModel = vm;
            this.DataContext = this.viewModel;
        }





        private Ellipse draggedPoint;
        
        private void AreaCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            draggedPoint = e.OriginalSource as Ellipse;
            if (draggedPoint != null)
            {
                var dc = draggedPoint.DataContext as PointViewModel;
                viewModel.SelectedArea.SelectedPoint = dc;
                Debug.WriteLine("Set");

            }
        }


        private void AreaCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (draggedPoint != null)
            {
                var pos = e.GetPosition((IInputElement)draggedPoint.Parent);
                double x = pos.X;
                double y = pos.Y;
                x -= draggedPoint.Width / 2;
                y -= draggedPoint.Height / 2;
                Canvas.SetLeft(draggedPoint, x);
                Canvas.SetTop(draggedPoint, y);
            }
        }

        private void AreaCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            draggedPoint = null;
        }

        private void AreaCanvas_MouseLeave(object sender, MouseEventArgs e)
        {
            draggedPoint = null;

        }



    }
}
