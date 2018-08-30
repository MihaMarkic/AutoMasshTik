using Avalonia.Markup;
using Avalonia.Media;
using System;
using System.Globalization;

namespace AutoMasshTik.UI.Converters
{
    public class NotEmptyRequiredToBrushConverter : IValueConverter
    {
        public static NotEmptyRequiredToBrushConverter Default { get; } = new NotEmptyRequiredToBrushConverter();
        static readonly IBrush requiredBrush = new SolidColorBrush(Colors.DarkRed);
        static readonly IBrush defaultBrush = new SolidColorBrush(Colors.Black);
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (string.IsNullOrWhiteSpace((string)value))
            {
                return requiredBrush;
            }
            return defaultBrush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
