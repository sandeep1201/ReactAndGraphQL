using System;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class WorkerTaskList
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
        public string    ModifiedBy         { get; set; }
        public DateTime  ModifiedDate       { get; set; }

        #endregion

        #region Navigation Properties

        public virtual Participant      Participant      { get; set; }
        public virtual Worker           Worker           { get; set; }
        public virtual WorkerTaskStatus WorkerTaskStatus { get; set; }

        #endregion
    }
}
