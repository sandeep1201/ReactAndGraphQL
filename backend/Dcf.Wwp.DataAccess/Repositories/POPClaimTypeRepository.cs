using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Repositories
{
    public class POPClaimTypeRepository : RepositoryBase<POPClaimType>, IPOPClaimTypeRepository
    {
        public POPClaimTypeRepository(IDbContext dbContext) : base(dbContext)
        {
        }
    }
}
