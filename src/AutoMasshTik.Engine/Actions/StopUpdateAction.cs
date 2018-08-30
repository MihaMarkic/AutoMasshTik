using Sharp.Redux;
using System;

namespace AutoMasshTik.Engine.Actions
{
    public class StopUpdateAction : ReduxAction
    {
        public bool IsCancelled { get; }
        public StopUpdateAction(bool isCancelled)
        {
            IsCancelled = isCancelled;
        }
    }
}
