using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace s3piwrappers.Converters
{
    public class PointColorConverter : IValueConverter
    {
        public Brush Selected { get; set; }
        public Brush Default { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Brush b = Default;
            if (value is bool && (bool) value)
            {
                b = Selected;
            }
            return b;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
