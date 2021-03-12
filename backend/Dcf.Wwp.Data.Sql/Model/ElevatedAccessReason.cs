using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class ElevatedAccessReason
    {
        #region Properties

        public string    Reason       { get; set; }
        public int?      SortOrder    { get; set; }
        public bool      IsDeleted    { get; set; }
        public string    ModifiedBy   { get; set; }
        public DateTime? ModifiedDate { get; set; }

        #endregion

        #region Navigation Properties

        public virtual ICollection<ElevatedAccess> ElevatedAccesses { get; set; }

        #endregion
    }
}
