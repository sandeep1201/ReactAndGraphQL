using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Repositories
{
    public class WorkerTaskStatusRepository : RepositoryBase<WorkerTaskStatus>, IWorkerTaskStatusRepository
    {
        public WorkerTaskStatusRepository(IDbContext dbContext) : base(dbContext)
        {
        }
    }
}
