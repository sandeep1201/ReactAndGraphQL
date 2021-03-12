using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Repositories
{
    public class ParticipationEntryHistoryRepository : RepositoryBase<ParticipationEntryHistory>, IParticipationEntryHistoryRepository
    {
        public ParticipationEntryHistoryRepository(IDbContext dbContext) : base(dbContext)
        {
        }
    }
}
