using Dcf.Wwp.Data.Sql.Model;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Repository;

namespace Dcf.Wwp.Model.Repository
{
    public partial class Repository : IWorkerTaskListRepository
    {
        public void NewWorkerTask(IWorkerTaskList workerTaskList)
        {
            _db.WorkerTaskLists.Add((WorkerTaskList)workerTaskList);
        }
    }
}
