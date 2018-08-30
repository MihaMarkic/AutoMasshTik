using Avalonia;
using Avalonia.Controls;
using Avalonia.Styling;

namespace AutoMasshTik.UI
{
    public static class AvaloniaExtensions
    {
        public static T AddChildren<T>(this T source, params IControl[] controls)
            where T: Panel
        {
            source.Children.AddRange(controls);
            return source;
        }
        public static T SetGridColumn<T>(this T source, int value)
            where T: AvaloniaObject
        {
            Grid.SetColumn(source, value);
            return source;
        }
        public static T SetGridRow<T>(this T source, int value)
            where T : AvaloniaObject
        {
            Grid.SetRow(source, value);
            return source;
        }
        public static T SetGridColumnSpan<T>(this T source, int value)
            where T : AvaloniaObject
        {
            Grid.SetColumnSpan(source, value);
            return source;
        }
        public static T SetGridRowSpan<T>(this T source, int value)
            where T : AvaloniaObject
        {
            Grid.SetRowSpan(source, value);
            return source;
        }
        public static T AddClasses<T>(this T source, params string[] classes)
            where T: Control
        {
            source.Classes.AddRange(classes);
            return source;
        }
        public static T AddClass<T>(this T source, string value)
            where T : Control
        {
            source.Classes.Add(value);
            return source;
        }
        public static T AddSetter<T>(this T source, Setter setter)
            where T: Style
        {
            source.Setters.Add(setter);
            return source;
        }
        public static T AddSetters<T>(this T source, params Setter[] setters)
            where T : Style
        {
            foreach (var setter in setters)
            {
                source.Setters.Add(setter);
            }
            return source;
        }
    }
}
