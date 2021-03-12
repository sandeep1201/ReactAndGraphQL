using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Repositories
{
    public class ActivityCompletionReasonRepository : RepositoryBase<ActivityCompletionReason>, IActivityCompletionReasonRepository
    {
        public ActivityCompletionReasonRepository(IDbContext dbContext) : base(dbContext)
        {
        }
    }    
}
