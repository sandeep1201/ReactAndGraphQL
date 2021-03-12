using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Repositories
{
    public class FrequencyRepository : RepositoryBase<Frequency>, IFrequencyRepository
    {
        public FrequencyRepository(IDbContext dbContext) : base(dbContext)
        {
        }
    }    
}
