using System.Collections.Generic;
using Dcf.Wwp.Api.Library.Contracts;

namespace Dcf.Wwp.Api.Library.Interfaces
{
    public interface IPlanDomain
    {
        #region Properties

        #endregion

        #region Methods

        List<PlanContract> GetW2PlansByParticipantId(int id);

        #endregion

        PlanSectionContract UpsertPlanSection(PlanSectionContract w2PlansSectionContract, int participantId);
    }
}
