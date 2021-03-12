using NRules.Fluent.Dsl;
using Dcf.Wwp.Api.Library.Contracts;

namespace Dcf.Wwp.Api.Library.Rules
{
    [Tag("RFA")]
    [Name("AgeInYearsRule")]
    public class AgeInYearsRule : Rule
    {
        // Eligibility Rule:
        // If the applicant's "Age" is calculated to be under 18 years of age, the
        // following failure reason will be displayed and stored:
        // "Not at least 18 years of age"

        public override void Define()
        {
            RequestForAssistanceContract contract = null;
            RFARulesContext              context  = null;

            When()
                .Match<RequestForAssistanceContract>(() => contract)
                .Match<RFARulesContext>(() => context, c => c.Participant.AgeInYears.HasValue == true, c => c.Participant.AgeInYears.Value < 18);

            Then()
                .Do(ctx => contract.AddAlert("AGE")); // "Not at least 18 years of age."
        }
    }
}
