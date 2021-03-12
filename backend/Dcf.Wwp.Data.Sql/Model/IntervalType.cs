using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class IntervalType
    {
        #region Properties

        public string    Name         { get; set; }
        public int?      SortOrder    { get; set; }
        public bool      IsDeleted    { get; set; }
        public string    ModifiedBy   { get; set; }
        public DateTime? ModifiedDate { get; set; }

        #endregion

        #region Navigation Properties

        public virtual ICollection<WageHour>         BeginRateIntervalTypes      { get; set; }
        public virtual ICollection<WageHour>         CurrentPayRateIntervalTypes { get; set; }
        public virtual ICollection<WageHour>         EndRateIntervalTypes        { get; set; }
        public virtual ICollection<WageHourHistory>  WageHourHistories           { get; set; }
        [JsonIgnore]
        public virtual ICollection<FormalAssessment> FormalAssessments           { get; set; }

        #endregion
    }
}
