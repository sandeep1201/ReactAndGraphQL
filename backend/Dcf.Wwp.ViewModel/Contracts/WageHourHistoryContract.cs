using System;

namespace Dcf.Wwp.Api.Library.Contracts
{
    public class WageHourHistoryContract
    {
        public int                   Id                    { get; set; }
        public string                HourlySubsidyRate     { get; set; }
        public string                EffectiveDate         { get; set; }
        public string                PayTypeDetails        { get; set; }
        public string                AverageWeeklyHours    { get; set; }
        public string                PayRate               { get; set; }
        public int?                  PayRateIntervalId     { get; set; }
        public string                PayRateIntervalName   { get; set; }
        public string                ComputedWageRateUnit  { get; set; }
        public string                ComputedWageRateValue { get; set; }
        public int                   SortOrder             { get; set; }
        public string                ModifiedBy            { get; set; }
        public DateTime?             ModifiedDate          { get; set; }
        public bool                  IsDeletedFromCurrent  { get; set; }
        public string                WorkSiteContribution  { get; set; }
        public JobActionTypeContract HistoryPayType        { get; set; }
    }
}
