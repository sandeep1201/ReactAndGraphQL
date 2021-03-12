using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Repositories
{
    public class POPClaimStatusRepository : RepositoryBase<POPClaimStatus>, IPOPClaimStatusRepository
    {
        public POPClaimStatusRepository(IDbContext dbContext) : base(dbContext)
        {
        }
    }
}
