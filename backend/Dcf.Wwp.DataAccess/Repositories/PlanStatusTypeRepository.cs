using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Repositories
{
    public class PlanStatusTypeRepository : RepositoryBase<PlanStatusType>, IPlanStatusTypeRepository
    {
        public PlanStatusTypeRepository(IDbContext dbContext) : base(dbContext)
        {
        }
    }
}
