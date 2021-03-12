using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Repositories
{
    public class ContractAreaRepository : RepositoryBase<ContractArea>, IContractAreaRepository
    {
        public ContractAreaRepository(IDbContext dbContext) : base(dbContext)
        {
        }
    }    
}
