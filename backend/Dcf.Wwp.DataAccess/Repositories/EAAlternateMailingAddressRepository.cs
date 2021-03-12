using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Repositories
{
    public class EAAlternateMailingAddressRepository : RepositoryBase<EAAlternateMailingAddress>, IEAAlternateMailingAddressRepository
    {
        public EAAlternateMailingAddressRepository(IDbContext dbContext) : base(dbContext)
        {
        }
    }    
}
