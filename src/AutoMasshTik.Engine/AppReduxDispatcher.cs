using AutoMasshTik.Engine.States;
using Sharp.Redux;
using System.Threading.Tasks;

namespace AutoMasshTik.Engine
{
    public class AppReduxDispatcher : ReduxDispatcher<RootState, IReduxReducer<RootState>>, IAppReduxDispatcher
    {
        /// <summary>
        /// Initialize a new instance of the <see cref="AppReduxDispatcher`2"/> that has initial state uses given reducer and runs synchronized event
        /// on <paramref name="notificationScheduler"/>.
        /// </summary>
        /// <param name="initialState">The initial state.</param>
        /// <param name="reducer">The reducer to use.</param>
        /// <param name="notificationScheduler">A scheduler for events.</param>
        public AppReduxDispatcher(RootState initialState, IReduxReducer<RootState> reducer) : 
            base(initialState, reducer, TaskScheduler.FromCurrentSynchronizationContext())
        {

        }
    }
}
