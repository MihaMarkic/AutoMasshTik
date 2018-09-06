using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace AutoMasshTik.UI.Converters
{
    public class IntToStringConverter : IValueConverter
    {
        public static IntToStringConverter Default { get; } = new IntToStringConverter();
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (int.TryParse((string)value, out int numeric))
            {
                return numeric;
            }
            return null;
        }
    }
}
