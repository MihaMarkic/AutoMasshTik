using AutoMasshTik.Engine.Core;
using AutoMasshTik.Engine.Models;
using Sharp.Redux;

namespace AutoMasshTik.UI.ViewModels
{
    public class ServerViewModel : NotifiableObject, IBoundViewModel<Server>
    {
        public Server State { get; private set; }
        public string Url { get; private set; }
        public ServerUpdateState ServerState { get; private set; }
        public string Error { get; private set; }
        bool isUpdatingState;
        public ServerViewModel(Server state)
        {
            State = state;
            Update(state);
        }
        public void Update(Server state)
        {
            isUpdatingState = true;
            try
            {
                State = state;
                Url = state.Url;
                ServerState = state.State;
                Error = state.Error;
            }
            finally
            {
                isUpdatingState = false;
            }
        }
    }
}
