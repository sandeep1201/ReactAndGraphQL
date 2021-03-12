using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Repositories
{
    public class EmployabilityPlanEmploymentInfoBridgeRepository: RepositoryBase<EmployabilityPlanEmploymentInfoBridge>, IEmployabilityPlanEmploymentInfoBridgeRepository
    {
        public EmployabilityPlanEmploymentInfoBridgeRepository(IDbContext dbContext) : base(dbContext)
        {
        }
    }
}
