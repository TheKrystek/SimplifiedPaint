using System;
using System.Collections.Generic;
using System.Windows.Data;

namespace SimplifiedPaint.Converters
{
    public class TextNumberConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
                      System.Globalization.CultureInfo culture)
        {
            var str = value as string;
            if (str == null) return 0;

            int result = 0;
            int.TryParse(str, out result);

            return result;

        }
        public object ConvertBack(object value, Type targetType, object parameter,
                      System.Globalization.CultureInfo culture)
        {
            return value.ToString();
        }
    }
}