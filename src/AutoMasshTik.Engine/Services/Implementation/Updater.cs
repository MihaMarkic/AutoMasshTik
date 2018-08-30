using AutoMasshTik.Engine.Actions;
using AutoMasshTik.Engine.Core;
using AutoMasshTik.Engine.Models;
using AutoMasshTik.Engine.Services.Abstract;
using Renci.SshNet;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AutoMasshTik.Engine.Services.Implementation
{
    public class Updater : DisposableObject, IUpdater
    {
        readonly IAppReduxDispatcher appReduxDispatcher;
        static readonly Task Completed = Task.FromResult(true);
        public Updater(IAppReduxDispatcher appReduxDispatcher)
        {
            this.appReduxDispatcher = appReduxDispatcher;
        }
        public Task UpdateAsync(UpdateMode mode, Server[] servers, string username, string password, int port, bool useCredentials, CancellationToken ct)
        {
            return Task.Run(async () => {
                var tasks = servers.Select(s => 
                    Task.Run(() =>
                    {
                        switch (mode)
                        {
                            case UpdateMode.Packages:
                                return UpdateServerAsync(s, username, password, port, action: UpdatePackages, ct);
                            case UpdateMode.Firmware:
                                return UpdateServerAsync(s, username, password, port, action: UpdateFirmware, ct);
                            case UpdateMode.Connection:
                            default:
                                return UpdateServerAsync(s, username, password, port, action: null, ct);
                        };
                    }
            )).ToArray();
                await Task.WhenAll(tasks);
            });
        }

        static Task UpdatePackages(SshClient client, CancellationToken ct)
        {
            ExecuteCommand(client, "/system package update install");
            return Completed;
        }
        static void ExecuteCommand(SshClient client, string text)
        {
            var command = client.RunCommand(text);
            if (command.ExitStatus != 0)
            {
                throw new Exception($"Command '{text}' failed with {command.ExitStatus}:{command.Error}");
            }
        }
        static async Task UpdateFirmware(SshClient client, CancellationToken ct)
        {
            ExecuteCommand(client, "/system routerboard upgrade");
            ct.ThrowIfCancellationRequested();
            await Task.Delay(2000, ct);
            ct.ThrowIfCancellationRequested();
            ExecuteCommand(client, "/system reboot\ny");
        }

        async Task UpdateServerAsync(Server server, string username, string password, int port, Func<SshClient, CancellationToken, Task> action, CancellationToken ct)
        {
            appReduxDispatcher.Dispatch(new StartUpdatingServerAction(server.Key));
            try
            {
                using (SshClient client = new SshClient(server.Url, port, username, password))
                {
                    ct.ThrowIfCancellationRequested();
                    client.Connect();
                    ct.ThrowIfCancellationRequested();
                    if (action != null)
                    {
                        await action(client, ct).ConfigureAwait(false);
                    }
                    appReduxDispatcher.Dispatch(new ServerUpdateSuccessAction(server.Key));
                }
            }
            catch (Exception ex)
            {
                appReduxDispatcher.Dispatch(new ServerUpdateFailureAction(server.Key, ex.Message));
            }
        }
    }
}
