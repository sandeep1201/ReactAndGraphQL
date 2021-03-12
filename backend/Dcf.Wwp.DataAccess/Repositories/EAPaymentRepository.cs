using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Repositories
{
    public class EAPaymentRepository : RepositoryBase<EAPayment>, IEAPaymentRepository
    {
        public EAPaymentRepository(IDbContext dbContext) : base(dbContext)
        {
        }
    }
}
