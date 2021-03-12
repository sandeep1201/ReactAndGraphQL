using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Repositories
{
    public class EAIPVReasonRepository : RepositoryBase<EAIPVReason>, IEAIPVReasonRepository
    {
        public EAIPVReasonRepository(IDbContext dbContext) : base(dbContext)
        {
        }
    }
}
