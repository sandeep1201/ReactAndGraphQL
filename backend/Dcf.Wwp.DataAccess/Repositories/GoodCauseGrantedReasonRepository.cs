using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Repositories
{
    public class GoodCauseGrantedReasonRepository : RepositoryBase<GoodCauseGrantedReason>, IGoodCauseGrantedReasonRepository
    {
        public GoodCauseGrantedReasonRepository(IDbContext dbContext) : base(dbContext)
        {
        }
    }
}
