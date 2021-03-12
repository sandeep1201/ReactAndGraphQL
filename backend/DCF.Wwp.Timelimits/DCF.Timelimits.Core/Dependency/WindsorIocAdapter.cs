using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Windsor;

namespace DCF.Core.Dependency
{
    public class WindsorIocAdapter : IContainerAdapter
    {

        #region IContainerAdapter Members
        private Castle.Windsor.IWindsorContainer _container;

        public WindsorIocAdapter(IWindsorContainer container)
        {
            this._container = container;
        }

        public T Resolve<T>()
        {
            return this._container.Resolve<T>();
        }

        public T TryResolve<T>()
        {
            if(this._container.Kernel.HasComponent(typeof(T))){
                return this._container.Resolve<T>();
            }
            return default(T);
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            this._container.Dispose();
        }

        #endregion
    }
}
