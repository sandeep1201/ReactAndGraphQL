using System;
using System.Collections.Generic;
using Dcf.Wwp.DataAccess.Base;

namespace Dcf.Wwp.DataAccess.Models
{
    public class ActionNeeded : BaseEntity
    {
        #region Properties

        public int       ParticipantId      { get; set; }
        public int       ActionNeededPageId { get; set; }
        public bool      IsNoActionNeeded   { get; set; }
        public bool      IsDeleted          { get; set; }
        public string    ModifiedBy         { get; set; }
        public DateTime? ModifiedDate       { get; set; }

        #endregion

        #region Navigation Properties

        public virtual ActionNeededPage              ActionNeededPage  { get; set; }
        public virtual ICollection<ActionNeededTask> ActionNeededTasks { get; set; } = new List<ActionNeededTask>();

        #endregion

        #region Clone

        #endregion
    }
}
