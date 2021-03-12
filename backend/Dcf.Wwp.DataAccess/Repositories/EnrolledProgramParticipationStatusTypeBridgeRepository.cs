using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Repositories
{
    public class EnrolledProgramParticipationStatusTypeBridgeRepository : RepositoryBase<EnrolledProgramParticipationStatusTypeBridge>, IEnrolledProgramParticipationStatusTypeBridgeRepository
    {
        public EnrolledProgramParticipationStatusTypeBridgeRepository(IDbContext dbContext) : base(dbContext)
        {
        }
    }    
}
