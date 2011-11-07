﻿using System;
using System.Windows.Data;

namespace s3piwrappers.RigEditor.Converters
{
    public class HexConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {

            return String.Format("0x{0:X8}",value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var str = value.ToString().Replace("0x","");
            if (string.IsNullOrEmpty(str)) str = "0";
            return System.Convert.ToInt32(str, 16);
        }
    }
}
