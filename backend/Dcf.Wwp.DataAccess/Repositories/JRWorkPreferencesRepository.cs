using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Repositories
{
    public class JRWorkPreferencesRepository : RepositoryBase<JRWorkPreferences>, IJRWorkPreferencesRepository
    {
        public JRWorkPreferencesRepository(IDbContext dbContext) : base(dbContext)
        {
        }
    }
}
