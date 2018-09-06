using AutoMasshTik.Engine.Core;
using Avalonia.Data.Converters;
using Avalonia.Media;
using System;
using System.Globalization;

namespace AutoMasshTik.UI.Converters
{
    public class ServerUpdateStateToBrushConverter : IValueConverter
    {
        static readonly IBrush failureBrush = new SolidColorBrush(Colors.DarkRed);
        static readonly IBrush successBrush = new SolidColorBrush(Colors.DarkGreen);
        static readonly IBrush defaultBrush = new SolidColorBrush(Colors.Black);
        static readonly IBrush updatingBrush = new SolidColorBrush(Colors.DarkBlue);
        public static ServerUpdateStateToBrushConverter Default { get; } = new ServerUpdateStateToBrushConverter();
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var state = (ServerUpdateState)value;
            switch (state)
            {
                case  ServerUpdateState.Failure:
                    return failureBrush;
                case ServerUpdateState.Success:
                    return successBrush;
                case ServerUpdateState.Updating:
                    return updatingBrush;
                default:
                    return defaultBrush;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
