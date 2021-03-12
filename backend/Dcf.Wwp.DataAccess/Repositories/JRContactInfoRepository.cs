using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Repositories
{
    public class JRContactInfoRepository : RepositoryBase<JRContactInfo>, IJRContactInfoRepository
    {
        public JRContactInfoRepository(IDbContext dbContext) : base(dbContext)
        {
        }
    }
}
