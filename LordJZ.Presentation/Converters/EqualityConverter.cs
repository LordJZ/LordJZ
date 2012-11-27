using System;
using System.Globalization;
using System.Windows.Data;

namespace LordJZ.Presentation.Converters
{
    /// <summary>
    /// Converts the specified values to true, if all of them are equal; otherwise, false.
    /// </summary>
    public sealed class EqualityConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            object value = null;

            int count = values.Length;
            for (int i = 0; i < count; i++)
            {
                if (i == 0)
                    value = values[i];
                else if (!value.Equals(values[i]))
                    return BooleanBoxes.False;
            }

            return BooleanBoxes.True;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
