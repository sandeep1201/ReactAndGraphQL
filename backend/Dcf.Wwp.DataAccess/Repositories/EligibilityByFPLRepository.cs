using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Repositories
{
    public class EligibilityByFPLRepository : RepositoryBase<EligibilityByFPL>, IEligibilityByFPLRepository
    {
        public EligibilityByFPLRepository(IDbContext dbContext) : base(dbContext)
        {
        }
    }
}
