using System;

namespace DCF.Common.Timing
{
    public class UnspecifiedClockProvider : IClockProvider
    {
        public DateTime Now => DateTime.Now;

        public DateTimeKind Kind => DateTimeKind.Unspecified;

        public Boolean SupportsMultipleTimezone => false;

        public DateTime Normalize(DateTime dateTime)
        {
            return dateTime;
        }

        internal UnspecifiedClockProvider()
        {
            
        }
    }
}