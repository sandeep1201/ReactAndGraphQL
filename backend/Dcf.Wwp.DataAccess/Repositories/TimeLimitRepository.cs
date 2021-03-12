using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Repositories
{
    public class TimeLimitRepository: RepositoryBase<TimeLimit>, ITimeLimitRepository
    {
        public TimeLimitRepository(IDbContext dbContext) : base(dbContext)
        {
        }
    }
}





