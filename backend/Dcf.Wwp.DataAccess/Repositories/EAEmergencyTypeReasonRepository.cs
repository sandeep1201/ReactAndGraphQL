using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Repositories
{
    public class EAEmergencyTypeReasonRepository : RepositoryBase<EAEmergencyTypeReason>, IEAEmergencyTypeReasonRepository
    {
        public EAEmergencyTypeReasonRepository(IDbContext dbContext) : base(dbContext)
        {
        }
    }
}
