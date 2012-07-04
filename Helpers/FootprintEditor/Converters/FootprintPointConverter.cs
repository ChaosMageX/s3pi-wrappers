using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using FootprintViewer.Models;

namespace FootprintViewer.Converters
{
    public class FootprintPointConverter : IValueConverter
    {
        public double Factor { get; set; }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var ft = value as IEnumerable<PointViewModel>;
            var points = new PointCollection();
            if (ft != null)
            {
                foreach (var ftPoint in ft)
                {
                    points.Add(new Point(ftPoint.X * Factor, ftPoint.Z * Factor));
                }

            }
            return points;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}