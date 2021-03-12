using System;

namespace Dcf.Wwp.Api.Library.Contracts.ActionNeeded
{
    public class ActionNeededTaskContract
    {
        public int Id { get; set; }
        public int ActionNeededId { get; set; }
        public int? AssigneeId { get; set; }
        public string AssigneeName { get; set; }
        public int PageId { get; set; }
        public string PageName { get; set; }
        public int? ActionItemId { get; set; }
        public string ActionItemName { get; set; }
        public int? PriorityId { get; set; }
        public string PriorityName { get; set; }
        public string FollowUpTask { get; set; }
        public DateTime? DueDate { get; set; }
        public bool IsNoDueDate { get; set; }
        public DateTime? CompletionDate { get; set; }
        public bool IsNoCompletionDate { get; set; }
        public string Details { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public byte[] RowVersion { get; set; }
        public string ModifiedByName { get; set; }
    }
}