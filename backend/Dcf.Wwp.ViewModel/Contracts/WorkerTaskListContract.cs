using System;

namespace Dcf.Wwp.Api.Library.Contracts
{
    public class WorkerTaskListContract
    {
        public int       Id                       { get; set; }
        public int       ParticipantId            { get; set; }
        public string    ParticipantFirstName     { get; set; }
        public string    ParticipantMiddleInitial { get; set; }
        public string    ParticipantLastName      { get; set; }
        public string    ParticipantSuffixName    { get; set; }
        public decimal?  Pin                      { get; set; }
        public int       WorkerId                 { get; set; }
        public int       CategoryId               { get; set; }
        public string    CategoryCode             { get; set; }
        public string    CategoryName             { get; set; }
        public int?      ActionPriorityId         { get; set; }
        public string    ActionPriorityName       { get; set; }
        public int?      WorkerTaskStatusId       { get; set; }
        public string    WorkerTaskStatusCode     { get; set; }
        public string    WorkerTaskStatusName     { get; set; }
        public string    TaskDate                 { get; set; }
        public string    TaskDetails              { get; set; }
        public string    StatusDate               { get; set; }
        public DateTime? DueDate                  { get; set; }
        public bool?     IsSystemGenerated        { get; set; }
        public bool      IsDeleted                { get; set; }
        public string    ModifiedBy               { get; set; }
        public DateTime  ModifiedDate             { get; set; }
    }
}
