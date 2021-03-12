using System.Collections.Generic;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.ConnectedServices.Mci;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Api.Library.Interfaces
{
    public interface IClientRegistrationViewModel
    {
        #region Properties

        #endregion

        #region Methods

        ClientRegistrationContract GetClientRegistration(long mciId, DemographicResultContract demographicContract = null);
        StatusContract UpsertClientRegistration(ClientRegistrationContract contract);
        List<DemographicResultContract> GetClearanceSearchResults(DemographicSearchContract contract);
        List<DemographicSearchMatch> SearchByDemographics(DemographicSearchContract contract);
        IParticipant GetParticipantByMciId(decimal mciID);

        #endregion
    }
}
