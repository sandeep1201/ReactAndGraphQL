using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Repositories
{
    public class AfterPullDownWeeklyBatchDetailsRepository : RepositoryBase<AfterPullDownWeeklyBatchDetails>, IAfterPullDownWeeklyBatchDetailsRepository
    {
        public AfterPullDownWeeklyBatchDetailsRepository(IDbContext dbContext) : base(dbContext)
        {
        }
    }
}
