using System.Collections.Generic;
using Dcf.Wwp.Api.Library.Contracts;

namespace Dcf.Wwp.Api.Library.Interfaces
{
    public interface ICareerAssessmentDomain
    {
        #region Properties

        #endregion

        #region Methods

        List<CareerAssessmentContract> GetCareerAssessmentsForPin(decimal           pin);
        CareerAssessmentContract GetCareerAssessment(int                     id);
        CareerAssessmentContract UpsertCareerAssessment(CareerAssessmentContract careerAssessmentContract, string pin);

        #endregion
    }
}
