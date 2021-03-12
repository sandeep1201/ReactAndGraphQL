using System;

namespace DCF.Common.Extensions
{
    public static class StringExtensions
    {
        public static Boolean IsNullOrEmpty(this String value)
        {
            return String.IsNullOrEmpty(value);
        }

        public static Boolean IsNullOrWhiteSpace(this String value)
        {
            return String.IsNullOrWhiteSpace(value);
        }

        public static string ToMaskOrNotToMask(this String value, bool doesWorkerHasRight)
        {
            return doesWorkerHasRight ? value : null;
        }
    }
}
