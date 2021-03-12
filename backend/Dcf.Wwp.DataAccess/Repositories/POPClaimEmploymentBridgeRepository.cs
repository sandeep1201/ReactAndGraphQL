using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Repositories
{
    public class POPClaimEmploymentBridgeRepository : RepositoryBase<POPClaimEmploymentBridge>, IPOPClaimEmploymentBridgeRepository
    {
        public POPClaimEmploymentBridgeRepository(IDbContext dbContext) : base(dbContext)
        {
        }
    }
}
