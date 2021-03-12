using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Repositories
{
    public class DrugScreeningStatusRepository : RepositoryBase<DrugScreeningStatus>, IDrugScreeningStatusRepository
    {
        public DrugScreeningStatusRepository(IDbContext dbContext) : base(dbContext)
        {
        }
    }
}
