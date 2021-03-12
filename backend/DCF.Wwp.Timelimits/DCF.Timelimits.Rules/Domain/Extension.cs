using System;
using DCF.Common.Dates;
using DCF.Common.Extensions;
using DCF.Core.Configuration.Startup;
using DCF.Core.Domain.Uow;
using EnumsNET;

namespace DCF.Timelimits.Rules.Domain
{
    public class Extension
    {
        private ClockTypes _clockType;
        public ExtensionDecision ExtensionDecision;

        public DateTimeRange? DateRange { get; set; }
        public DateTime DecisionDate { get; set; }

        public ClockTypes ClockType
        {
            get { return this._clockType; }
            set
            {
                value = ClockTypes.ExtensableTypes.CommonFlags(value);
                if (value.IsValid())
                {
                    if (value.Equals(ClockTypes.TNP) || value.Equals(ClockTypes.TMP))
                    {
                        value = ClockTypes.TEMP;
                    }

                    this._clockType = value;
                }
            }
        }

        public Extension(ClockTypes timeLimitTypeId, DateTime decisionDate, DateTime? beginMonth = null, DateTime? endMonth = null)
        {
            this.ClockType = timeLimitTypeId;
            this.DecisionDate = decisionDate;
            if (beginMonth.HasValue && endMonth.HasValue)
            {
                this.DateRange = new DateTimeRange(beginMonth.Value, endMonth.Value);
            }
        }

        public Boolean HasStarted => !this.DateRange.HasValue || (this.DateRange.Value.Start.IsSameOrBefore(ApplicationContext.Current.Date, DateTimeUnit.Month));

        public Boolean HasElapsed => this.DateRange.HasValue && this.DateRange.Value.End.IsBefore(ApplicationContext.Current.Date, DateTimeUnit.Month);
    }
}