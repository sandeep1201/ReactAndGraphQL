using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Repositories
{
    public class EACommentTypeRepository : RepositoryBase<EACommentType>, IEACommentTypeRepository
    {
        public EACommentTypeRepository(IDbContext dbContext) : base(dbContext)
        {
        }
    }    
}
