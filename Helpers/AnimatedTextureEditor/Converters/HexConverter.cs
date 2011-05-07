using System;
using System.Windows.Data;

namespace s3piwrappers.Converters
{
    public class HexConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return "0x"+ ((UInt32)value).ToString("X8"); 
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var str = value.ToString().Replace("0x","");
            if (string.IsNullOrEmpty(str)) str = "0";

            return System.Convert.ToUInt32(str, 16);
        }
    }
}
