using Autofac;
using AutoMasshTik.Engine;
using AutoMasshTik.UI.ViewModels;

namespace AutoMasshTik.UI
{
    public static class IoC
    {
        public static void Init()
        {
            var builder = IoCRegistrar.Register();
            builder.RegisterType<MainViewModel>();
            builder.RegisterType<ServerViewModel>();
            IoCRegistrar.Build(builder);
        }
        public static void Dispose()
        {
            IoCRegistrar.Container.Dispose();
        }
    }
}
