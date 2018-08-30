using Sharp.Redux;

namespace AutoMasshTik.Engine.Actions
{
    public class PortChangedAction: ReduxAction
    {
        public int Port { get; }
        public PortChangedAction(int port)
        {
            Port = port;
        }
    }
}
