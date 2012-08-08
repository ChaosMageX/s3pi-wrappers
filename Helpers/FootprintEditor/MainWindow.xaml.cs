using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;
using System.Diagnostics;
using s3piwrappers.Models;
using System.Windows.Media;
using System.Windows.Threading;
using System.Threading;
using System.Linq;

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

        private FootprintEditorViewModel viewModel;
        public MainWindow(FootprintEditorViewModel vm)
            : this()
        {
            this.viewModel = vm;
            this.DataContext = this.viewModel;
            
        }
        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            if(viewModel.SelectedArea == null)
                SetGrid(0 ,0);
            DrawGridLines();
            Register();
        }
        void SetGrid(double x,double y)
        {
            Area_Scroller.ScrollToHorizontalOffset(x+(  5000 - Area_Scroller.ActualWidth / 2));
            Area_Scroller.ScrollToVerticalOffset(y+(  5000 - Area_Scroller.ActualHeight / 2));
            
        }

        private static void Register()
        {
            EventManager.RegisterClassHandler(typeof(TextBox), PreviewMouseLeftButtonDownEvent,
                                              new MouseButtonEventHandler(SelectivelyHandleMouseButton), true);
            EventManager.RegisterClassHandler(typeof(TextBox), GotKeyboardFocusEvent,
                                              new RoutedEventHandler(SelectAllText), true);
        }

        private static void SelectivelyHandleMouseButton(object sender, MouseButtonEventArgs e)
        {
            var textbox = (sender as TextBox);
            if (textbox != null && !textbox.IsKeyboardFocusWithin)
            {
                if (e.OriginalSource.GetType().Name == "TextBoxView")
                {
                    e.Handled = true;
                    textbox.Focus();
                }
            }
        }

        private static void SelectAllText(object sender, RoutedEventArgs e)
        {
            var textBox = e.OriginalSource as TextBox;
            if (textBox != null)
                textBox.SelectAll();
        }

        private void DrawGridLines()
        {
            var zmax = AreaCanvas.ActualHeight;
            var xmax = AreaCanvas.ActualWidth;
            var interval = 100;
            Line line;
            Brush stroke = Brushes.LightGreen;
            double strokeThickness = 1;
            Canvas canvas = PointGrid;

            for (double x = 0; x <= zmax; x += interval)
            {
                line = new Line
                    {
                        X1 = 0,
                        X2 = xmax,
                        Y1 = x,
                        Y2 = x,
                        Stroke = stroke,
                        StrokeThickness = strokeThickness

                    };
                if (x == zmax / 2)
                {
                    line.Stroke = Brushes.DarkGreen;
                    line.StrokeThickness = strokeThickness * 2;
                }
                canvas.Children.Add(line);

            }
            for (double z = 0; z <= xmax; z += interval)
            {
                line = new Line
                {
                    X1 = z,
                    X2 = z,
                    Y1 = 0,
                    Y2 = zmax,
                    Stroke = stroke,
                    StrokeThickness = strokeThickness

                };
                if (z == xmax / 2)
                {
                    line.Stroke = Brushes.DarkGreen;
                    line.StrokeThickness = strokeThickness * 2;
                }
                canvas.Children.Add(line);

            }

        }
        private Shape draggedPoint;

        private void AreaCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            if (e.OriginalSource is Ellipse)
            {
                draggedPoint = e.OriginalSource as Shape;
                var dc = draggedPoint.DataContext as PointViewModel;
                if (dc != null)
                {
                    viewModel.SelectedArea.SelectedPoint = dc;
                }

            }
        }


        private void AreaCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            var crs = e.GetPosition((IInputElement)AreaCanvas);


            viewModel.CursorX = (crs.X - AreaCanvas.ActualWidth/2)  / 100;
            viewModel.CursorZ = (crs.Y - AreaCanvas.ActualHeight/2) / 100;
            if (draggedPoint != null)
            {
                var pos = e.GetPosition((IInputElement)draggedPoint.Parent);
                double x = pos.X;
                double y = pos.Y;
                if (x < 0) x = 0;
                if (y < 0) y = 0;
                x -= draggedPoint.Width / 2;
                y -= draggedPoint.Height / 2;
                Canvas.SetLeft(draggedPoint, x);
                Canvas.SetTop(draggedPoint, y);
            }
        }

        private void AreaCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (draggedPoint != null)
            {
                draggedPoint = null;
            }
            viewModel.CursorX = null;
            viewModel.CursorZ = null;
        }

        private void AreaCanvas_MouseLeave(object sender, MouseEventArgs e)
        {

            draggedPoint = null;
            viewModel.CursorX = null;
            viewModel.CursorZ = null;

        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(e.AddedItems.Count > 0)
            {
                var item = e.AddedItems[0] as AreaViewModel;
                if(item!=null)
                {
                    SetGrid(item.OffsetX * 100,item.OffsetZ * 100);
                }
            }

        }

        private void PointBox_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var pnt = PointBox.SelectedItem as PointViewModel;
            if (pnt != null)
            {
                SetGrid(pnt.X * 100, pnt.Z * 100);
            }

        }

        private void AreaCanvas_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if(e.Delta > 0)
            {
                viewModel.Zoom += 5;
            }else
            {
                viewModel.Zoom -= 5;
            }
            e.Handled = true;
        }
    }
}
