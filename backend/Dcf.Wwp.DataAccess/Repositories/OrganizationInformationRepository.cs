using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Repositories
{
    public class OrganizationInformationRepository : RepositoryBase<OrganizationInformation>, IOrganizationInformationRepository
    {
        public OrganizationInformationRepository(IDbContext dbContext) : base(dbContext)
        {
        }
    }
}
