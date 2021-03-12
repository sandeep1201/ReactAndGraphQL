using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Repositories
{
    public class EAAssetsRepository : RepositoryBase<EAAssets>, IEAAssetsRepository
    {
        public EAAssetsRepository(IDbContext dbContext) : base(dbContext)
        {
        }
    }
}
