using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class BarrierType
    {
        #region Properties

        public string    Name         { get; set; }
        public bool?     IsRequired   { get; set; }
        public int?      SortOrder    { get; set; }
        public bool      IsDeleted    { get; set; }
        public string    ModifiedBy   { get; set; }
        public DateTime? ModifiedDate { get; set; }

        #endregion

        #region Navigation Properties

        public virtual ICollection<BarrierDetail>  BarrierDetails  { get; set; }
        public virtual ICollection<BarrierSubtype> BarrierSubtypes { get; set; }

        #endregion
    }
}
