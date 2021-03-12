using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DCF.Core;
using EnsureThat;
using FastMember;

namespace DCF.Core
{
    /// <summary>
    /// Extension methods for checking a type's structure.
    /// </summary>
    public static class TypeExtensions
    {
        public static object GetDefaultValue<T>(this T type) where T : Type
        {
            return TypeExtensions.GetTypeDefaultValue(type);
        }

        private static Object GetTypeDefaultValue(Type t)
        {
            // If no Type was supplied, if the Type was a reference type, or if the Type was a System.Void, return null
            if (t == null || !t.IsValueType || t == typeof(void))
                return null;

            // If the supplied Type has generic parameters, its default value cannot be determined
            if (t.ContainsGenericParameters)
                throw new ArgumentException(
                    "{" + MethodInfo.GetCurrentMethod() + "} Error:\n\nThe supplied value type <" + t +
                    "> contains generic parameters, so the default value cannot be retrieved");

            // If the Type is a primitive type, or if it is another publicly-visible value type (i.e. struct/enum), return a 
            //  default instance of the value type
            if (t.IsPrimitive || !t.IsNotPublic)
            {
                try
                {
                    return Activator.CreateInstance(t);
                }
                catch (Exception e)
                {
                    throw new ArgumentException(
                        "{" + MethodInfo.GetCurrentMethod() + "} Error:\n\nThe Activator.CreateInstance method could not " +
                        "create a default instance of the supplied value type <" + t +
                        "> (Inner Exception message: \"" + e.Message + "\")", e);
                }
            }

            // Fail with exception
            throw new ArgumentException("{" + MethodInfo.GetCurrentMethod() + "} Error:\n\nThe supplied value type <" + t +
                                        "> is not a publicly-visible type, so the default value cannot be retrieved");
        }
        /// <summary>
        /// Creates an instance of a type using the constructor with matching parameter types.
        /// </summary>
        /// <typeparam name="T">The type to instantiate.</typeparam>
        /// <param name="args">The arguments to pass to a matching constructor.</param>
        /// <returns>The created instance.</returns>
        public static T CreateInstance<T>(params Object[] args)
        {
            return (T)TypeExtensions.CreateInstance(typeof(T), args);

        }

        /// <summary>
        /// Creates an instance of a type using the constructor with matching parameter types.
        /// </summary>
        /// <param name="type">The type object representing the type.</param>
        /// <param name="args">The arguments to pass to a matching constructor.</param>
        /// <returns>The created instance.</returns>
        public static Object CreateInstance(this Type type, params Object[] args)
        {
            Ensure.That(type, nameof(type)).IsNotNull();

            Object instance = null;

            if (args == null || args.Length == 0)
            {
                //Try fastmemeber first
                var accesssor = TypeAccessor.Create(type);
                if (accesssor.CreateNewSupported)
                {
                    instance = accesssor.CreateNew();
                }
            }

            if (instance.IsDefaultValue())
            {
                instance = Activator.CreateInstance(type, args);
            }

            return instance ?? type.GetDefaultValue();
        }

        /// <summary>
        /// Determines if a type derives from a base type.
        /// </summary>
        /// <typeparam name="T">The parent type.</typeparam>
        /// <param name="childType">The child type to check.</param>
        /// <returns>True if the type derives from the specified base type.
        /// otherwise false.</returns>
        public static Boolean IsDerivedFrom<T>(this Type childType)
        {
            Ensure.That(childType, nameof(childType)).IsNotNull();

            return typeof(T).IsAssignableFrom(childType);
        }

        /// <summary>
        /// Determines if a type derives from a parent type.
        /// </summary>
        /// <param name="type">The child type to check.</param>
        /// <param name="baseType">The parent type.</param>
        /// <returns></returns>
        public static Boolean IsDerivedFrom(this Type childType, Type parentType)
        {
            Ensure.That(childType, nameof(childType)).IsNotNull();
            Ensure.That(parentType, nameof(parentType)).IsNotNull();

            return parentType.IsAssignableFrom(childType);
        }

        public static Boolean IsCreatableType(this Type type)
        {
            return type.IsClass && !type.IsGenericType
                && !type.IsAbstract && type.HasDefaultConstructor();
        }

        public static Boolean HasDefaultConstructor(this Type type)
        {
            return type.IsValueType || type.GetConstructor(Type.EmptyTypes) != null;
        }

        /// <summary>
        /// Determines if the specified type is an open generic type.
        /// </summary>
        /// <param name="sourceType">The type to check.</param>
        /// <returns>Returns true if the type is an open-generic type.</returns>
        /// <example>List<> would return true.  List<string> would return false.</string></example>
        public static Boolean IsOpenGenericType(this Type sourceType)
        {
            Ensure.That(sourceType, nameof(sourceType)).IsNotNull();
            return sourceType.IsGenericType && sourceType.ContainsGenericParameters;
        }

        /// <summary>
        /// Determines if the specified type is a closed type of the specified
        /// open generic type.
        /// </summary>
        /// <param name="closedGenericType">The closed-generic type to check.</param>
        /// <param name="openGenericType">The open-generic type to test.</param>
        /// <param name="specificClosedArgTypes">Optional.  If specified, the closed type
        /// arguments must be assignable to those listed.</param>
        /// <returns>True if the type if a closed generic type of the specified open
        /// generic type. </returns>
        public static Boolean IsClosedGenericTypeOf(this Type closedGenericType,
            Type openGenericType,
            params Type[] specificClosedArgTypes)
        {
            Ensure.That(closedGenericType, nameof(closedGenericType)).IsNotNull();
            Ensure.That(openGenericType, nameof(openGenericType)).IsNotNull();

            if (!openGenericType.IsOpenGenericType())
            {
                throw new InvalidOperationException(
                    $"The type of: {openGenericType} is not an open generic type.");
            }

            if (!closedGenericType.IsGenericType) return false;

            // Test if the closed type is based on the same open type.
            var closedGenericTypeDef = closedGenericType.GetGenericTypeDefinition();
            if (closedGenericTypeDef != openGenericType)
            {
                return false;
            }

            // If no specific generic type parameters were specified, 
            // then the source type is a closed type of the generic type.
            if (specificClosedArgTypes.Length == 0)
            {
                return true;
            }

            var openGenericTypeInfo = IntrospectionExtensions.GetTypeInfo(openGenericType);

            if (openGenericTypeInfo.GenericTypeParameters.Length != specificClosedArgTypes.Length)
            {
                throw new InvalidOperationException(
                    "The number of generic arguments of the open-generic type does not match the " +
                    "number of specified closed-parameter types.");
            }

            var closedTypeArgTypes = closedGenericType.GetGenericArguments();
            for (Int32 i = 0; i < specificClosedArgTypes.Length; i++)
            {
                if (!specificClosedArgTypes[i].IsAssignableFrom(closedTypeArgTypes[i]))
                {
                    return false;
                }
            }

            return true;
        }

        public static Boolean IsPrimitiveExtendedIncludingNullable(this Type type)
        {
            if (IsPrimitiveExtended(type))
            {
                return true;
            }

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                return IsPrimitiveExtended(type.GenericTypeArguments[0]);
            }

            return false;
        }

        private static Boolean IsPrimitiveExtended(Type type)
        {
            if (type.IsPrimitive)
            {
                return true;
            }

            return type == typeof(String) ||
                   type == typeof(Decimal) ||
                   type == typeof(DateTime) ||
                   type == typeof(DateTimeOffset) ||
                   type == typeof(TimeSpan) ||
                   type == typeof(Guid);
        }
    }
}
