using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace AutoMasshTik.Engine.Core
{
    /// <summary>
    /// Base class that implements <see cref="INotifyPropertyChanged"/>.
    /// </summary>
    public abstract class NotifiableObject : DisposableObject, INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName]string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        #endregion
    }
}
