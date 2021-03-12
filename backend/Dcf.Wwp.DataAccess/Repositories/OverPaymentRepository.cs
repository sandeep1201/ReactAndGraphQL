using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Repositories
{
    public class OverPaymentRepository : RepositoryBase<OverPayment>, IOverPaymentRepository
    {
        public OverPaymentRepository(IDbContext dbContext) : base(dbContext)
        {
        }
    }
}
