using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Repositories
{
    public class GoodCauseDeniedReasonRepository : RepositoryBase<GoodCauseDeniedReason>, IGoodCauseDeniedReasonRepository
    {
        public GoodCauseDeniedReasonRepository(IDbContext dbContext) : base(dbContext)
        {
        }
    }
}
