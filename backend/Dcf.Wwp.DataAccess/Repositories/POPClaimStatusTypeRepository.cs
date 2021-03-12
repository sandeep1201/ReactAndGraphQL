using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Repositories
{
    public class POPClaimStatusTypeRepository : RepositoryBase<POPClaimStatusType>, IPOPClaimStatusTypeRepository
    {
        public POPClaimStatusTypeRepository(IDbContext dbContext) : base(dbContext)
        {
        }
    }
}
