using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Repositories
{
    public class EnrolledProgramRepository : RepositoryBase<EnrolledProgram>, IEnrolledProgramRepository
    {
        public EnrolledProgramRepository(IDbContext dbContext) : base(dbContext)
        {
        }
    }    
}
