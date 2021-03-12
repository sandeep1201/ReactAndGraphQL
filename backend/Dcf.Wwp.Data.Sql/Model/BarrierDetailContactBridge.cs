using System;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class BarrierDetailContactBridge
    {
        #region Properties

        public int?      BarrierDetailId { get; set; }
        public int?      ContactId       { get; set; }
        public bool      IsDeleted       { get; set; }
        public string    ModifiedBy      { get; set; }
        public DateTime? ModifiedDate    { get; set; }

        #endregion

        #region Navigation Properties

        public virtual BarrierDetail BarrierDetail { get; set; }
        public virtual Contact       Contact       { get; set; }

        #endregion
    }
}
