using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Repositories
{
    public class ParticipantPlacementRepository : RepositoryBase<ParticipantPlacement>, IParticipantPlacementRepository
    {
        public ParticipantPlacementRepository(IDbContext dbContext) : base(dbContext)
        {
        }
    }
}
