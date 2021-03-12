using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Repositories
{
    public class SupportiveServiceRepository : RepositoryBase<SupportiveService>, ISupportiveServiceRepository
    {
        public SupportiveServiceRepository(IDbContext dbContext) : base(dbContext)
        {
        }
    }    
}
