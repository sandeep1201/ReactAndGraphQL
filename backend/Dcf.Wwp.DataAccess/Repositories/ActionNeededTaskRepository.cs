using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Repositories
{
    public class ActionNeededTaskRepository : RepositoryBase<ActionNeededTask>, IActionNeededTaskRepository
    {
        public ActionNeededTaskRepository(IDbContext dbContext) : base(dbContext)
        {
        }
    }
}
