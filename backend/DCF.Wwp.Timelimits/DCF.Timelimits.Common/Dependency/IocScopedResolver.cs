using System;
using System.Collections.Generic;
using System.Linq;
using DCF.Core.Dependency;

namespace DCF.Common.Dependency
{
    internal class IocScopedResolver : IIocScopedResolver
    {
        private readonly IIocResolver _iocResolver;
        private readonly List<Object> _resolvedObjects;

        public IocScopedResolver(IIocResolver iocResolver)
        {
            this._iocResolver = iocResolver;
            this._resolvedObjects = new List<Object>();
        }

        public T Resolve<T>()
        {
            return this.Resolve<T>(typeof(T));
        }

        public T Resolve<T>(Type type)
        {
            return (T)this.Resolve(type);
        }

        public T Resolve<T>(Object argumentsAsAnonymousType)
        {
            return (T)this.Resolve(typeof(T), argumentsAsAnonymousType);
        }

        public Object Resolve(Type type)
        {
            return this.Resolve(type, null);
        }

        public Object Resolve(Type type, Object argumentsAsAnonymousType)
        {
            var resolvedObject = argumentsAsAnonymousType != null
                ? this._iocResolver.Resolve(type, argumentsAsAnonymousType)
                : this._iocResolver.Resolve(type);

            this._resolvedObjects.Add(resolvedObject);
            return resolvedObject;
        }

        public T[] ResolveAll<T>()
        {
            return this.ResolveAll(typeof(T)).OfType<T>().ToArray();
        }

        public T[] ResolveAll<T>(Object argumentsAsAnonymousType)
        {
            return this.ResolveAll(typeof(T), argumentsAsAnonymousType).OfType<T>().ToArray();
        }

        public Object[] ResolveAll(Type type)
        {
            return this.ResolveAll(type, null);
        }

        public Object[] ResolveAll(Type type, Object argumentsAsAnonymousType)
        {
            var resolvedObjects = argumentsAsAnonymousType != null
                ? this._iocResolver.ResolveAll(type, argumentsAsAnonymousType)
                : this._iocResolver.ResolveAll(type);

            this._resolvedObjects.AddRange(resolvedObjects);
            return resolvedObjects;
        }

        public void Release(Object obj)
        {
            this._resolvedObjects.Remove(obj);
            this._iocResolver.Release(obj);
        }

        public Boolean IsRegistered(Type type)
        {
            return this._iocResolver.IsRegistered(type);
        }

        public Boolean IsRegistered<T>()
        {
            return this.IsRegistered(typeof(T));
        }

        public void Dispose()
        {
            this._resolvedObjects.ForEach(this._iocResolver.Release);
        }
    }
}
