using System;
using Dcf.Wwp.DataAccess.Base;

namespace Dcf.Wwp.DataAccess.Models
{
    public class ActivityLocation : BaseEntity
    {
        #region Properties

        public string   Name         { get; set; }
        public int      SortOrder    { get; set; }
        public bool     IsDeleted    { get; set; }
        public string   ModifiedBy   { get; set; }
        public DateTime ModifiedDate { get; set; }

        #endregion

        #region Navigation Properties

        #endregion

        #region Clone

        public ActivityLocation Clone()
        {
            var a = new ActivityLocation
                    {
                        Id           = Id,
                        IsDeleted    = IsDeleted,
                        ModifiedBy   = ModifiedBy,
                        ModifiedDate = ModifiedDate,
                        RowVersion   = RowVersion,
                        Name         = Name,
                        SortOrder    = SortOrder
                    };

            return a;
        }

        #endregion
    }
}
