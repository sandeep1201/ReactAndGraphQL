using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Repositories
{
    public class ActivityScheduleFrequencyBridgeRepository : RepositoryBase<ActivityScheduleFrequencyBridge>, IActivityScheduleFrequencyBridgeRepository
    {
        public ActivityScheduleFrequencyBridgeRepository(IDbContext dbContext) : base(dbContext)
        {
        }
    }    
}
