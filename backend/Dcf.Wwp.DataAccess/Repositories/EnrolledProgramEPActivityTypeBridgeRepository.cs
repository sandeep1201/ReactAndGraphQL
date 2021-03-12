using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Repositories
{
    public class EnrolledProgramEPActivityTypeBridgeRepository : RepositoryBase<EnrolledProgramEPActivityTypeBridge>, IEnrolledProgramEPActivityTypeBridgeRepository
    {
        public EnrolledProgramEPActivityTypeBridgeRepository(IDbContext dbContext) : base(dbContext)
        {
        }
    }
}
