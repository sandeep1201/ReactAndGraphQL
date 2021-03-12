using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Repositories
{
    public class GoalTypeRepository : RepositoryBase<GoalType>, IGoalTypeRepository
    {
        public GoalTypeRepository(IDbContext dbContext) : base(dbContext)
        {
        }
    }    
}
