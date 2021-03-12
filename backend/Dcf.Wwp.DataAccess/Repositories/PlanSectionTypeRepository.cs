using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Repositories
{
    public class PlanSectionTypeRepository : RepositoryBase<PlanSectionType>, IPlanSectionTypeRepository
    {
        public PlanSectionTypeRepository(IDbContext dbContext) : base(dbContext)
        {
        }
    }
}
