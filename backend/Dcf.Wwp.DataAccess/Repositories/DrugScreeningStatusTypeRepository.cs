using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Repositories
{
    public class DrugScreeningStatusTypeRepository : RepositoryBase<DrugScreeningStatusType>, IDrugScreeningStatusTypeRepository
    {
        public DrugScreeningStatusTypeRepository(IDbContext dbContext) : base(dbContext)
        {
        }
    }
}
