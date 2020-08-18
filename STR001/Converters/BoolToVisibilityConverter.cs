using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace STR001.WPF.Converters
{
    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter is string invertParam)
            {
                if (invertParam is "invert")
                {
                    return !(bool)value ? Visibility.Visible : Visibility.Collapsed;
                }
                else
                {
                    return (bool)value ? Visibility.Visible : Visibility.Collapsed;
                }
            }
            else
            {
                return (bool)value ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
