using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class WageHour
    {
        #region Properties

        public DateTime? CurrentEffectiveDate            { get; set; }
        public string    CurrentPayTypeDetails           { get; set; }
        public decimal?  CurrentAverageWeeklyHours       { get; set; }
        public decimal?  CurrentPayRate                  { get; set; }
        public int?      CurrentPayRateIntervalId        { get; set; }
        public decimal?  CurrentHourlySubsidyRate        { get; set; }
        public decimal?  PastBeginPayRate                { get; set; }
        public int?      PastBeginPayRateIntervalId      { get; set; }
        public decimal?  PastEndPayRate                  { get; set; }
        public int?      PastEndPayRateIntervalId        { get; set; }
        public bool?     IsUnchangedPastPayRateIndicator { get; set; }
        public string    ComputedCurrentWageRateUnit     { get; set; }
        public decimal?  ComputedCurrentWageRateValue    { get; set; }
        public string    ComputedPastEndWageRateUnit     { get; set; }
        public decimal?  ComputedPastEndWageRateValue    { get; set; }
        public decimal?  WorkSiteContribution            { get; set; }
        public int?      SortOrder                       { get; set; }
        public bool      IsDeleted                       { get; set; }
        public string    ModifiedBy                      { get; set; }
        public DateTime? ModifiedDate                    { get; set; }

        #endregion

        #region Navigation Properties

        public virtual IntervalType                        BeginRateIntervalType   { get; set; }
        public virtual IntervalType                        CurrentPayIntervalType  { get; set; }
        public virtual IntervalType                        EndRateIntervalType     { get; set; }
        public virtual ICollection<EmploymentInformation>  EmploymentInformations  { get; set; } = new List<EmploymentInformation>();
        public virtual ICollection<WageHourHistory>        WageHourHistories       { get; set; } = new List<WageHourHistory>();
        public virtual ICollection<WageHourWageTypeBridge> WageHourWageTypeBridges { get; set; } = new List<WageHourWageTypeBridge>();

        #endregion
    }
}
