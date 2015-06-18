using System;
using Windows.UI.Xaml.Data;

namespace OnJamendo.Common.Converters
{
    public sealed class TimeSpanToDoubleConverter :IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var timeSpan = value is TimeSpan ? (TimeSpan)value : new TimeSpan();
            return timeSpan.TotalSeconds;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return TimeSpan.FromSeconds(value is double ? (double)value : 0.0);
        }
    }
}
