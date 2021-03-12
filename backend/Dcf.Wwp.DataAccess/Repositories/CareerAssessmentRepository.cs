
using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Repositories
{
    public class CareerAssessmentRepository : RepositoryBase<CareerAssessment>, ICareerAssessmentRepository
    {
        public CareerAssessmentRepository(IDbContext dbContext) : base(dbContext)
        {
        }
    }
}
