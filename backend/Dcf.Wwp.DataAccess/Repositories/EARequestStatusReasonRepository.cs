using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Repositories
{
    public class EARequestStatusReasonRepository : RepositoryBase<EARequestStatusReason>, IEARequestStatusReasonRepository
    {
        public EARequestStatusReasonRepository(IDbContext dbContext) : base(dbContext)
        {
        }
    }
}
