using System;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class CDOTracking : BaseEntity
    {
        #region Properties

        public string    WUID           { get; set; }
        public DateTime? StartTimestamp { get; set; }
        public DateTime? EndTimestamp   { get; set; }
        public DateTime? CDODate        { get; set; }
        public bool      IsDeleted      { get; set; }
        public string    ModifiedBy     { get; set; }
        public DateTime? ModifiedDate   { get; set; }

        #endregion

        #region Navigation Properties

        #endregion
    }
}
