using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Repositories
{
    public class JRInterviewInfoRepository : RepositoryBase<JRInterviewInfo>, IJRInterviewInfoRepository
    {
        public JRInterviewInfoRepository(IDbContext dbContext) : base(dbContext)
        {
        }
    }
}
