using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Repositories
{
    public class PlacementTypeRepository : RepositoryBase<PlacementType>, IPlacementTypeRepository
    {
        public PlacementTypeRepository(IDbContext dbContext) : base(dbContext)
        {
        }
    }
}
