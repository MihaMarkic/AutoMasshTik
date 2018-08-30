using Sharp.Redux;

namespace AutoMasshTik.Engine.Actions
{
    public class StartUpdateAction: ReduxAction
    {
        public UpdateMode Mode { get; }
        public StartUpdateAction(UpdateMode mode)
        {
            Mode = mode;
        }
    }
}
