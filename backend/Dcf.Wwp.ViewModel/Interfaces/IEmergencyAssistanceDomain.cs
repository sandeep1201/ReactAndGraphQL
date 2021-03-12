using System;
using System.Collections.Generic;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.Api.Library.Contracts.EmergencyAssistance;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.Api.Library.Interfaces
{
    public interface IEmergencyAssistanceDomain
    {
        #region Properties

        #endregion

        #region Methods

        ParticipantsContract          GetEAGroupConfidentialParticipant(string                pin);
        List<EARequestContract>       GetRequests(string                                      pin, List<EARequest> request = null);
        EARequestContract             GetRequest(int                                          id,  EARequest       request = null);
        List<EAAGMembers>             GetAGMembers(string                                     pin, int             id);
        T0011                         SearchParticipant(string                                pin);
        EAAgencySummaryContract       GetEARequestAgencySummary(int                           id);
        List<EAIPVContract>           GetIPVs(string                                          pin);
        EAIPVContract                 GetIPV(int                                              id);
        EAPaymentContract             GetPayment(int                                          id,  EAPayment              payment = null);
        EARequestContract             UpsertDemographics(string                               pin, EADemographicsContract contract);
        EAEmergencyTypeContract       UpsertEmergencyType(EAEmergencyTypeContract             contract);
        EAGroupMembersContract        UpsertGroupMembers(EAGroupMembersContract               contract);
        EAHouseHoldFinancialsContract UpsertHouseHoldFinancials(EAHouseHoldFinancialsContract contract);
        EAAgencySummaryContract       UpsertAgencySummary(string                              pin,      EAAgencySummaryContract contract);
        CommentContract               UpsertComments(CommentContract                          contract, int                     requestId);
        void                          UpsertIPV(EAIPVContract                                 contract, string                  pin);
        EAPaymentContract             UpsertPayment(EAPaymentContract                         contract);

        #endregion
    }
}
