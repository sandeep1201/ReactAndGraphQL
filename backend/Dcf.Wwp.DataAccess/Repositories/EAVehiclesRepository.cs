using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Repositories
{
    public class EAVehiclesRepository : RepositoryBase<EAVehicles>, IEAVehiclesRepository
    {
        public EAVehiclesRepository(IDbContext dbContext) : base(dbContext)
        {
        }
    }
}
