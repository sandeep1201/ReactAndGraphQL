using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Repositories
{
    public class EARequestEmergencyTypeBridgeRepository : RepositoryBase<EARequestEmergencyTypeBridge>, IEARequestEmergencyTypeBridgeRepository
    {
        public EARequestEmergencyTypeBridgeRepository(IDbContext dbContext) : base(dbContext)
        {
        }
    }
}
