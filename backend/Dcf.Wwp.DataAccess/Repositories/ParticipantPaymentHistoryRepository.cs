using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Repositories
{
    public class ParticipantPaymentHistoryRepository : RepositoryBase<ParticipantPaymentHistory>, IParticipantPaymentHistoryRepository
    {
        public ParticipantPaymentHistoryRepository(IDbContext dbContext) : base(dbContext)
        {
        }
    }
}
