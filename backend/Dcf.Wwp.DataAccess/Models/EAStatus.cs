using System;
using System.Collections.Generic;
using Dcf.Wwp.DataAccess.Base;

namespace Dcf.Wwp.DataAccess.Models
{
    public class EAStatus : BaseEntity
    {
        #region Properties

        public string    Code            { get; set; }
        public string    Name            { get; set; }
        public int       SortOrder       { get; set; }
        public bool      IsSystemUseOnly { get; set; }
        public DateTime  EffectiveDate   { get; set; }
        public DateTime? EndDate         { get; set; }
        public bool      IsDeleted       { get; set; }
        public string    ModifiedBy      { get; set; }
        public DateTime  ModifiedDate    { get; set; }

        #endregion

        #region Navigation Properties

        public virtual List<EAStatusReason> EaStatusReasons { get; set; } = new List<EAStatusReason>();

        #endregion

        #region Clone

        public EAStatus Clone()
        {
            var a = new EAStatus
                    {
                        Id              = Id,
                        IsDeleted       = IsDeleted,
                        ModifiedBy      = ModifiedBy,
                        ModifiedDate    = ModifiedDate,
                        RowVersion      = RowVersion,
                        Code            = Code,
                        Name            = Name,
                        IsSystemUseOnly = IsSystemUseOnly,
                        SortOrder       = SortOrder,
                        EffectiveDate   = EffectiveDate,
                        EndDate         = EndDate
                    };

            return a;
        }

        #endregion
    }
}
