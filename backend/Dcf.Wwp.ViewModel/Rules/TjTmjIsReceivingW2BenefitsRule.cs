using NRules.Fluent.Dsl;
using Dcf.Wwp.Api.Library.Contracts;

namespace Dcf.Wwp.Api.Library.Rules
{
    [Tag("RFA")]
    [Name("TjTmjIsReceivingW2BenefitsRule")]
    public class TjTmjIsReceivingW2BenefitsRule : Rule
    {
        // Eligibility Rule:
        // If the Agency Worker selects "Yes" for the question "Is applicant receiving W-2
        // benefits or services?" the following failure reason will be displayed and stored:
        // "Receiving W-2 benefits or services"

        public override void Define()
        {
            RequestForAssistanceContract contract = null;

            When()
                .Match<RequestForAssistanceContract>(() => contract, c => c.TjTmjIsReceivingW2Benefits.HasValue, c => c.TjTmjIsReceivingW2Benefits.Value);

            Then()
                .Do(ctx => contract.AddAlert("W-2")); // "Receiving W-2 benefits or services."
        }
    }
}
