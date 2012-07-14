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
            Area_Scroller.ScrollToHorizontalOffset(4800);
            Area_Scroller.ScrollToVerticalOffset(4800);
        }
        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            DrawGridLines();
            Register();
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
        }

        private void AreaCanvas_MouseLeave(object sender, MouseEventArgs e)
        {

            draggedPoint = null;

        }
    }
}
