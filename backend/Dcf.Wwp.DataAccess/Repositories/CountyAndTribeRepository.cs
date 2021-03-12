using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Repositories
{
    public class CountyAndTribeRepository : RepositoryBase<CountyAndTribe>, ICountyAndTribeRepository
    {
        public CountyAndTribeRepository(IDbContext dbContext) : base(dbContext)
        {
        }
    }
}
