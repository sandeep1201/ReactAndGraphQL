using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Repositories
{
    public class PinCommentTypeRepository : RepositoryBase<PinCommentType>, IPinCommentTypeRepository
    {
        public PinCommentTypeRepository(IDbContext dbContext) : base(dbContext)
        {
        }
    }
}
