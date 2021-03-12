using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Repositories
{
    public class POPClaimRepository : RepositoryBase<POPClaim>, IPOPClaimRepository
    {
        public POPClaimRepository(IDbContext dbContext) : base(dbContext)
        {
        }
    }
}
