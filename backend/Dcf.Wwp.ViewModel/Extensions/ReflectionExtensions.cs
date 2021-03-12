using System.Linq;

namespace Dcf.Wwp.Api.Library.Extensions
{
    public static class ReflectionExtensions
    {
        /// <summary>
        /// Gets the property value from an object.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static object GetPropertyValue(this object obj, string propertyName)
        {
            return obj.GetType().GetProperties()
               .Single(pi => pi.Name == propertyName)
               .GetValue(obj, null);
        }
    }
}