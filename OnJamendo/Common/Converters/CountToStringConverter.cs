using System;
using System.Linq;
using Windows.UI.Xaml.Data;

namespace OnJamendo.Common.Converters
{
    public sealed class CountToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var count = value is int ? (int)value : 0;
            if (count == 1)
                return "1 track";
            return count > 1 ? string.Format("{0} tracks", count) : "none";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            var countStr = value as string;
            if(countStr != null)
                if (char.IsDigit(countStr[0]))
                    return int.Parse(countStr.Split(' ').FirstOrDefault());
            return 0;
        }
    }
}
