using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Repositories
{
    public class EAVerificationTypeBridgeRepository : RepositoryBase<EAVerificationTypeBridge>, IEAVerificationTypeBridgeRepository
    {
        public EAVerificationTypeBridgeRepository(IDbContext dbContext) : base(dbContext)
        {
        }
    }
}
