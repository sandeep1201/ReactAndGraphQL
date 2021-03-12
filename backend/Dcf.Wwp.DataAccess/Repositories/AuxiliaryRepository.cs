using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Repositories
{
    public class AuxiliaryRepository : RepositoryBase<Auxiliary>, IAuxiliaryRepository
    {
        public AuxiliaryRepository(IDbContext dbContext) : base(dbContext)
        {
        }
    }
}
