using NRules.Fluent.Dsl;
using Dcf.Wwp.Api.Library.Contracts;

namespace Dcf.Wwp.Api.Library.Rules
{
    [Tag("RFA")]
    [Name("TjTmjIsBenefitFromSubsidizedJobRule")]
    public class TjTmjIsBenefitFromSubsidizedJobRule : Rule
    {
        // Eligibility Rule:
        // If the Agency Worker selects "No" for the question "Is applicant able to obtain and benefit
        // from a subsidized job?" the following failure reason will be displayed and stored:
        // "Unable to obtain and benefit from a subsidized job"

        public override void Define()
        {
            RequestForAssistanceContract contract = null;

            When()
                .Match<RequestForAssistanceContract>(() => contract, c => c.TjTmjIsBenefitFromSubsidizedJob.HasValue, c => !c.TjTmjIsBenefitFromSubsidizedJob.Value);

            Then()
                .Do(ctx => contract.AddAlert("SUB")); // "Unable to obtain and benefit from a subsidized job"
        }
    }
}
