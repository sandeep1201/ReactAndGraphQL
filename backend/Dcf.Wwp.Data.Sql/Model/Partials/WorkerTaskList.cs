using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class WorkerTaskList : BaseCommonModel, IWorkerTaskList
    {
        IParticipant IWorkerTaskList.Participant
        {
            get => Participant;
            set => Participant = (Participant)value;
        }

        IWorker IWorkerTaskList.Worker
        {
            get => Worker;
            set => Worker = (Worker)value;
        }

        IWorkerTaskStatus IWorkerTaskList.WorkerTaskStatus
        {
            get => WorkerTaskStatus;
            set => WorkerTaskStatus = (WorkerTaskStatus)value;
        }
    }
}
