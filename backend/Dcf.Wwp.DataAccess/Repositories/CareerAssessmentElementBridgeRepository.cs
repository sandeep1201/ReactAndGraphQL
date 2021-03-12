

using Dcf.Wwp.DataAccess.Base;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.DataAccess.Repositories
{
    public class CareerAssessmentElementBridgeRepository : RepositoryBase<CareerAssessmentElementBridge>, ICareerAssessmentElementBridgeRepository
    {
        public CareerAssessmentElementBridgeRepository(IDbContext dbContext) : base(dbContext)
        {
        }
    }
}
