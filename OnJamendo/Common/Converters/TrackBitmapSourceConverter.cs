using System;
using OnJamendo.Model;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Imaging;

namespace OnJamendo.Common.Converters
{
    public sealed class TrackBitmapSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var track = value as Track;
            return track != null ? new BitmapImage(new Uri(track.Album_Image, UriKind.RelativeOrAbsolute)) : null;
           
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }
}
