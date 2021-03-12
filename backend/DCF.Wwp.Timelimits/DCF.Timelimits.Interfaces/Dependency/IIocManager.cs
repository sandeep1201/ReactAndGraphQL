using System;
using Castle.Windsor;

namespace DCF.Core.Dependency
{
    public interface IIocManager : IIocRegistrar, IIocResolver, IDisposable
    {
        /// <summary>
        /// Reference to the windor AppContainer.
        /// </summary>
        IWindsorContainer IocContainer { get; }

        /// <summary>
        /// Checks whether given type is registered before.
        /// </summary>
        /// <param name="type">Type to check</param>
        new Boolean IsRegistered(Type type);

        /// <summary>
        /// Checks whether given type is registered before.
        /// </summary>
        /// <typeparam name="T">Type to check</typeparam>
        new Boolean IsRegistered<T>();

    }
}