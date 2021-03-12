using System;

namespace Dcf.Wwp.Model.Interface
{
    public interface IWorkerTaskList : ICommonModelFinal
    {
        int               WorkerId           { get; set; }
        int               CategoryId         { get; set; }
        int               ParticipantId      { get; set; }
        int?              ActionPriorityId   { get; set; }
        int?              WorkerTaskStatusId { get; set; }
        DateTime          TaskDate           { get; set; }
        string            TaskDetails        { get; set; }
        DateTime?         StatusDate         { get; set; }
        DateTime?         DueDate            { get; set; }
        bool?             IsSystemGenerated  { get; set; }
        IParticipant      Participant        { get; set; }
        IWorker           Worker             { get; set; }
        IWorkerTaskStatus WorkerTaskStatus   { get; set; }
    }
}
