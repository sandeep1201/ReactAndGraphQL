using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Repositories
{
    public class EAStatusReasonRepository : RepositoryBase<EAStatusReason>, IEAStatusReasonRepository
    {
        public EAStatusReasonRepository(IDbContext dbContext) : base(dbContext)
        {
        }
    }
}
