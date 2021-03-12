using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Repositories
{
    public class EASSNExemptTypeRepository : RepositoryBase<EASSNExemptType>, IEASSNExemptTypeRepository
    {
        public EASSNExemptTypeRepository(IDbContext dbContext) : base(dbContext)
        {
        }
    }
}
