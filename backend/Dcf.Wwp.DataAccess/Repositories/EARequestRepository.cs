using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Repositories
{
    public class EARequestRepository : RepositoryBase<EARequest>, IEARequestRepository
    {
        public EARequestRepository(IDbContext dbContext) : base(dbContext)
        {
        }
    }
}
