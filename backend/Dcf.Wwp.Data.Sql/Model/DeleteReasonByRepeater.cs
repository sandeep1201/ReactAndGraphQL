using System;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class DeleteReasonByRepeater
    {
        #region Properties

        public string    Repeater       { get; set; }
        public int       DeleteReasonId { get; set; }
        public int       SortOrder      { set; get; }
        public bool      IsDeleted      { get; set; }
        public string    ModifiedBy     { get; set; }
        public DateTime? ModifiedDate   { get; set; }

        #endregion

        #region Navigation Properties

        public virtual DeleteReason DeleteReason { get; set; }

        #endregion
    }
}
