using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Repositories
{
    public class ParticipantEnrolledProgramRepository : RepositoryBase<ParticipantEnrolledProgram>, IParticipantEnrolledProgramRepository
    {
        public ParticipantEnrolledProgramRepository(IDbContext dbContext) : base(dbContext)
        {
        }
    }    
}
