using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Repositories
{
    public class EARelationshipTypeRepository : RepositoryBase<EARelationshipType>, IEARelationshipTypeRepository
    {
        public EARelationshipTypeRepository(IDbContext dbContext) : base(dbContext)
        {
        }
    }
}
