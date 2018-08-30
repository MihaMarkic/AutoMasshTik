using AutoMasshTik.Engine.Core;
using Righthand.Immutable;
using Sharp.Redux;

namespace AutoMasshTik.Engine.Models
{
    public readonly struct Server: IKeyedItem<int>
    {
        public int Key { get; }
        public string Url { get; }
        public bool IsAlive { get; }
        public ServerUpdateState State { get; }
        public string Error { get; }

        public Server(int key, string url, bool isAlive, ServerUpdateState state, string error)
        {
            Key = key;
            Url = url;
            IsAlive = isAlive;
            State = state;
            Error = error;
        }

        public Server Clone(Param<int>? key = null, Param<string>? url = null, Param<bool>? isAlive = null, Param<ServerUpdateState>? state = null, Param<string>? error = null)
        {
            return new Server(key.HasValue ? key.Value.Value : Key,
				url.HasValue ? url.Value.Value : Url,
				isAlive.HasValue ? isAlive.Value.Value : IsAlive,
				state.HasValue ? state.Value.Value : State,
				error.HasValue ? error.Value.Value : Error);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;
            var o = (Server)obj;
            return Equals(Key, o.Key) && Equals(Url, o.Url) && Equals(IsAlive, o.IsAlive) && Equals(State, o.State) && Equals(Error, o.Error);}

        public override int GetHashCode()
        {
            unchecked
			{
				int hash = base.GetHashCode();
				hash = hash * 37 + Key.GetHashCode();
				hash = hash * 37 + (Url != null ? Url.GetHashCode() : 0);
				hash = hash * 37 + IsAlive.GetHashCode();
				hash = hash * 37 + State.GetHashCode();
				hash = hash * 37 + (Error != null ? Error.GetHashCode() : 0);
				return hash;
			}
        }
    }
}
