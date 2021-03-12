﻿using System;
using Dcf.Wwp.DataAccess.Base;

namespace Dcf.Wwp.DataAccess.Models
{
    public class ActionNeededTask : BaseEntity
    {
        #region Properties

        public int       ActionNeededId     { get; set; }
        public int?      ActionAssigneeId   { get; set; }
        public int?      ActionItemId       { get; set; }
        public int?      ActionPriorityId   { get; set; }
        public DateTime? DueDate            { get; set; }
        public bool      IsNoDueDate        { get; set; }
        public DateTime? CompletionDate     { get; set; }
        public bool      IsNoCompletionDate { get; set; }
        public string    Details            { get; set; }
        public string    FollowUpTask       { get; set; }
        public bool      IsDeleted          { get; set; }
        public string    ModifiedBy         { get; set; }
        public DateTime? ModifiedDate       { get; set; }
        public DateTime  CreatedDate        { get; set; }

        #endregion

        #region Navigation Properties

        public virtual ActionAssignee ActionAssignee { get; set; }
        public virtual ActionItem     ActionItem     { get; set; }
        public virtual ActionNeeded   ActionNeeded   { get; set; }
        public virtual ActionPriority ActionPriority { get; set; }

        #endregion

        #region Clone

        #endregion
    }
}
