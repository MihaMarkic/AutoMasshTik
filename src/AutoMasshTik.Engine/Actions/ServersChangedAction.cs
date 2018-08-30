using AutoMasshTik.Engine.Models;
using Sharp.Redux;

namespace AutoMasshTik.Engine.Actions
{
    public class ServersChangedAction: ReduxAction
    {
        public string ServersText { get; }
        public ServersChangedAction(string serversText)
        {
            ServersText = serversText;
        }
    }
}
