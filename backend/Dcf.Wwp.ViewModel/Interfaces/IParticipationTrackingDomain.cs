using System.Collections.Generic;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.Model.Interface.Model;

namespace Dcf.Wwp.Api.Library.Interfaces
{
    public interface IParticipationTrackingDomain
    {
        #region Properties

        #endregion

        #region Methods

        List<ParticipationTrackingContract> GetParticipationTrackingDetails(int                              participantId,                 string startDate, string endDate, bool isFromDetails, string programCode);
        ParticipationTrackingContract       UpsertParticipationTrackingDetails(ParticipationTrackingContract participationTrackingContract, string programCode);
        bool                                DeleteParticipationTrackingDetails(int                           id);
        List<ParticipationTrackingContract> MakeFullOrNoParticipation(int                                    participantId,        string makeFullOrNoParticipation, string startDate, string endDate, List<ParticipationTrackingContract> participationTrackingContracts, string programCode);
        CommitStatus                        UpdatePlacement(List<UpdatePlacement>                            UpdatePlacementModel, string modifiedBy);

        #endregion
    }
}
