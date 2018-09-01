using Avalonia;
using Avalonia.Logging.Serilog;

namespace AutoMasshTik.UI
{
    class Program
    {
        static void Main()
        {
            IoC.Init();
            try
            {
                BuildAvaloniaApp().Start<MainWindow>();
            }
            finally
            {
                IoC.Dispose();
            }
        }

        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .LogToDebug();
    }
}
