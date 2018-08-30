using Sharp.Redux;

namespace AutoMasshTik.Engine.Actions
{
    public class ServerUpdateFailureAction : ReduxAction
    {
        public int Key { get; }
        public string Error { get; }
        public ServerUpdateFailureAction(int key, string error)
        {
            Key = key;
            Error = error;
        }
    }
}
