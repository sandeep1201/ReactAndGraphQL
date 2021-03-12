using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Repositories
{
    public class EARequestParticipantBridgeRepository : RepositoryBase<EARequestParticipantBridge>, IEARequestParticipantBridgeRepository
    {
        public EARequestParticipantBridgeRepository(IDbContext dbContext) : base(dbContext)
        {
        }
    }    
}
