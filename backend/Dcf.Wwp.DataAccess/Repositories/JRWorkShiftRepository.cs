using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Repositories
{
    public class JRWorkShiftRepository : RepositoryBase<JRWorkShift>, IJRWorkShiftRepository
    {
        public JRWorkShiftRepository(IDbContext dbContext) : base(dbContext)
        {
        }
    }
}
