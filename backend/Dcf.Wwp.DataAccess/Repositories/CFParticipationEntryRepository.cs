using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Repositories
{
    public class CFParticipationEntryRepository : RepositoryBase<CFParticipationEntry>, ICFParticipationEntryRepository
    {
        public CFParticipationEntryRepository(IDbContext dbContext) : base(dbContext)
        {
        }
    }
}
