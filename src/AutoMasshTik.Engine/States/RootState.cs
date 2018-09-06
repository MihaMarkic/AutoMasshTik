using AutoMasshTik.Engine.Models;
using Righthand.Immutable;

namespace AutoMasshTik.Engine.States
{
    public class RootState
    {
        public Server[] Servers { get; }
        public bool IsUpdating { get; }
        public string Username { get; }
        public string Password { get; }
        public int Port { get; }
        public string OperationInProgress { get; }
        public bool ShowPassword { get; }

        public RootState(Server[] servers, bool isUpdating, string username, string password, int port, string operationInProgress, bool showPassword)
        {
            Servers = servers;
            IsUpdating = isUpdating;
            Username = username;
            Password = password;
            Port = port;
            OperationInProgress = operationInProgress;
            ShowPassword = showPassword;
        }

        public RootState Clone(Param<Server[]>? servers = null, Param<bool>? isUpdating = null, Param<string>? username = null, Param<string>? password = null, Param<int>? port = null, Param<string>? operationInProgress = null, Param<bool>? showPassword = null)
        {
            return new RootState(servers.HasValue ? servers.Value.Value : Servers,
				isUpdating.HasValue ? isUpdating.Value.Value : IsUpdating,
				username.HasValue ? username.Value.Value : Username,
				password.HasValue ? password.Value.Value : Password,
				port.HasValue ? port.Value.Value : Port,
				operationInProgress.HasValue ? operationInProgress.Value.Value : OperationInProgress,
				showPassword.HasValue ? showPassword.Value.Value : ShowPassword);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;
            var o = (RootState)obj;
            return Equals(Servers, o.Servers) && Equals(IsUpdating, o.IsUpdating) && Equals(Username, o.Username) && Equals(Password, o.Password) && Equals(Port, o.Port) && Equals(OperationInProgress, o.OperationInProgress) && Equals(ShowPassword, o.ShowPassword);}

        public override int GetHashCode()
        {
            unchecked
			{
				int hash = 23;
				hash = hash * 37 + (Servers != null ? Servers.GetHashCode() : 0);
				hash = hash * 37 + IsUpdating.GetHashCode();
				hash = hash * 37 + (Username != null ? Username.GetHashCode() : 0);
				hash = hash * 37 + (Password != null ? Password.GetHashCode() : 0);
				hash = hash * 37 + Port.GetHashCode();
				hash = hash * 37 + (OperationInProgress != null ? OperationInProgress.GetHashCode() : 0);
				hash = hash * 37 + ShowPassword.GetHashCode();
				return hash;
			}
        }
    }
}
