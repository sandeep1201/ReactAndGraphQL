using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Repositories
{
    public class PlanSectionRepository : RepositoryBase<PlanSection>, IPlanSectionRepository
    {
        public PlanSectionRepository(IDbContext dbContext) : base(dbContext)
        {
        }
    }
}
