using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DCF.Core.Dependency;

namespace DCF.Common.Dependency
{
    internal class DisposableDependencyObjectWrapper : DisposableDependencyObjectWrapper<Object>,
        IDisposableDependencyObjectWrapper
    {
        public DisposableDependencyObjectWrapper(IIocResolver iocResolver, Object obj)
            : base(iocResolver, obj)
        {

        }
    }

    internal class DisposableDependencyObjectWrapper<T> : IDisposableDependencyObjectWrapper<T>
    {
        private readonly IIocResolver _iocResolver;

        public T Object { get; private set; }

        public DisposableDependencyObjectWrapper(IIocResolver iocResolver, T obj)
        {
            _iocResolver = iocResolver;
            Object = obj;
        }

        public void Dispose()
        {
            _iocResolver.Release(Object);
        }
    }
}