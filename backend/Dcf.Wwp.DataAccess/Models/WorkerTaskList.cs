using System;
using Dcf.Wwp.DataAccess.Base;

namespace Dcf.Wwp.DataAccess.Models
{
    public class WorkerTaskList : BaseEntity
    {
        #region Properties

        public int       WorkerId           { get; set; }
        public int       CategoryId         { get; set; }
        public int       ParticipantId      { get; set; }
        public int?      ActionPriorityId   { get; set; }
        public int?      WorkerTaskStatusId { get; set; }
        public DateTime  TaskDate           { get; set; }
        public string    TaskDetails        { get; set; }
        public DateTime? StatusDate         { get; set; }
        public DateTime? DueDate            { get; set; }
        public bool?     IsSystemGenerated  { get; set; }
        public bool      IsDeleted          { get; set; }
        public string    ModifiedBy         { get; set; }
        public DateTime  ModifiedDate       { get; set; }

        #endregion

        #region Navigation Properties

        public virtual Participant        Participant        { get; set; }
        public virtual Worker             Worker             { get; set; }
        public virtual WorkerTaskCategory WorkerTaskCategory { get; set; }
        public virtual ActionPriority     ActionPriority     { get; set; }
        public virtual WorkerTaskStatus   WorkerTaskStatus   { get; set; }

        #endregion

        #region Clone

        public WorkerTaskList Clone()
        {
            var wtl = new WorkerTaskList
                      {
                          WorkerId           = WorkerId,
                          CategoryId         = CategoryId,
                          ParticipantId      = ParticipantId,
                          ActionPriorityId   = ActionPriorityId,
                          WorkerTaskStatusId = WorkerTaskStatusId,
                          TaskDate           = TaskDate,
                          TaskDetails        = TaskDetails,
                          StatusDate         = StatusDate,
                          DueDate            = DueDate,
                          IsSystemGenerated  = IsSystemGenerated,
                          IsDeleted          = IsDeleted,
                          ModifiedBy         = ModifiedBy,
                          ModifiedDate       = ModifiedDate
                      };

            return wtl;
        }

        #endregion
    }
}
