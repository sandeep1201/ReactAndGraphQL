using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Repositories
{
    public class EARequestContactInfoRepository : RepositoryBase<EARequestContactInfo>, IEARequestContactInfoRepository
    {
        public EARequestContactInfoRepository(IDbContext dbContext) : base(dbContext)
        {
        }
    }
}
