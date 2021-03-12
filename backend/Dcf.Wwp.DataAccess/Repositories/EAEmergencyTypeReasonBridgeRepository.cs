using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Repositories
{
    public class EAEmergencyTypeReasonBridgeRepository : RepositoryBase<EAEmergencyTypeReasonBridge>, IEAEmergencyTypeReasonBridgeRepository
    {
        public EAEmergencyTypeReasonBridgeRepository(IDbContext dbContext) : base(dbContext)
        {
        }
    }
}
