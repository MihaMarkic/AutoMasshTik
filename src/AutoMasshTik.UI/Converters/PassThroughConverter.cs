using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace AutoMasshTik.UI.Converters
{
    public class PassThroughConverter : IValueConverter
    {
        public static PassThroughConverter Default { get; } = new PassThroughConverter();
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
