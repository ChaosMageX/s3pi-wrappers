using System.Windows.Data;
using System;
namespace s3piwrappers.Converters
{
    /// <summary>
    /// Firetruck!
    /// </summary>
    class DebugConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }
}
