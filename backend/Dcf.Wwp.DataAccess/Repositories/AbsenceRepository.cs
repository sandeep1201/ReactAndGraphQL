using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Repositories
{
    public class AbsenceRepository : RepositoryBase<Absence>, IAbsenceRepository
    {
        public AbsenceRepository(IDbContext dbContext) : base(dbContext)
        {
        }
    }
}
