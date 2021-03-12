using System;
using System.Collections.Generic;

namespace DCF.Timelimits.Rules.Domain
{
    internal class EpisodeValidationException : Exception
    {
        public EpisodeValidationException()
            : this(null, (Exception)null, null)
        {

        }

        public EpisodeValidationException(String message)
            : this(message, (Exception)null, null)

        {
        }

        public EpisodeValidationException(String message, params TimelineMonth[] ticks)
            : this(message, (Exception)null, ticks)

        {
        }

        public EpisodeValidationException(String message, Exception innerException, params TimelineMonth[] ticks)
            : base(message, innerException)
        {
            this.Ticks = new List<TimelineMonth>();
            if (ticks != null)
                this.Ticks.AddRange(ticks);
        }

        public List<TimelineMonth> Ticks { get; }
    }
}