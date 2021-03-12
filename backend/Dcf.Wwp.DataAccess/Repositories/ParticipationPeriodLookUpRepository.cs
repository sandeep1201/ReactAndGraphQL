using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Repositories
{
    public class ParticipationPeriodLookUpRepository : RepositoryBase<ParticipationPeriodLookUp>, IParticipationPeriodLookUpRepository
    {
        public ParticipationPeriodLookUpRepository(IDbContext dbContext) : base(dbContext)
        {
        }
    }
}
