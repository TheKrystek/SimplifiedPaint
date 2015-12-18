using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Data;

namespace SimplifiedPaint.Converters
{
    public class VisiblityToBool : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
                      System.Globalization.CultureInfo culture)
        {
            var val = (Visibility)value;
            if (val == Visibility.Visible)
                return true;
            return false;
        }
        public object ConvertBack(object value, Type targetType, object parameter,
                      System.Globalization.CultureInfo culture)
        {
            var val = (bool) value;
            return (val ? Visibility.Visible : Visibility.Hidden);
        }
    }
}