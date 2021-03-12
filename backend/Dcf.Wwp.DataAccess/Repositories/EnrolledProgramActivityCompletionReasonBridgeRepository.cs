using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Repositories
{
    public class EnrolledProgramActivityCompletionReasonBridgeRepository : RepositoryBase<EnrolledProgramActivityCompletionReasonBridge>, IEnrolledProgramActivityCompletionReasonBridgeRepository
    {
        public EnrolledProgramActivityCompletionReasonBridgeRepository(IDbContext dbContext) : base(dbContext)
        {
        }
    }    
}
