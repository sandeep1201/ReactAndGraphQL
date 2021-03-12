using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Repositories
{
    public class EARequestStatusRepository : RepositoryBase<EARequestStatus>, IEARequestStatusRepository
    {
        public EARequestStatusRepository(IDbContext dbContext) : base(dbContext)
        {
        }
    }
}
