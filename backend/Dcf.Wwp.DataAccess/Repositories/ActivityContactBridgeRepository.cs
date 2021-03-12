using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Repositories
{
    public class ActivityContactBridgeRepository : RepositoryBase<ActivityContactBridge>, IActivityContactBridgeRepository
    {
        public ActivityContactBridgeRepository(IDbContext dbContext) : base(dbContext)
        {
        }
    }    
}
