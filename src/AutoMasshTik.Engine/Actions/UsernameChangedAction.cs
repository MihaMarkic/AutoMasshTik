using Sharp.Redux;

namespace AutoMasshTik.Engine.Actions
{
    public class UsernameChangedAction: ReduxAction
    {
        public string Username { get; }
        public UsernameChangedAction(string username)
        {
            Username = username;
        }
    }
}
