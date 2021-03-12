using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Repositories
{
    public class POPClaimHighWageRepository : RepositoryBase<POPClaimHighWage>, IPOPClaimHighWageRepository
    {
        public POPClaimHighWageRepository(IDbContext dbContext) : base(dbContext)
        {
        }
    }
}
