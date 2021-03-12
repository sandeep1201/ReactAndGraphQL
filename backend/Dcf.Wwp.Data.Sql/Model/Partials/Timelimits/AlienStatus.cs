using System;
using DCF.Common.Dates;
using DCF.Common.Extensions;

namespace Dcf.Wwp.Data.Sql.Model
{
    public class AlienStatus
    {

        public Decimal PinNumber { get; set; }
        public Int32 AlienStatusCode { get; set; }
        public String AlienStatusCodeDescriptionText { get; set; }

        public DateTimeRange DateRange { get; set; } = DateTimeRange.Empty;

        public AlienStatus(Decimal pinNumber,DateTime effectiveBeginMonth, DateTime? effectiveEndMonth)
        {
            this.PinNumber = pinNumber;
            var start = effectiveBeginMonth.StartOf(DateTimeUnit.Month);
            var end = effectiveEndMonth.GetValueOrDefault(DateTime.MaxValue).EndOf(DateTimeUnit.Month);
            this.DateRange = new DateTimeRange(start,end);
        }
    }
}