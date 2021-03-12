using System;
using Dcf.Wwp.DataAccess.Base;

namespace Dcf.Wwp.DataAccess.Models
{
    public class WageHourHistory : BaseEntity
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

        public virtual WageHour WageHour { get; set; }

        #endregion
    }
}
