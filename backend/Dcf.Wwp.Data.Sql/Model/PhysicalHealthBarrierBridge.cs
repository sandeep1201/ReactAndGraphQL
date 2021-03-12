using System;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class PhysicalHealthBarrierBridge : BaseEntity
    {
        #region Properties

        public int?      PhysicalHealthId { get; set; }
        public int?      BarrierId        { get; set; }
        public int?      SortOrder        { get; set; }
        public bool      IsDeleted        { get; set; }
        public string    ModifiedBy       { get; set; }
        public DateTime? ModifiedDate     { get; set; }

        #endregion

        #region Navigation Properties

        // table is missing

        #endregion
    }
}
