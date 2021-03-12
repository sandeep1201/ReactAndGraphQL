using NRules.Fluent.Dsl;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Api.Library.Rules
{
    [Tag("RFA")]
    [Name("FPLisNullRule")]
    public class FPLisNullRule : Rule
    {
        public override void Define()
        {
            //RFARulesContext              rfaCtx      = null;
            EligibilityByFPL             fpl      = null;
            RequestForAssistanceContract contract = null;

            When()
                .Match<EligibilityByFPL>(() => fpl, f => f.Pct150PerMonth == null);

            Then()
                .Do(ctx => contract.AddAlert("FPLERR1")); // "Annual household income rules cannot be verified."
        }
    }
}
