using System;
using System.Collections.Generic;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.Api.Library.Interfaces
{
    public interface IPOPClaimDomain
    {
        #region Properties

        #endregion

        #region Methods

        List<POPClaimContract>           GetPOPClaims(int                                participantId);
        List<POPClaimContract>           GetPOPClaimsByAgency(string                     agencyCode = null);
        POPClaimContract                 GetPOPClaim(int                                 i);
        List<POPClaimEmploymentContract> GetEmploymentsForPOP(string                     pin,      int  popClaimId);
        void                             UpsertPOPClaim(POPClaimContract                 contract, bool isSystemGenerated = false);
        PreAddingPOPClaimContract        PreAddCheck(POPClaimContract                    contract);
        List<POPClaimContract>           GetPOPClaimsWithStatuses(List<string>           statuses,          string agencyCode = null);
        bool                             InsertSystemGeneratedPOPClaim(EmployabilityPlan employabilityPlan, string activityTypeCode, string activityCompletionReasonCode, DateTime? activityEndDate, int activityId, string popclaimType);

        #endregion
    }
}
