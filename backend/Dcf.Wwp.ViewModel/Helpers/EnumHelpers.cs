using System;
using System.ComponentModel;
using System.Reflection;

namespace Dcf.Wwp.Api.Library.Helpers
{
    public static class EnumHelpers
    {
        /// <summary>
        /// function to get the description of the Status by passing the Enum value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetEnumDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            var attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(
                    typeof(DescriptionAttribute),
                    false);

            if (attributes.Length <= 0) return value.ToString();
            return attributes[0].Description;
        }
    }
}