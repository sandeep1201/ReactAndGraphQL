using System;

namespace Dcf.Wwp.Api.Library.Extensions
{
    public static class IntExtensions
    {
        public static bool IsBetween(this int input, int startCheck, int endCheck)
        {
            return input >= startCheck && input <= endCheck;
        }
    }
}
