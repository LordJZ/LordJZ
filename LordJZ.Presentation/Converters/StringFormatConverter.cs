using System;
using System.Globalization;
using System.Windows.Data;

namespace LordJZ.Presentation.Converters
{
    [ValueConversion(typeof(string), typeof(string))]
    public sealed class StringFormatConverter : IMultiValueConverter, IValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return string.Format(culture, (string)parameter, values);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return string.Format(culture, (string)parameter, value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
