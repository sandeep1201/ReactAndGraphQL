using System;
using DCF.Common.Dates;
using DCF.Common.Extensions;

namespace Dcf.Wwp.Data.Sql.Model
{
    public class AlienStatus
    {
        public String AlienStatusCode { get; set; }
        public String AlienStatusCodeDescriptionText { get; set; }

        public DateTimeRange DateRange { get; set; } = DateTimeRange.Empty;

        public AlienStatus(DateTime effectiveBeginMonth, DateTime? effectiveEndMonth)
        {
            var start = effectiveBeginMonth.StartOf(DateTimeUnit.Month);
            var end = effectiveEndMonth.GetValueOrDefault(DateTime.MaxValue).EndOf(DateTimeUnit.Month);
            this.DateRange = new DateTimeRange(start,end);
        }
    }
}