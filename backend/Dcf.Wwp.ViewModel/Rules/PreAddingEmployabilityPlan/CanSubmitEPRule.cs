using Dcf.Wwp.Api.Library.Enums;
using Dcf.Wwp.Api.Library.Model.Api;
using NRules.Fluent.Dsl;
using Dcf.Wwp.Api.Library.Contracts;

namespace Dcf.Wwp.Api.Library.Rules.PreAddingEmployabilityPlan
{
    [Tag("EPSubmit")]
    [Name("CanSubmitEPRule")]
    public class CanSubmitEPRule : Rule
    {
        public override void Define()
        {
            MessageCodeLevelContext   messageCodeLevelResult = null;
            EmployabilityPlanContract contract               = null;
            bool                      hasContact             = false;

            // Check for participant Enrolled program and Workers contact info
            When()
                .Match(() => contract, c => c.EnrolledProgramId    == Wwp.Model.Interface.Constants.EnrolledProgram.ChildrenFirstId
                                            || c.EnrolledProgramId == Wwp.Model.Interface.Constants.EnrolledProgram.TransformMilwaukeeJobsId
                                            || c.EnrolledProgramId == Wwp.Model.Interface.Constants.EnrolledProgram.TransitionalJobsId)
                .Match(() => hasContact,             count => count == false)
                .Match(() => messageCodeLevelResult, c => c         != null);

            Then()
                // Error of code PS
                .Do(ctx => CreateMsg(messageCodeLevelResult));
        }

        private void CreateMsg(MessageCodeLevelContext messageCodeLevelResult)
        {
            const string code = Wwp.Model.Interface.Constants.RuleReason.EPSUBMIT;
            var          msg  = messageCodeLevelResult.GetMessegeByCode(code);
            messageCodeLevelResult.AddMessegeCodeAndLevel(msg, code, CodeLevel.Error);
        }
    }
}
