using System;
using DCF.Core.Dependency;

namespace DCF.Core.Container
{
    public sealed class AppContainer : IDisposable
    {
        public IContainerAdapter _adapter {get; private set;}
        public AppContainer(IContainerAdapter adapater){
            this._adapter = adapater;
        }

        public T Resolve<T>(){
            return this._adapter.Resolve<T>();
        }

        public T TryResolve<T>(){
            return this._adapter.TryResolve<T>();
        }

        #region IDisposable Members
        
        public void Dispose()
        {
            // TODO
            throw new NotImplementedException();
        }

        #endregion
    }
}
