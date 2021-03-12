using System;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class ActionNeededPageActionItemBridge
    {
        #region Properties

        public int       ActionNeededPageId { get; set; }
        public int       ActionItemId       { get; set; }
        public int       SortOrder          { get; set; }
        public bool      IsDeleted          { get; set; }
        public string    ModifiedBy         { get; set; }
        public DateTime? ModifiedDate       { get; set; }

        #endregion

        #region Navigation Properties

        public virtual ActionItem       ActionItem       { get; set; }
        public virtual ActionNeededPage ActionNeededPage { get; set; }

        #endregion
    }
}
