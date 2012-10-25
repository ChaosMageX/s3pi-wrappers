using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace s3piwrappers.Converters
{
    internal class ImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
                              object parameter, CultureInfo culture)
        {
            if (String.IsNullOrEmpty(value as string))
            {
                return null;
            }
            try
            {
                return new BitmapImage(new Uri((string) value));
            }
            catch
            {
                return new BitmapImage();
            }
        }

        public object ConvertBack(object value, Type targetType,
                                  object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
