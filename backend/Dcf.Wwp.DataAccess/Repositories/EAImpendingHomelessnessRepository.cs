using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Repositories
{
    public class EAImpendingHomelessnessRepository : RepositoryBase<EAImpendingHomelessness>, IEAImpendingHomelessnessRepository
    {
        public EAImpendingHomelessnessRepository(IDbContext dbContext) : base(dbContext)
        {
        }
    }
}
