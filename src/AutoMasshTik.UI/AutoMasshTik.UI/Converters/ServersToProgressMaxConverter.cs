using AutoMasshTik.Engine.Models;
using Avalonia.Markup;
using System;
using System.Globalization;

namespace AutoMasshTik.UI.Converters
{
    public class ServersToProgressMaxConverter : IValueConverter
    {
        public static ServersToProgressMaxConverter Default { get; } = new ServersToProgressMaxConverter();
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var servers = (Server[])value;
            return servers.Length;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
