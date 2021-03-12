using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Repositories
{
    public class EmploymentInformationRepository : RepositoryBase<EmploymentInformation>, IEmploymentInformationRepository
    {
        public EmploymentInformationRepository(IDbContext dbContext) : base(dbContext)
        {
        }
    }
}
