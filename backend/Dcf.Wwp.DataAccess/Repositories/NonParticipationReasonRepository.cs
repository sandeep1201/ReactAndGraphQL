using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Repositories
{
    public class NonParticipationReasonRepository : RepositoryBase<NonParticipationReason>, INonParticipationReasonRepository
    {
        public NonParticipationReasonRepository(IDbContext dbContext) : base(dbContext)
        {
        }
    }
}
