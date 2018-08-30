using Sharp.Redux;

namespace AutoMasshTik.Engine.Actions
{
    public class PasswordChangedAction: ReduxAction
    {
        public string Password { get; }
        public PasswordChangedAction(string password)
        {
            Password = password;
        }
    }
}
