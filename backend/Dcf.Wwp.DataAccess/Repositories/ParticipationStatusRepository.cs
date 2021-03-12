using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Repositories
{
    public class ParticipationStatusRepository : RepositoryBase<ParticipationStatus>, IParticipationStatusRepository
    {
        public ParticipationStatusRepository(IDbContext dbContext) : base(dbContext)
        {
        }
    }    
}
