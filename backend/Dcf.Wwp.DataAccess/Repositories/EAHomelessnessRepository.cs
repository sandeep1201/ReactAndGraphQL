using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Repositories
{
    public class EAHomelessnessRepository : RepositoryBase<EAHomelessness>, IEAHomelessnessRepository
    {
        public EAHomelessnessRepository(IDbContext dbContext) : base(dbContext)
        {
        }
    }
}
