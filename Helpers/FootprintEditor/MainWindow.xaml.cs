using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using s3piwrappers.Models;

namespace s3piwrappers
{
    /// <summary>
    ///   Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public const int kGridSize = 200;

        public MainWindow()
        {
            InitializeComponent();
        }

        private readonly FootprintEditorViewModel viewModel;

        public MainWindow(FootprintEditorViewModel vm)
            : this()
        {
            viewModel = vm;
            DataContext = viewModel;
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            if (viewModel.SelectedArea == null)
                SetGrid(0, 0);
            DrawGridLines();
            Register();
        }

        private void SetGrid(double x, double y)
        {
            Area_Scroller.ScrollToHorizontalOffset(x + (5000 - Area_Scroller.ActualWidth/2));
            Area_Scroller.ScrollToVerticalOffset(y + (5000 - Area_Scroller.ActualHeight/2));
        }

        private static void Register()
        {
            EventManager.RegisterClassHandler(typeof (TextBox), PreviewMouseLeftButtonDownEvent,
                                              new MouseButtonEventHandler(SelectivelyHandleMouseButton), true);
            EventManager.RegisterClassHandler(typeof (TextBox), GotKeyboardFocusEvent,
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
            double zmax = AreaCanvas.ActualHeight;
            double xmax = AreaCanvas.ActualWidth;
            int interval = kGridSize;
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
                if (x == zmax/2)
                {
                    line.Stroke = Brushes.DarkGreen;
                    line.StrokeThickness = strokeThickness*2;
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
                if (z == xmax/2)
                {
                    line.Stroke = Brushes.DarkGreen;
                    line.StrokeThickness = strokeThickness*2;
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
            Point crs = e.GetPosition(AreaCanvas);


            viewModel.CursorX = (crs.X - AreaCanvas.ActualWidth/2)/kGridSize;
            viewModel.CursorZ = (crs.Y - AreaCanvas.ActualHeight/2)/kGridSize;
            if (draggedPoint != null)
            {
                Point pos = e.GetPosition((IInputElement) draggedPoint.Parent);
                double x = pos.X;
                double y = pos.Y;
                if (x < 0) x = 0;
                if (y < 0) y = 0;
                x -= draggedPoint.Width/2;
                y -= draggedPoint.Height/2;
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
            if (e.AddedItems.Count > 0)
            {
                var item = e.AddedItems[0] as AreaViewModel;
                if (item != null)
                {
                    SetGrid(item.OffsetX*kGridSize, item.OffsetZ*kGridSize);
                }
            }
        }

        private void PointBox_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var pnt = PointBox.SelectedItem as PointViewModel;
            if (pnt != null)
            {
                SetGrid(pnt.X*kGridSize, pnt.Z*kGridSize);
            }
        }
    }
}
