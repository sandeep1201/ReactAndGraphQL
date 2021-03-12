using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class WageHourHistory
    {
        #region Properties

        public int?      WageHourId            { get; set; }
        public decimal?  HourlySubsidyRate     { get; set; }
        public DateTime? EffectiveDate         { get; set; }
        public string    PayTypeDetails        { get; set; }
        public decimal?  AverageWeeklyHours    { get; set; }
        public decimal?  PayRate               { get; set; }
        public int?      PayRateIntervalId     { get; set; }
        public string    ComputedWageRateUnit  { get; set; }
        public decimal?  ComputedWageRateValue { get; set; }
        public decimal?  WorkSiteContribution  { get; set; }
        public int?      SortOrder             { get; set; }
        public bool      IsDeleted             { get; set; }
        public string    ModifiedBy            { get; set; }
        public DateTime? ModifiedDate          { get; set; }

        #endregion

        #region Navigation Properties

        public virtual IntervalType                               IntervalType                   { get; set; }
        public virtual WageHour                                   WageHour                       { get; set; }
        public virtual ICollection<WageHourHistoryWageTypeBridge> WageHourHistoryWageTypeBridges { get; set; } = new List<WageHourHistoryWageTypeBridge>();

        #endregion
    }
}
