using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Repositories
{
    public class GoalStepRepository : RepositoryBase<GoalStep>, IGoalStepRepository
    {
        public GoalStepRepository(IDbContext dbContext) : base(dbContext)
        {
        }
    }    
}
