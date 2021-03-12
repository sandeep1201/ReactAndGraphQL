using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Repositories
{
    public class ActivityLocationRepository : RepositoryBase<ActivityLocation>, IActivityLocationRepository
    {
        public ActivityLocationRepository(IDbContext dbContext) : base(dbContext)
        {
        }
    }    
}
