using Avalonia.Markup;
using System;
using System.Globalization;

namespace AutoMasshTik.UI.Converters
{
    public class NegateConverter : IValueConverter
    {
        public static NegateConverter Default { get; } = new NegateConverter();
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !(bool)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
