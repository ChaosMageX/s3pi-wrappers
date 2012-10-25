using System;
using System.Globalization;
using System.Windows.Data;

namespace s3piwrappers.Converters
{
    /// <summary>
    ///   Firetruck!
    /// </summary>
    internal class DebugConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
