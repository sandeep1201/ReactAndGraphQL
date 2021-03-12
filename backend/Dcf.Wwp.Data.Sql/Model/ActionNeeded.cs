using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class ActionNeeded
    {
        #region Properties

        public int       ParticipantId      { get; set; }
        public int       ActionNeededPageId { get; set; }
        public bool      IsNoActionNeeded   { get; set; }
        public DateTime  CreatedDate        { get; set; }
        public bool      IsDeleted          { get; set; }
        public string    ModifiedBy         { get; set; }
        public DateTime? ModifiedDate       { get; set; }

        #endregion

        #region Navigation Properties

        public virtual Participant                   Participant       { get; set; }
        public virtual ActionNeededPage              ActionNeededPage  { get; set; }
        public virtual ICollection<ActionNeededTask> ActionNeededTasks { get; set; } = new List<ActionNeededTask>();

        #endregion
    }
}
