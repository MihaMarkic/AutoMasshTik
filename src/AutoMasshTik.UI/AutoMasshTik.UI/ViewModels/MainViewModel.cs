using AutoMasshTik.Engine;
using AutoMasshTik.Engine.Actions;
using AutoMasshTik.Engine.Core;
using AutoMasshTik.Engine.Models;
using AutoMasshTik.Engine.Services.Abstract;
using AutoMasshTik.Engine.States;
using Sharp.Redux;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;

namespace AutoMasshTik.UI.ViewModels
{
    public class MainViewModel: NotifiableObject
    {
        readonly IAppReduxDispatcher appReduxDispatcher;
        readonly IUpdater updater;
        readonly Func<Server, ServerViewModel> serverViewModelFactory;
        public string ServersText { get; private set; }
        public Server[] ServerModels { get; private set; }
        public string Username { get; private set; }
        public string Password { get; private set; }
        public int Port { get; private set; }
        public  bool IsUpdating { get; private set; }
        public ObservableCollection<ServerViewModel> Servers { get; }
        public RelayCommand<UpdateMode> StartUpdateCommand { get; }
        public RelayCommand StopUpdateCommand { get; }
        bool isUpdatingState;
        CancellationTokenSource cts;
        RootState state;
        public MainViewModel(IAppReduxDispatcher appReduxDispatcher, IUpdater updater, Func<Server, ServerViewModel> serverViewModelFactory)
        {
            this.appReduxDispatcher = appReduxDispatcher;
            this.updater = updater;
            this.serverViewModelFactory = serverViewModelFactory;
            state = appReduxDispatcher.InitialState;
            ServerModels = state.Servers;
            Servers = new ObservableCollection<ServerViewModel>();
            StartUpdateCommand = new RelayCommand<UpdateMode>(StartUpdate, m => !IsUpdating);
            StopUpdateCommand = new RelayCommand(StopUpdate, () => IsUpdating);
            this.appReduxDispatcher.StateChanged += AppReduxDispatcher_StateChanged;
        }
        async void StartUpdate(UpdateMode mode)
        {
            appReduxDispatcher.Dispatch(new StartUpdateAction(mode));
            cts?.Cancel();
            cts = new CancellationTokenSource();
            try
            {
                await updater.UpdateAsync(state.Servers, Username, Password, Port, useCredentials: true, cts.Token);
            }
            catch (Exception ex)
            {
                
            }
            finally
            {
                appReduxDispatcher.Dispatch(new StopUpdateAction(false));
            }
        }
        void StopUpdate()
        {
            cts?.Cancel();
        }
        public void Start()
        {
            appReduxDispatcher.Start();
        }
        private void AppReduxDispatcher_StateChanged(object sender, StateChangedEventArgs<RootState> e)
        {
            isUpdatingState = true;
            try
            {
                state = e.State;
                ServerModels = state.Servers;
                ServersText = string.Join(Environment.NewLine, e.State.Servers.Select(s => s.Url));
                ReduxMerger.MergeList<int, Server, ServerViewModel>(state.Servers, Servers, serverViewModelFactory);
                Username = e.State.Username;
                Password = e.State.Password;
                IsUpdating = e.State.IsUpdating;
                Port = e.State.Port;
            }
            finally
            {
                isUpdatingState = false;
            }
        }
        protected override void OnPropertyChanged(string name)
        {
            if (!isUpdatingState)
            {
                switch (name)
                {
                    case nameof(ServersText):
                        appReduxDispatcher.Dispatch(new ServersChangedAction(ServersText));
                        break;
                    case nameof(Username):
                        appReduxDispatcher.Dispatch(new UsernameChangedAction(Username));
                        break;
                    case nameof(Password):
                        appReduxDispatcher.Dispatch(new PasswordChangedAction(Password));
                        break;
                    case nameof(Port):
                        appReduxDispatcher.Dispatch(new PortChangedAction(Port));
                        break;
                }
            }
            switch (name)
            {
                case nameof(IsUpdating):
                    StartUpdateCommand.RaiseCanExecuteChanged();
                    StopUpdateCommand.RaiseCanExecuteChanged();
                    break;
            }
            base.OnPropertyChanged(name);
        }
    }
}
