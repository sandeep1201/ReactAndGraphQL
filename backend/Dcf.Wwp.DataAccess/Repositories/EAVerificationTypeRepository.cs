using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Repositories
{
    public class EAVerificationTypeRepository : RepositoryBase<EAVerificationType>, IEAVerificationTypeRepository
    {
        public EAVerificationTypeRepository(IDbContext dbContext) : base(dbContext)
        {
        }
    }
}
