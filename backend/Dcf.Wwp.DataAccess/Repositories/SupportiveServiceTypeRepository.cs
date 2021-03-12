using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Repositories
{
    public class SupportiveServiceTypeRepository : RepositoryBase<SupportiveServiceType>, ISupportiveServiceTypeRepository
    {
        public SupportiveServiceTypeRepository(IDbContext dbContext) : base(dbContext)
        {
        }
    }    
}
