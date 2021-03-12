using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Repositories
{
    public class ActionItemRepository : RepositoryBase<ActionItem>, IActionItemRepository
    {
        public ActionItemRepository(IDbContext dbContext) : base(dbContext)
        {
        }
    }
}
