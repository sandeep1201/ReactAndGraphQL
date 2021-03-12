using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Repositories
{
    public class YesNoUnknownLookupRepository : RepositoryBase<YesNoUnknownLookup>, IYesNoUnknownLookupRepository
    {
        public YesNoUnknownLookupRepository(IDbContext dbContext) : base(dbContext)
        {
        }
    }
}
