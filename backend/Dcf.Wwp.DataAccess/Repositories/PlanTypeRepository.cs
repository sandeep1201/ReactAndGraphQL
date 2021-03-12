using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Repositories
{
    public class PlanTypeRepository : RepositoryBase<PlanType>, IPlanTypeRepository
    {
        public PlanTypeRepository(IDbContext dbContext) : base(dbContext)
        {
        }
    }
}
