using System;
using Windows.UI.Xaml.Data;

namespace OnJamendo.Common.Converters
{
    public sealed class IntegerToTimeSpanStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var integer = value is int ? (int) value : 0;
            return TimeSpan.FromSeconds(integer).ToString("c");
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            var timeStamp = value as string;
            if (timeStamp != null)
                return TimeSpan.Parse(timeStamp).Ticks;
            return default(TimeSpan);
        }
    }
}
