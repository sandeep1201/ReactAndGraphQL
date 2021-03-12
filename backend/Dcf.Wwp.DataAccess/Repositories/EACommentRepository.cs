using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Repositories
{
    public class EACommentRepository : RepositoryBase<EAComment>, IEACommentRepository
    {
        public EACommentRepository(IDbContext dbContext) : base(dbContext)
        {
        }
    }    
}
