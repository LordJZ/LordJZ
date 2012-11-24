using System;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Windows.Data;

namespace LordJZ.Presentation.Converters
{
    [ValueConversion(typeof(string), typeof(string))]
    public sealed class ResourceStringConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var resourceMgr = (ResourceManager)parameter;
            var format = (string)values[0];

            return resourceMgr.GetString(string.Format(format, values.Skip(1).ToArray()));
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
