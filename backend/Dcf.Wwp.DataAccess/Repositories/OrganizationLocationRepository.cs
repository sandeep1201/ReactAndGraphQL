using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Repositories
{
    public class OrganizationLocationRepository : RepositoryBase<OrganizationLocation>, IOrganizationLocationRepository
    {
        public OrganizationLocationRepository(IDbContext dbContext) : base(dbContext)
        {
        }
    }
}
