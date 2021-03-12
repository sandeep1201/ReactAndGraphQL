using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Repositories
{
    public class FrequencyTypeRepository : RepositoryBase<FrequencyType>, IFrequencyTypeRepository
    {
        public FrequencyTypeRepository(IDbContext dbContext) : base(dbContext)
        {
        }
    }    
}
