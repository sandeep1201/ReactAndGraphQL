using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Repositories
{
    public class JobReadinessRepository : RepositoryBase<JobReadiness>, IJobReadinessRepository
    {
        public JobReadinessRepository(IDbContext dbContext) : base(dbContext)
        {
        }
    }    
}
