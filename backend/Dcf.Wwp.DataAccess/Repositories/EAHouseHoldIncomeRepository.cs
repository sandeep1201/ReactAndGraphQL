using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Repositories
{
    public class EAHouseHoldIncomeRepository : RepositoryBase<EAHouseHoldIncome>, IEAHouseHoldIncomeRepository
    {
        public EAHouseHoldIncomeRepository(IDbContext dbContext) : base(dbContext)
        {
        }
    }
}
