using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Repositories
{
    public class ActionAssigneeRepository : RepositoryBase<ActionAssignee>, IActionAssigneeRepository
    {
        public ActionAssigneeRepository(IDbContext dbContext) : base(dbContext)
        {
        }
    }
}
