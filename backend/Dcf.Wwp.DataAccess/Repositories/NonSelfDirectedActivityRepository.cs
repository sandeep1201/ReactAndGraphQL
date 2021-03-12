using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Repositories
{
    public class NonSelfDirectedActivityRepository : RepositoryBase<NonSelfDirectedActivity>, INonSelfDirectedActivityRepository
    {
        public NonSelfDirectedActivityRepository(IDbContext dbContext) : base(dbContext)
        {
        }
    }    
}
