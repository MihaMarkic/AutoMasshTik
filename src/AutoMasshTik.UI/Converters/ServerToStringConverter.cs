using AutoMasshTik.UI.ViewModels;
using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace AutoMasshTik.UI.Converters
{
    public class ServerToStringConverter : IValueConverter
    {
        public static ServerToStringConverter Default { get; } = new ServerToStringConverter();
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var server = (ServerViewModel)value;
            return server.Url;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
