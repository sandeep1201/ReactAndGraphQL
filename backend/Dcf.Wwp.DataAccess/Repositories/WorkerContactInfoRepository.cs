using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Repositories
{
    public class WorkerContactInfoRepository : RepositoryBase<WorkerContactInfo>, IWorkerContactInfoRepository
    {
        public WorkerContactInfoRepository(IDbContext dbContext) : base(dbContext)
        {
        }
    }
}
