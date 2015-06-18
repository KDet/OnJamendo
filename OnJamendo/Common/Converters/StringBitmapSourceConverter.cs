using System;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Imaging;

namespace OnJamendo.Common.Converters
{
    public sealed class StringBitmapSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var str = value as string;
            return str != null ? new BitmapImage(new Uri(str, UriKind.RelativeOrAbsolute)) : null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            var bitmap = value as BitmapImage;
            return bitmap != null ? bitmap.UriSource.AbsolutePath : null;
        }
    }
}
