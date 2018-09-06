using AutoMasshTik.Engine.Core;
using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace AutoMasshTik.UI.Converters
{
    public class ServerUpdateStateToStringConverter : IValueConverter
    {
        public static ServerUpdateStateToStringConverter Default { get; } = new ServerUpdateStateToStringConverter();
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((ServerUpdateState)value)
            {
                case ServerUpdateState.Idle:
                    return "Idle";
                case ServerUpdateState.Failure:
                    return "Failed";
                case ServerUpdateState.Success:
                    return "Success";
                case ServerUpdateState.Updating:
                    return "Updating";
                default:
                    return "?";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
