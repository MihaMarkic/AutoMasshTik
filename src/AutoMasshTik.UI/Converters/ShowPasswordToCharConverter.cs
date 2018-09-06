using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace AutoMasshTik.UI.Converters
{
    class ShowPasswordToCharConverter : IValueConverter
    {
        public static ShowPasswordToCharConverter Default { get; } = new ShowPasswordToCharConverter();
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((bool)value) ? '\0' : '*';
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
