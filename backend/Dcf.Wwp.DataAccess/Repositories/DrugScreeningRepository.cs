using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Repositories
{
    public class DrugScreeningRepository : RepositoryBase<DrugScreening>, IDrugScreeningRepository
    {
        public DrugScreeningRepository(IDbContext dbContext) : base(dbContext)
        {
        }
    }
}
