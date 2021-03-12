using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Repositories
{
    public class ParticipationEntryRepository : RepositoryBase<ParticipationEntry>, IParticipationEntryRepository
    {
        public ParticipationEntryRepository(IDbContext dbContext) : base(dbContext)
        {
        }
    }
}
