using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Repositories
{
    public class JRApplicationInfoRepository : RepositoryBase<JRApplicationInfo>, IJRApplicationInfoRepository
    {
        public JRApplicationInfoRepository(IDbContext dbContext) : base(dbContext)
        {
        }
    }
}
