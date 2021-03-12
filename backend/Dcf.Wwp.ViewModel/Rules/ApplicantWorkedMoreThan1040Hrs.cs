using NRules.Fluent.Dsl;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Api.Library.Rules
{
    [Tag("RFA")]
    [Name("ApplicantWorkedMoreThan1040Hrs")]
    public class ApplicantWorkedMoreThan1040Hrs : Rule
    {
        /// <summary>
        /// Eligibility Rule are per US1387.
        /// </summary>
        public override void Define()
        {
            //RFARulesContext              rfaCtx      = null;
            RequestForAssistanceContract contract = null;

            When()
                   .Match<RequestForAssistanceContract>(() => contract, c => c.TjTmjHasWorked1040Hours == true);

            Then()
                .Do(ctx => contract.AddAlert("TOT")); // TjTmjHasWorked1040Hours 
        }
    }
}