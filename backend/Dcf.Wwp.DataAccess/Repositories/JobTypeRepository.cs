using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Repositories
{
    public class JobTypeRepository : RepositoryBase<JobType>, IJobTypeRepository
    {
        public JobTypeRepository(IDbContext dbContext) : base(dbContext)
        {
        }
    }
}
