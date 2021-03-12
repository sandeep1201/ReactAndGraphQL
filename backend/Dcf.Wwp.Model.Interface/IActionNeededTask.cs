using System;

namespace Dcf.Wwp.Model.Interface
{
    public interface IActionNeededTask : ICommon2Model, ICloneable
    {
        int ActionNeededId { get; set; }
        Nullable<int> ActionAssigneeId { get; set; }
        Nullable<int> ActionItemId { get; set; }
        Nullable<int> ActionPriorityId { get; set; }
        Nullable<System.DateTime> DueDate { get; set; }
        bool IsNoDueDate { get; set; }
        Nullable<System.DateTime> CompletionDate { get; set; }
        bool IsNoCompletionDate { get; set; }
        string Details { get; set; }
        string FollowUpTask { get; set; }

        IActionAssignee ActionAssignee { get; set; }
        IActionItem ActionItem { get; set; }
        IActionNeeded ActionNeeded { get; set; }
        IActionPriority ActionPriority { get; set; }
    }
}