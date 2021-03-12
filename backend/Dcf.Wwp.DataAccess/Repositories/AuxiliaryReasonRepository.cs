using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Repositories
{
    public class AuxiliaryReasonRepository : RepositoryBase<AuxiliaryReason>, IAuxiliaryReasonRepository
    {
        public AuxiliaryReasonRepository(IDbContext dbContext) : base(dbContext)
        {
        }
    }
}
