using System;
using System.Collections.Generic;
using Dcf.Wwp.DataAccess.Base;

namespace Dcf.Wwp.DataAccess.Models
{
    public class ActivityType : BaseEntity
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

        public virtual ICollection<EnrolledProgramEPActivityTypeBridge> EnrolledProgramEPActivityTypeBridges { get; set; } = new List<EnrolledProgramEPActivityTypeBridge>();

        #endregion

        #region Clone

        public ActivityType Clone()
        {
            var a = new ActivityType
                    {
                        Id            = Id,
                        IsDeleted     = IsDeleted,
                        ModifiedBy    = ModifiedBy,
                        ModifiedDate  = ModifiedDate,
                        RowVersion    = RowVersion,
                        Code          = Code,
                        Name          = Name,
                        SortOrder     = SortOrder,
                        EffectiveDate = EffectiveDate,
                        EndDate       = EndDate
                    };

            return a;
        }

        #endregion
    }
}
