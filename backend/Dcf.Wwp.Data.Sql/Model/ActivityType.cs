using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class ActivityType
    {
        #region Properties

        public string    Code          { get; set; }
        public string    Name          { get; set; }
        public int       SortOrder     { get; set; }
        public bool      IsDeleted     { get; set; }
        public string    ModifiedBy    { get; set; }
        public DateTime  ModifiedDate  { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public DateTime? EndDate       { get; set; }

        #endregion

        #region Navigation Properties

        public virtual ICollection<Activity> Activities { get; set; }
        public virtual ICollection<EnrolledProgramEPActivityTypeBridge> EnrolledProgramEPActivityTypeBridges { get; set; }

        #endregion
    }
}
