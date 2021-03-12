using System.Collections.Generic;
using Dcf.Wwp.Api.Library.ViewModels;

namespace Dcf.Wwp.Api.Library.Contracts.Timelimits
{
    public class TimelineContract
    {
        public List<TimelineMonthContract> TimelineMonths { get; } = new List<TimelineMonthContract>();
        public List<ExtensionSequenceContract> ExtensionSequences { get; } = new List<ExtensionSequenceContract>();
    }
}