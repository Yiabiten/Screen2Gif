using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Forms;

namespace ScreenToGif.Util.Converters
{
    class KeysToString : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var keys = value as Keys?;

            return keys?.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
