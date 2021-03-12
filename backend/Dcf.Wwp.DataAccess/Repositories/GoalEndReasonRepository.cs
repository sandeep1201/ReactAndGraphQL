using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Repositories
{
    public class GoalEndReasonRepository : RepositoryBase<GoalEndReason>, IGoalEndReasonRepository
    {
        public GoalEndReasonRepository(IDbContext dbContext) : base(dbContext)
        {
        }
    }    
}
