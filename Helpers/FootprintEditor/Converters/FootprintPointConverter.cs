using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using s3piwrappers.Models;
using System.Linq;
namespace s3piwrappers.Converters
{
    public class FootprintPointConverter : IValueConverter
    {
        public double Scale { get; set; }
        public double Offset { get; set; }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var ft = value as IEnumerable<PointViewModel>;
            var points = new PointCollection();
            if (ft != null)
            {
                foreach (var ftPoint in ft)
                {
                    points.Add(new Point( (ftPoint.X * Scale) + Offset, (ftPoint.Z * Scale) + Offset));
                }

            }
            return points;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
    public class FootprintPointBoundsConverter : IValueConverter
    {
        public double Scale { get; set; }
        public double Offset { get; set; }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var ft = value as IEnumerable<PointViewModel>;
            var points = new PointCollection();
            if (ft != null && ft.Any() && ft.Count() >2)
            {
                double minX=double.MaxValue, minZ=double.MaxValue, maxX=double.MinValue, maxZ=double.MinValue;
                foreach (var point in ft)
                {
                    if(point.X < minX) minX = point.X;
                    if (point.Z < minZ) minZ = point.Z;
                    if (point.X > maxX) maxX = point.X;
                    if (point.Z > maxZ) maxZ= point.Z;
                }
                points.Add(new Point((minX * Scale) + Offset, (minZ * Scale) + Offset));
                points.Add(new Point((maxX * Scale) + Offset, (minZ * Scale) + Offset));
                points.Add(new Point((maxX * Scale) + Offset, (maxZ * Scale) + Offset));
                points.Add(new Point((minX * Scale) + Offset, (maxZ * Scale) + Offset));

            }
            return points;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}