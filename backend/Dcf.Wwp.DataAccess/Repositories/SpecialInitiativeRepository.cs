using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Repositories
{
    public class SpecialInitiativeRepository : RepositoryBase<SpecialInitiative>, ISpecialInitiativeRepository
    {
        public SpecialInitiativeRepository(IDbContext dataContext) : base(dataContext)
        {
        }
    }
}
