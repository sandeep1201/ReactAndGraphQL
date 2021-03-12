using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Repositories
{
    public class WageHourRepository : RepositoryBase<WageHour>, IWageHourRepository
    {
        public WageHourRepository(IDbContext dbContext) : base(dbContext)
        {
        }
    }
}