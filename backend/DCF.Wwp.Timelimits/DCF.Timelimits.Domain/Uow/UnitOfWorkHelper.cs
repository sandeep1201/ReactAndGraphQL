using System;
using System.Reflection;

namespace DCF.Core.Domain.Uow
{
    /// <summary>
    /// A helper class to simplify unit of work process.
    /// </summary>
    internal static class UnitOfWorkHelper
    {
        /// <summary>
        /// Returns true if UOW must be used for given type as convention.
        /// </summary>
        /// <param name="type">Type to check</param>
        public static Boolean IsConventionalUowClass(Type type)
        {
            //TODO: return typeof(IRepository).IsAssignableFrom(type) || typeof(IApplicationService).IsAssignableFrom(type);
            return false;
        }

        /// <summary>
        /// Returns true if given method has UnitOfWorkAttribute attribute.
        /// </summary>
        /// <param name="methodInfo">Method info to check</param>
        public static Boolean HasUnitOfWorkAttribute(MemberInfo methodInfo)
        {
            return methodInfo.IsDefined(typeof(UnitOfWorkAttribute), true);
        }

        /// <summary>
        /// Returns UnitOfWorkAttribute it exists.
        /// </summary>
        /// <param name="methodInfo">Method info to check</param>
        public static UnitOfWorkAttribute GetUnitOfWorkAttributeOrNull(MemberInfo methodInfo)
        {
            var attrs = methodInfo.GetCustomAttributes(typeof (UnitOfWorkAttribute), false);
            if (attrs.Length <= 0)
            {
                return null;
            }

            return (UnitOfWorkAttribute) attrs[0];
        }
    }
}