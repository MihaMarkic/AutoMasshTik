using AutoMasshTik.Engine.Actions;
using AutoMasshTik.Engine.Core;
using AutoMasshTik.Engine.Models;
using AutoMasshTik.Engine.States;
using Sharp.Redux;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AutoMasshTik.Engine.Reducers
{
    public class RootReducer : IReduxReducer<RootState>
    {
        public Task<RootState> ReduceAsync(RootState state, ReduxAction action, CancellationToken ct)
        {
            RootState result;
            switch (action)
            {
                case ServersChangedAction serversChangedAction:
                    {
                        int index = 0;
                        var servers = serversChangedAction.ServersText.Split(new[] { Environment.NewLine }, StringSplitOptions.None)
                                        .Select(url => new Server(index++, url.Trim('\n', '\r'), isAlive: false, state: ServerUpdateState.Idle, error: null))
                                        .ToArray();
                        result = state.Clone(servers: servers);
                    }
                    break;
                case UsernameChangedAction usernameChangedAction:
                    result = state.Clone(username: usernameChangedAction.Username);
                    break;
                case PasswordChangedAction passwordChangedAction:
                    result = state.Clone(password: passwordChangedAction.Password);
                    break;
                case PortChangedAction portChangedAction:
                    result = state.Clone(port: portChangedAction.Port);
                    break;
                case StartUpdateAction startUpdateAction:
                    {
                        string operationInProgress;
                        switch (startUpdateAction.Mode)
                        {
                            case UpdateMode.Connection:
                                operationInProgress = "Testing connection";
                                break;
                            case UpdateMode.Firmware:
                                operationInProgress = "Upgrading firmware";
                                break;
                            case UpdateMode.Packages:
                                operationInProgress = "Upgrading packages";
                                break;
                            default:
                                operationInProgress = "???";
                                break;
                        }
                        var servers = state.Servers.Select(s => s.Clone(state: ServerUpdateState.Idle, error: "")).ToArray();
                        result = state.Clone(servers: servers, isUpdating: true, operationInProgress: operationInProgress);
                    }
                    break;
                case StopUpdateAction _:
                    result = state.Clone(isUpdating: false, operationInProgress: "");
                    break;
                case StartUpdatingServerAction startUpdatingServerAction:
                    {
                        var server = state.Servers.Single(i => i.Key == startUpdatingServerAction.Key);
                        var changed = server.Clone(state: ServerUpdateState.Updating, error: null);
                        var servers = state.Servers.Replace(server, changed);
                        result = state.Clone(servers: servers);
                    }
                    break;
                case ServerUpdateSuccessAction serverUpdateSuccessAction:
                    {
                        var server = state.Servers.Single(i => i.Key == serverUpdateSuccessAction.Key);
                        var changed = server.Clone(state: ServerUpdateState.Success, error: "");
                        var servers = state.Servers.Replace(server, changed);
                        result = state.Clone(servers: servers);
                    }
                    break;
                case ServerUpdateFailureAction serverUpdateFailureAction:
                    {
                        var server = state.Servers.Single(i => i.Key == serverUpdateFailureAction.Key);
                        var changed = server.Clone(state: ServerUpdateState.Failure, error: serverUpdateFailureAction.Error);
                        var servers = state.Servers.Replace(server, changed);
                        result = state.Clone(servers: servers);
                    }
                    break;
                case ToggleShowPasswordAction _:
                    result = state.Clone(showPassword: !state.ShowPassword);
                    break;
                default:
                    return Task.FromResult(state);
            }
            return Task.FromResult(result);
        }
    }
}
