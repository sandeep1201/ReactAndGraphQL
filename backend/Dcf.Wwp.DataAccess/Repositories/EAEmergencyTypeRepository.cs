using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Repositories
{
    public class EAEmergencyTypeRepository : RepositoryBase<EAEmergencyType>, IEAEmergencyTypeRepository
    {
        public EAEmergencyTypeRepository(IDbContext dbContext) : base(dbContext)
        {
        }
    }
}
