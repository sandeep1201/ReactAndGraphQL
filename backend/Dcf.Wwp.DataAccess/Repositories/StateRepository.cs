using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Repositories
{
    public class StateRepository : RepositoryBase<State>, IStateRepository
    {
        public StateRepository(IDbContext dbContext) : base(dbContext)
        {
        }
    }    
}
