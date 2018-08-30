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
        public Updater(IAppReduxDispatcher appReduxDispatcher)
        {
            this.appReduxDispatcher = appReduxDispatcher;
        }
        public Task UpdateAsync(Server[] servers, string username, string password, int port, bool useCredentials, CancellationToken ct)
        {
            return Task.Run(async () => {
                var tasks = servers.Select(s => Task.Run(() => UpdateServerAsync(s, username, password, port, action: null, ct))).ToArray();
                await Task.WhenAll(tasks);
            });
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
