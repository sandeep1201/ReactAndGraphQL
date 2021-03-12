using NRules.Fluent.Dsl;

namespace Dcf.Wwp.Api.Library.Rules.Security
{
    [Tag("Access")]
    [Tag("Confidentiality")]
    [Name("IsConfidential")]
    public class IsConfidential //: Rule
    {
        #region Properties

        #endregion

        #region Methods

        //public override void Define()
        //{
        //    //RequestForAssistanceContract contract = null;

        //    When()
        //        //.Match<EligibilityByFPL>(() => fpl)
        //        .Match<RequestForAssistanceContract>(() => contract, c => c.ProgramId           == Wwp.Model.Interface.Constants.EnrolledProgram.TransformMilwaukeeJobsId)
        //        .Match<RequestForAssistanceContract>(() => contract, c => c.CountyOfResidenceId != 40);

        //    Then()
        //        .Do(ctx => contract.AddAlert("GEO")); // "Does not reside in geographical area of service."
        //}

        #endregion
    }
}
