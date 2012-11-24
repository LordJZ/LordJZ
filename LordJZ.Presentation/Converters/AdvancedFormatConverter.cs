using System;
using System.Globalization;
using System.Windows.Data;

namespace LordJZ.Presentation.Converters
{
    [ValueConversion(typeof(string), typeof(string))]
    public sealed class AdvancedFormatConverter : IMultiValueConverter, IValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return ((string)parameter).AdvancedFormat(culture, values);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((string)parameter).AdvancedFormat(culture, value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
