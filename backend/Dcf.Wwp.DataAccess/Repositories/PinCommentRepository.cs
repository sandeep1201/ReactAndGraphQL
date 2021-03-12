using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Repositories
{
    public class PinCommentRepository : RepositoryBase<PinComment>, IPinCommentRepository
    {
        public PinCommentRepository(IDbContext dbContext) : base(dbContext)
        {
        }
    }    
}
