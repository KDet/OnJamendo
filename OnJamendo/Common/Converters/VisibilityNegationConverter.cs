using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace OnJamendo.Common.Converters
{
    public sealed class VisibilityNegationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return value is Visibility && (Visibility)value == Visibility.Collapsed ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return value is Visibility && (Visibility)value == Visibility.Collapsed ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}
