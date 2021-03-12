using System;
using System.Collections.Generic;
using Dcf.Wwp.DataAccess.Base;

namespace Dcf.Wwp.DataAccess.Models
{
    public class WageHour : BaseEntity
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
        public int?      SortOrder                       { get; set; }
        public bool      IsDeleted                       { get; set; }
        public string    ComputedCurrentWageRateUnit     { get; set; }
        public decimal?  ComputedCurrentWageRateValue    { get; set; }
        public string    ComputedPastEndWageRateUnit     { get; set; }
        public decimal?  ComputedPastEndWageRateValue    { get; set; }
        public decimal?  WorkSiteContribution            { get; set; }

        #endregion

        #region Navigation Properties

        public virtual ICollection<EmploymentInformation> EmploymentInformations { get; set; } = new List<EmploymentInformation>();
        public virtual ICollection<WageHourHistory>       WageHourHistories      { get; set; } = new List<WageHourHistory>();

        #endregion
    }
}
