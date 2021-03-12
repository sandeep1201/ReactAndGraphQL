using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Repositories
{
    public class EmployabilityPlanGoalBridgeRepository : RepositoryBase<EmployabilityPlanGoalBridge>, IEmployabilityPlanGoalBridgeRepository
    {
        public EmployabilityPlanGoalBridgeRepository(IDbContext dbContext) : base(dbContext)
        {
        }
    }    
}
