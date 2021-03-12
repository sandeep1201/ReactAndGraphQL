using System;
using System.Collections.Generic;

namespace DCF.Timelimits.Rules.Domain
{
    public class TickTimelineValidationException : Exception
    {
        public IEnumerable<TimelineMonth> Ticks { get; }
        public TickTimelineValidationException()
            :this(null)
        {

        }
        public TickTimelineValidationException(String message, Exception innerException = null)
            : this(message, innerException, null)
        {

        }

        public TickTimelineValidationException(String message, Exception innerException=null, params TimelineMonth[] ticks)
            : base(message, innerException)
        {
            this.Ticks = new List<TimelineMonth>();

        }

        
    }
}