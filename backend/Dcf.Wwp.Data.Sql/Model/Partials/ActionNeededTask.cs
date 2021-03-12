using System;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class ActionNeededTask : BaseCommonModel, IActionNeededTask, IEquatable<ActionNeededTask>
    {
        IActionAssignee IActionNeededTask.ActionAssignee
        {
            get => ActionAssignee;
            set => ActionAssignee = (ActionAssignee) value;
        }

        IActionItem IActionNeededTask.ActionItem
        {
            get => ActionItem;
            set => ActionItem = (ActionItem) value;
        }

        IActionNeeded IActionNeededTask.ActionNeeded
        {
            get => ActionNeeded;
            set => ActionNeeded = (ActionNeeded) value;
        }

        IActionPriority IActionNeededTask.ActionPriority
        {
            get => ActionPriority;
            set => ActionPriority = (ActionPriority) value;
        }

        #region ICloneable

        public object Clone()
        {
            var clone = new ActionNeededTask
                        {
                            Id                 = Id,
                            ActionNeededId     = ActionNeededId,
                            ActionAssigneeId   = ActionAssigneeId,
                            ActionItemId       = ActionItemId,
                            ActionPriorityId   = ActionPriorityId,
                            DueDate            = DueDate,
                            IsNoDueDate        = IsNoDueDate,
                            CompletionDate     = CompletionDate,
                            IsNoCompletionDate = IsNoCompletionDate,
                            Details            = Details,
                            IsDeleted          = IsDeleted,
                            FollowUpTask       = FollowUpTask,
                            ModifiedDate       = ModifiedDate,
                            ModifiedBy         = ModifiedBy,
                            RowVersion         = RowVersion
                        };

            return clone;
        }

        #endregion ICloneable

        #region IEquatable<T>

        public override bool Equals(object other)
        {
            if (other == null)
            {
                return false;
            }

            var obj = other as ActionNeededTask;

            return obj != null && Equals(obj);
        }

        public bool Equals(ActionNeededTask other)
        {
            // Check whether the compared object is null.
            if (ReferenceEquals(other, null))
            {
                return false;
            }

            // Check whether the compared object references the same data.
            if (ReferenceEquals(this, other))
            {
                return true;
            }

            // Check whether the products' properties are equal.
            if (!AdvEqual(Id, other.Id))
            {
                return false;
            }

            if (!AdvEqual(ActionNeededId, other.ActionNeededId))
            {
                return false;
            }

            if (!AdvEqual(ActionAssigneeId, other.ActionAssigneeId))
            {
                return false;
            }

            if (!AdvEqual(ActionItemId, other.ActionItemId))
            {
                return false;
            }

            if (!AdvEqual(ActionPriorityId, other.ActionPriorityId))
            {
                return false;
            }

            if (!AdvEqual(DueDate, other.DueDate))
            {
                return false;
            }

            if (!AdvEqual(IsNoDueDate, other.IsNoDueDate))
            {
                return false;
            }

            if (!AdvEqual(CompletionDate, other.CompletionDate))
            {
                return false;
            }

            if (!AdvEqual(IsNoCompletionDate, other.IsNoCompletionDate))
            {
                return false;
            }

            if (!AdvEqual(Details, other.Details))
            {
                return false;
            }

            if (!AdvEqual(IsDeleted, other.IsDeleted))
            {
                return false;
            }

            if (!AdvEqual(FollowUpTask, other.FollowUpTask))
            {
                return false;
            }

            return true;
        }

        #endregion IEquatable<T>
    }
}
