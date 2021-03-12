using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Repositories
{
    public class AuxiliaryStatusRepository : RepositoryBase<AuxiliaryStatus>, IAuxiliaryStatusRepository
    {
        public AuxiliaryStatusRepository(IDbContext dbContext) : base(dbContext)
        {
        }
    }
}
