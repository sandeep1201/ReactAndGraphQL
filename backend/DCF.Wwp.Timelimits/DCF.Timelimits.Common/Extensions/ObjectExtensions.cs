using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using EnsureThat;
using FastMember;
using Newtonsoft.Json;

namespace DCF.Core
{
    public static class ObjectExtensions
    {
        public static String ToJson(this Object val)
        {
            return JsonConvert.SerializeObject(val, Formatting.Indented);
        }

        public static IDictionary<String, Object> ToDictionary(this Object val)
        {
            var dictionary = new Dictionary<String, Object>();
            foreach (PropertyDescriptor propertyDescriptor in TypeDescriptor.GetProperties(val))
            {
                var obj = propertyDescriptor.GetValue(val);
                dictionary.Add(propertyDescriptor.Name, obj);
            }
            return dictionary;
        }

        public static Boolean IsFunc(this Object obj)
        {
            if (obj == null)
            {
                return false;
            }

            var type = obj.GetType();
            if (!type.IsGenericType)
            {
                return false;
            }

            return type.GetGenericTypeDefinition() == typeof(Func<>);
        }

        public static Boolean IsFunc<TReturn>(this Object val)
        {
            return val != null && val.GetType() == typeof(Func<TReturn>);
        }


        // TODO: Add some unit/perf tests. Probably replace with Automapper
        /// <summary>
        /// Merge source object properties into the target object.
        /// Ignore unknown properties or properties set to NULL
        /// or default value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target"></param>
        /// <param name="source"></param>
        public static void MergeObjects<T>(this T target, Object source)
        {
            if (target == null)
            {
                return;
            }

            Ensure.That(source,name: nameof(source)).IsNotNull();

            var targetTypeAccessor = TypeAccessor.Create(typeof(T));
            var sourceTargetAccessor = TypeAccessor.Create(source.GetType());

            var targetMembers = targetTypeAccessor.GetMembers();
            var sourceMembers = targetTypeAccessor.GetMembers();

            foreach (var member in targetMembers)
            {
                var sourceMemeber = sourceMembers.SingleOrDefault(c => c.Name == member.Name && member.Type.IsAssignableFrom(c.Type) );
                // if the source doesn't have the property or it has the default value
                // we'll ignore it
                if (sourceMemeber == null || sourceTargetAccessor[source,sourceMemeber.Name].IsDefaultValue())
                {
                    continue;
                }

                //set the value on the target
                targetTypeAccessor[target, member.Name] = sourceTargetAccessor[source, sourceMemeber.Name];
            }

        }

        public static Boolean IsDefaultValue(this Object objectValue)
        {
            // If no ObjectValue was supplied, return true, null should always be the default
            if (objectValue == null)
            {
                return true;
            }

            // Determine ObjectType from ObjectValue
            var objectType = objectValue.GetType();

            // Get the default value of type ObjectType
            Object Default = objectType.GetDefaultValue();

            // If a non-null ObjectValue was supplied, compare Value with its default value and return the result
            return objectValue.Equals(Default);

        }

        /// <summary>
        /// Used to simplify and beautify casting an object to a type. 
        /// </summary>
        /// <typeparam name="T">Type to be casted</typeparam>
        /// <param name="obj">Object to cast</param>
        /// <returns>Casted object</returns>
        public static T As<T>(this Object obj)
            where T : class
        {
            return obj as T;
        }

        /// <summary>
        /// Converts given object to a value type using <see cref="Convert.ChangeType(object,System.TypeCode)"/> method.
        /// </summary>
        /// <param name="obj">Object to be converted</param>
        /// <typeparam name="T">Type of the target object</typeparam>
        /// <returns>Converted object</returns>
        public static T To<T>(this Object obj)
            where T : struct
        {
            return (T)Convert.ChangeType(obj, typeof(T), CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Check if an item is in a list.
        /// </summary>
        /// <param name="item">Item to check</param>
        /// <param name="list">List of items</param>
        /// <typeparam name="T">Type of the items</typeparam>
        public static Boolean IsIn<T>(this T item, params T[] list)
        {
            return list.Contains(item);
        }


        private static void CopyProps<T>(T target, Object source)
        {
            Type t = target.GetType();
            var properties = t.GetProperties().Where(prop => prop.CanRead && prop.CanWrite);

            foreach (var prop in properties)
            {
                var value = prop.GetValue(source, null);
                if (value != null)
                    prop.SetValue(target, value, null);
            }
        }


    }
}
