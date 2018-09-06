using AutoMasshTik.Engine.Core;
using AutoMasshTik.Engine.Models;
using Avalonia.Data.Converters;
using System;
using System.Globalization;
using System.Linq;

namespace AutoMasshTik.UI.Converters
{
    public class ServersToProgressConverter : IValueConverter
    {
        public static ServersToProgressConverter Default { get; } = new ServersToProgressConverter();
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var servers = (Server[])value;
            return servers.Count(s => s.State == ServerUpdateState.Success || s.State == ServerUpdateState.Failure);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
