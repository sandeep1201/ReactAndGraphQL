using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Api.Library.Enums;
using Dcf.Wwp.Api.Library.Model.Api;
using NRules.Fluent.Dsl;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.Api.Library.Domains;
using Dcf.Wwp.Api.Library.Extensions;
using Dcf.Wwp.DataAccess.Models;
using SmartFormat;
using POPClaimType = Dcf.Wwp.Model.Interface.Constants.POPClaimType;
using RuleReason = Dcf.Wwp.Model.Interface.Constants.RuleReason;


namespace Dcf.Wwp.Api.Library.Rules.PreAddingPOPClaim
{
    [Tag(RuleReason.JobAttainmentPOPClaimAdd)]
    [Tag(RuleReason.JobRetentionPOPClaimAdd)]
    [Tag(RuleReason.JobAttainmentWithHighWage)]
    [Name("CanAddPOPClaimWithJA12Rule")]
    public class CanAddPOPClaimWithJA12Rule : Rule
    {
        public override void Define()
        {
            CodeLevelMessageContext               codeLevelMessageContext = null;
            POPClaimContract                      contract                = null;
            List<POPClaim>                        popClaimsInAgency       = null;
            POPClaimDomain.CheckDB2ClaimTypExists checkDB2ClaimTypExists  = null;

            When()
                .Match(() => contract,               c => c                                         != null)
                .Match(() => checkDB2ClaimTypExists, c => c                                         != null)
                .Match(() => popClaimsInAgency,      pops => pops.Any(pop => (pop.POPClaimType.Code == contract.POPClaimTypeCode || ((pop.POPClaimType.Code == POPClaimType.JobAttainmentCd || pop.POPClaimType.Code == POPClaimType.JobAttainmentWithHighWageCd) && (contract.POPClaimTypeCode == POPClaimType.JobAttainmentCd || contract.POPClaimTypeCode == POPClaimType.JobAttainmentWithHighWageCd))) &&
                                                                             (pop.ClaimPeriodBeginDate
                                                                              ?? pop.POPClaimEmploymentBridges
                                                                                    .FirstOrDefault(j => j.IsPrimary)
                                                                                    .EmploymentInformation.JobBeginDate.GetValueOrDefault())
                                                                             .WithinAYear(contract.ClaimPeriodBeginDate ?? contract.POPClaimEmployments
                                                                                                                                   .FirstOrDefault(i => i.IsPrimary)
                                                                                                                                   .JobBeginDate.ToDateMonthDayYear())) ||
                                                             checkDB2ClaimTypExists.HasClaimTypExists == true)
                .Match(() => codeLevelMessageContext, c => c                                          != null);

            Then()
                .Do(ctx => CreateMsg(codeLevelMessageContext, contract, popClaimsInAgency));
        }

        private void CreateMsg(CodeLevelMessageContext codeLevelMessageContext, POPClaimContract contract, List<POPClaim> popClaimsInAgency)
        {
            const string code         = RuleReason.POPJA12;
            POPClaim popClaimInAgency = popClaimsInAgency.First(pop => pop.POPClaimType.Code == contract.POPClaimTypeCode || ((pop.POPClaimType.Code == POPClaimType.JobAttainmentCd || pop.POPClaimType.Code == POPClaimType.JobAttainmentWithHighWageCd) && (contract.POPClaimTypeCode == POPClaimType.JobAttainmentCd || contract.POPClaimTypeCode == POPClaimType.JobAttainmentWithHighWageCd)));
            var          placeholder  = popClaimInAgency.POPClaimType.Description;
            var          placeHolders = new object[] { placeholder };

            var msg = Smart.Format(codeLevelMessageContext.GetMessageByCode(code), placeHolders);
            codeLevelMessageContext.AddMessageCodeAndLevel(msg, code, CodeLevel.Error);
        }
    }
}
