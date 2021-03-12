using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Repositories
{
    public class WorkerTaskListRepository : RepositoryBase<WorkerTaskList>, IWorkerTaskListRepository
    {
        public WorkerTaskListRepository(IDbContext dbContext) : base(dbContext)
        {
        }
    }
}
