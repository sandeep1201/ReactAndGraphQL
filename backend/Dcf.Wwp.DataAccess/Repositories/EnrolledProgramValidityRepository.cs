using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Repositories
{
    public class EnrolledProgramValidityRepository : RepositoryBase<EnrolledProgramValidity>, IEnrolledProgramValidityRepository
    {
        public EnrolledProgramValidityRepository(IDbContext dbContext) : base(dbContext)
        {
        }
    }    
}
