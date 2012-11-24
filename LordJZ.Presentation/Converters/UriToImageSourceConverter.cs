using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace LordJZ.Presentation.Converters
{
    [ValueConversion(typeof(string), typeof(ImageSource))]
    public sealed class UriToImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var source = value as ImageSource;
            if (source != null)
                return source;

            Uri uri;

            var str = value as string;
            if (str != null)
                uri = new Uri(str, UriKind.Relative);
            else
                uri = value as Uri;

            if (uri != null)
                return new BitmapImage(uri);

            var image = value as Image;
            if (image != null)
                return image.Source;

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
