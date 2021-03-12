using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Repositories
{
    public class EACommentTypeBridgeRepository : RepositoryBase<EACommentTypeBridge>, IEACommentTypeBridgeRepository
    {
        public EACommentTypeBridgeRepository(IDbContext dbContext) : base(dbContext)
        {
        }
    }    
}
