using System.Collections.Generic;
using Dcf.Wwp.Api.Library.Contracts;

namespace Dcf.Wwp.Api.Library.Interfaces
{
    public interface IWorkerTaskListDomain
    {
        #region Properties

        #endregion

        #region Methods

        List<WorkerTaskListContract> GetWorkerTaskLists(string                   wiuid);
        WorkerTaskListContract       UpsertWorkerTaskList(WorkerTaskListContract workerTaskListContract, bool isSystemGenerated = false, bool canCommit = true);
        void                         ReassignWorker(int                          id,                     int  workerId);

        #endregion
    }
}
