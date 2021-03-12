using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Repositories
{
    public class EAFinancialNeedRepository : RepositoryBase<EAFinancialNeed>, IEAFinancialNeedRepository
    {
        public EAFinancialNeedRepository(IDbContext dbContext) : base(dbContext)
        {
        }
    }
}
