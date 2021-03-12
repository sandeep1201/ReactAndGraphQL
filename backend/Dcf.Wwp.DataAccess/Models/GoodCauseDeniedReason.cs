using System;
using Dcf.Wwp.DataAccess.Base;

namespace Dcf.Wwp.DataAccess.Models
{
    public class GoodCauseDeniedReason : BaseEntity
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

        #endregion

        #region Clone

        public GoodCauseDeniedReason Clone()
        {
            var a = new GoodCauseDeniedReason
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
