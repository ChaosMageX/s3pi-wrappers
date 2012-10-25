using System;
using System.Globalization;
using System.Windows.Data;

namespace s3piwrappers.Converters
{
    public class ScaleConverter : IValueConverter
    {
        public double Scale { get; set; }

        public double Offset { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (((float) value)*Scale) + Offset;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double inc = (double) value - Offset;
            inc = inc/Scale;
            return inc;
        }
    }
}
