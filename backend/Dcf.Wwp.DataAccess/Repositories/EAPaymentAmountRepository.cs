using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Repositories
{
    public class EAPaymentAmountRepository : RepositoryBase<EAPaymentAmount>, IEAPaymentAmountRepository
    {
        public EAPaymentAmountRepository(IDbContext dbContext) : base(dbContext)
        {
        }
    }
}
