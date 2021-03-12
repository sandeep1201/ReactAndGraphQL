using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Repositories
{
    public class ParticipationPeriodSummaryRepository : RepositoryBase<ParticipationPeriodSummary>, IParticipationPeriodSummaryRepository
    {
        public ParticipationPeriodSummaryRepository(IDbContext dbContext) : base(dbContext)
        {
        }
    }
}
