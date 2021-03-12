using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Repositories
{
    public class EAIPVReasonBridgeRepository : RepositoryBase<EAIPVReasonBridge>, IEAIPVReasonBridgeRepository
    {
        public EAIPVReasonBridgeRepository(IDbContext dbContext) : base(dbContext)
        {
        }
    }
}
