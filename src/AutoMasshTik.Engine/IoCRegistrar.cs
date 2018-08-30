using Autofac;
using AutoMasshTik.Engine.Models;
using AutoMasshTik.Engine.Reducers;
using AutoMasshTik.Engine.Services.Abstract;
using AutoMasshTik.Engine.Services.Implementation;
using AutoMasshTik.Engine.States;
using Sharp.Redux;
using System.Linq;

namespace AutoMasshTik.Engine
{
    public static class IoCRegistrar
    {
        public static IContainer Container { get; private set; }
        public static ContainerBuilder Register()
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterType<RootReducer>().As<IReduxReducer<RootState>>().SingleInstance();
            builder.RegisterType<Updater>().As<IUpdater>();
            // register root dispatcher and initialize state
#if DEBUG
            var servers = Enumerable.Range(1, 50).Select(i => new Server(i-1, $"xxx", false, Core.ServerUpdateState.Idle, null)).ToArray();
            //appReduxDispatcher.Dispatch(new ServersChangedAction(text));
#endif
            builder.Register<IAppReduxDispatcher>(ctx => new AppReduxDispatcher(
                initialState: new RootState(
                    servers: new Server[0],
                    isUpdating: false,
                    username: "",
                    password: "",
                    port: 22,
                    operationInProgress: ""
                ),
                reducer: ctx.Resolve<IReduxReducer<RootState>>())).SingleInstance();
            return builder;
        }
        public static void Build(ContainerBuilder builder)
        {
            Container = builder.Build();
        }
    }
}
