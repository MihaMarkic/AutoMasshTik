using Sharp.Redux;

namespace AutoMasshTik.Engine.Actions
{
    public class StartUpdatingServerAction: ReduxAction
    {
        public int Key { get; }
        public StartUpdatingServerAction(int key)
        {
            Key = key;
        }
    }
}
