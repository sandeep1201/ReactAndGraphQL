using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Repositories
{
    public class EAIndividualTypeRepository : RepositoryBase<EAIndividualType>, IEAIndividualTypeRepository
    {
        public EAIndividualTypeRepository(IDbContext dbContext) : base(dbContext)
        {
        }
    }
}
