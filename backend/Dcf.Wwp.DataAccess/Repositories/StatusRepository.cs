using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Repositories
{
    public class StatusRepository : RepositoryBase<ParticipationStatusType>, IStatusRepository
    {
        public StatusRepository(IDbContext dbContext) : base(dbContext)
        {
        }
    }    
}
