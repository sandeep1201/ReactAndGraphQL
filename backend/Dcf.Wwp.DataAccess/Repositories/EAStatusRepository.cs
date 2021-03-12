using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Repositories
{
    public class EAStatusRepository : RepositoryBase<EAStatus>, IEAStatusRepository
    {
        public EAStatusRepository(IDbContext dbContext) : base(dbContext)
        {
        }
    }
}
