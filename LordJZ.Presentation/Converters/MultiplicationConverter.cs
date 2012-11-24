using System;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace LordJZ.Presentation.Converters
{
    [ValueConversion(typeof(double), typeof(double))]
    [ValueConversion(typeof(GridLength), typeof(GridLength))]
    public sealed class MultiplicationConverter : IValueConverter
    {
        enum ConvertibleKind
        {
            OneWay = 0,

            GridUnitAuto = 1,
            GridUnitPixel = 2,
            GridUnitStar = 3,
        }

        #region ConvertibleKind / GridUnitType conversion

        const int ConvertibleKind_GridUnitFirst = (int)ConvertibleKind.GridUnitAuto;
        const int ConvertibleKind_GridUnitLast = (int)ConvertibleKind.GridUnitStar;

        static readonly int GridUnitType_First;
        static readonly int GridUnitType_Last;

        static MultiplicationConverter()
        {
            var gridUnits = Enum.GetValues(typeof(GridUnitType)).Cast<GridUnitType>().Cast<int>().ToArray();
            GridUnitType_First = gridUnits.Min();
            GridUnitType_Last = gridUnits.Max();

            Contract.Assert(gridUnits.OrderBy(i => i).SequenceEqual(
                Enumerable.Range(GridUnitType_First, GridUnitType_Last - GridUnitType_First + 1)));
        }

        static ConvertibleKind GridUnitTypeToConvertibleKind(GridUnitType type)
        {
            Contract.Requires((int)type >= GridUnitType_First);
            Contract.Requires((int)type <= GridUnitType_Last);
            Contract.Ensures((int)Contract.Result<ConvertibleKind>() >= ConvertibleKind_GridUnitFirst);
            Contract.Ensures((int)Contract.Result<ConvertibleKind>() <= ConvertibleKind_GridUnitLast);

            return (ConvertibleKind)((int)type - GridUnitType_First + ConvertibleKind_GridUnitFirst);
        }

        static GridUnitType ConvertibleKindToGridUnitType(ConvertibleKind kind)
        {
            Contract.Requires((int)kind >= ConvertibleKind_GridUnitFirst);
            Contract.Requires((int)kind <= ConvertibleKind_GridUnitLast);
            Contract.Ensures((int)Contract.Result<GridUnitType>() >= GridUnitType_First);
            Contract.Ensures((int)Contract.Result<GridUnitType>() <= GridUnitType_Last);

            return (GridUnitType)((int)kind - ConvertibleKind_GridUnitFirst + GridUnitType_First);
        }

        #endregion

        #region From/To IConvertible

        static IConvertible ToIConvertible(object obj, IFormatProvider provider, out ConvertibleKind kind)
        {
            kind = ConvertibleKind.OneWay;

            if (obj == null)
                return null;

            try
            {
                var convertible = obj as IConvertible;
                if (convertible != null)
                    return convertible;

                if (obj is GridLength)
                {
                    var gl = (GridLength)obj;
                    kind = GridUnitTypeToConvertibleKind(gl.GridUnitType);
                    return gl.Value;
                }

                var formattable = obj as IFormattable;
                if (formattable != null)
                    return formattable.ToString(null, provider);

                return obj.ToString();
            }
            catch
            {
                return null;
            }
        }

        static object FromIConvertible(double value, ConvertibleKind kind)
        {
            switch (kind)
            {
                case ConvertibleKind.OneWay:
                    return value;
                case ConvertibleKind.GridUnitAuto:
                case ConvertibleKind.GridUnitPixel:
                case ConvertibleKind.GridUnitStar:
                    return new GridLength(value, ConvertibleKindToGridUnitType(kind));
                default:
                    throw new ArgumentOutOfRangeException("kind");
            }
        }

        #endregion

        #region Hardcore Conversion

        delegate double ConversionOperator(double value, double parameter);

        object DoConversion(object value, Type targetType, object parameter, CultureInfo culture,
            ConversionOperator op)
        {
            ConvertibleKind valueKind;
            var c_value = ToIConvertible(value, culture, out valueKind);
            if (c_value == null)
                return null;

            ConvertibleKind paramKind;
            var c_param = ToIConvertible(parameter, culture, out paramKind);
            if (c_param == null || paramKind != ConvertibleKind.OneWay)
                return null;

            double result;

            try
            {
                var d_value = c_value.ToDouble(culture);
                var d_param = c_param.ToDouble(culture);

                result = op(d_value, d_param);
            }
            catch
            {
                return null;
            }

            return FromIConvertible(result, valueKind);
        }

        #endregion

        #region Public Interface

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return this.DoConversion(value, targetType, parameter, culture, (v, p) => v * p);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return this.DoConversion(value, targetType, parameter, culture, (v, p) => v / p);
        }

        #endregion
    }
}
