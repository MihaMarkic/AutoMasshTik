using Sharp.Redux;

namespace AutoMasshTik.Engine.Actions
{
    public class ServerUpdateSuccessAction: ReduxAction
    {
        public int Key { get; }
        public ServerUpdateSuccessAction(int key)
        {
            Key = key;
        }
    }
}
