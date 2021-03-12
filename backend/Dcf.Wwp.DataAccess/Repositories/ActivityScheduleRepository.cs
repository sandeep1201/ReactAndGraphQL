using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Repositories
{
    public class ActivityScheduleRepository : RepositoryBase<ActivitySchedule>, IActivityScheduleRepository
    {
        public ActivityScheduleRepository(IDbContext dbContext) : base(dbContext)
        {
        }
    }    
}
