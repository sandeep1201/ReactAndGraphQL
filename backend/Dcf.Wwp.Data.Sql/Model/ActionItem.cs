using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class ActionItem
    {
        #region Properties

        public string    Name         { get; set; }
        public bool      IsDeleted    { get; set; }
        public string    ModifiedBy   { get; set; }
        public DateTime? ModifiedDate { get; set; }

        #endregion

        #region Navigation Properties

        public virtual ICollection<ActionNeededPageActionItemBridge> ActionNeededPageActionItemBridges { get; set; }
        public virtual ICollection<ActionNeededTask>                 ActionNeededTasks                 { get; set; }

        #endregion
    }
}
