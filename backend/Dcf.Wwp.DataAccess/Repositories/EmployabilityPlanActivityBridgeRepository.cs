using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Repositories
{
    public class EmployabilityPlanActivityBridgeRepository : RepositoryBase<EmployabilityPlanActivityBridge>, IEmployabilityPlanActivityBridgeRepository
    {
        public EmployabilityPlanActivityBridgeRepository(IDbContext dbContext) : base(dbContext)
        {
        }
    }    
}
