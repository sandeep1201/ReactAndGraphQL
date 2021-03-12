using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Repositories
{
    public class ActionPriorityRepository : RepositoryBase<ActionPriority>, IActionPriorityRepository
    {
        public ActionPriorityRepository(IDbContext dbContext) : base(dbContext)
        {
        }
    }
}
