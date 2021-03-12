using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Repositories
{
    public class EnrolledProgramJobTypeBridgeRepository : RepositoryBase<EnrolledProgramJobTypeBridge>, IEnrolledProgramJobTypeBridgeRepository
    {
        public EnrolledProgramJobTypeBridgeRepository(IDbContext dbContext) : base(dbContext)
        {
        }
    }
}
