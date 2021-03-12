using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Repositories
{
    public class EAIPVStatusRepository : RepositoryBase<EAIPVStatus>, IEAIPVStatusRepository
    {
        public EAIPVStatusRepository(IDbContext dbContext) : base(dbContext)
        {
        }
    }
}
