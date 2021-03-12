using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class ConvictionType
    {
        #region Properties

        public string    Name         { get; set; }
        public int?      SortOrder    { get; set; }
        public string    ModifiedBy   { get; set; }
        public DateTime? ModifiedDate { get; set; }

        #endregion

        #region Navigation Properties

        public virtual ICollection<PendingCharge> PendingCharges { get; set; }
        public virtual ICollection<Conviction> Convictions { get; set; }

        #endregion
    }
}
