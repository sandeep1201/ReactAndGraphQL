using System;

namespace Dcf.Wwp.Api.Library.Extensions
{
    public static class DecimalExtensions
    {
        public static decimal? ToDecimalNull(this string s)
        {
            if (string.IsNullOrWhiteSpace(s))
                return null;

            return Convert.ToDecimal(s);
        }
    }
}
