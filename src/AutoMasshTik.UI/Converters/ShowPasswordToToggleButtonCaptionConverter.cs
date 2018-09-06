using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace AutoMasshTik.UI.Converters
{
    public class ShowPasswordToToggleButtonCaptionConverter : IValueConverter
    {
        public static ShowPasswordToToggleButtonCaptionConverter Default { get; } = new ShowPasswordToToggleButtonCaptionConverter();
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((bool)value) ? "Hide" : "Show";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
