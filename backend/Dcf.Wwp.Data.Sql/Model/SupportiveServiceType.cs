using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class SupportiveServiceType
    {
        #region Properties

        public string   Name         { get; set; }
        public int      SortOrder    { get; set; }
        public bool     IsDeleted    { get; set; }
        public string   ModifiedBy   { get; set; }
        public DateTime ModifiedDate { get; set; }

        #endregion

        #region Methods

        public virtual ICollection<SupportiveService> SupportiveServices { get; set; }

        #endregion
    }
}
