using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Repositories
{
    public class ParticipationMakeUpEntryRepository : RepositoryBase<ParticipationMakeUpEntry>, IParticipationMakeUpEntryRepository
    {
        public ParticipationMakeUpEntryRepository(IDbContext dbContext) : base(dbContext)
        {
        }
    }
}
