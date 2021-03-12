using System;
using Dcf.Wwp.DataAccess.Base;

namespace Dcf.Wwp.DataAccess.Models
{
    public class POPClaimHighWage : BaseEntity
    {
        #region Properties

        public int       OrganizationId  { get; set; }
        public decimal   StartingWage    { get; set; }
        public int       SortOrder       { get; set; }
        public bool      IsSystemUseOnly { get; set; }
        public DateTime  EffectiveDate   { get; set; }
        public DateTime? EndDate         { get; set; }
        public bool      IsDeleted       { get; set; }
        public string    ModifiedBy      { get; set; }
        public DateTime  ModifiedDate    { get; set; }

        #endregion

        #region Navigation Properties

        public virtual Organization Organization { get; set; }

        #endregion

        #region Clone

        public POPClaimHighWage Clone()
        {
            var a = new POPClaimHighWage
                    {
                        Id              = Id,
                        IsDeleted       = IsDeleted,
                        ModifiedBy      = ModifiedBy,
                        ModifiedDate    = ModifiedDate,
                        RowVersion      = RowVersion,
                        OrganizationId  = OrganizationId,
                        StartingWage    = StartingWage,
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
