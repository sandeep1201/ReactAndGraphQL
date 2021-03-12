using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Repositories
{
    public class EmployabilityPlanRepository : RepositoryBase<EmployabilityPlan>, IEmployabilityPlanRepository
    {
        public EmployabilityPlanRepository(IDbContext dbContext) : base(dbContext)
        {
        }
    }    
}
