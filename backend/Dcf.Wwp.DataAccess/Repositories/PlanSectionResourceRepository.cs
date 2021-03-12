using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Repositories
{
    public class PlanSectionResourceRepository : RepositoryBase<PlanSectionResource>, IPlanSectionResourceRepository
    {
        public PlanSectionResourceRepository(IDbContext dbContext) : base(dbContext)
        {
        }
    }
}
