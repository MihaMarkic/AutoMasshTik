using System;

namespace AutoMasshTik.Engine.Core
{
    public abstract class DisposableObject : IDisposable
    {
        protected bool disposed;

        protected virtual void Dispose(bool disposing)
        {
            disposed = true;
        }

        public bool IsDisposed
        {
            get { return disposed; }
        }

        #region IDisposable Members
        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
