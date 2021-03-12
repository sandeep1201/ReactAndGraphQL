
using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Repositories
{
    public class ElementRepository : RepositoryBase<Element>, IElementRepository
    {
        public ElementRepository(IDbContext dbContext) : base(dbContext)
        {
        }
    }
}

