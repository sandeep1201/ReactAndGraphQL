using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.Api.Library.Enums;
using Dcf.Wwp.Api.Library.Model.Api;
using Dcf.Wwp.Model.Interface;
using NRules.Fluent.Dsl;
using SmartFormat;

namespace Dcf.Wwp.Api.Library.Rules.RFA
{
    [Tag("RFA-Validate")]
    [Name("TMJCFOfficeMatch")]
    public class TMJCFOfficeMatch : Rule
    {
        // RFA for Children First should match the office number for the TMJ office which can only be 8040.
        public override void Define()
        {
            RequestForAssistanceContract      contract               = null;
            List<IParticipantEnrolledProgram> peps                   = null;
            MessageCodeLevelContext           messageCodeLevelResult = null;

            When()
                .Match(() => messageCodeLevelResult,                  c => c != null)
                .Match<RequestForAssistanceContract>(() => contract,  c => c != null && c.IsCF)
                .Match<List<IParticipantEnrolledProgram>>(() => peps, p => p.FirstOrDefault(x => x.IsTmj && (x.IsEnrolled || x.IsReferred) && x.Office != null && x.Office.OfficeNumber != contract.WorkProgramOfficeNumber) != null);

            Then()
                // Error of code CFTJTMJOFF
                .Do(ctx => CreateMsg(messageCodeLevelResult, peps));
        }


        private void CreateMsg(MessageCodeLevelContext messageCodeLevelResult, List<IParticipantEnrolledProgram> peps)
        {
            // Example: This individual has been referred to WP Office [DANE - 811] for TMJ. Co-enrolled individuals must be in the same WP Office for both programs.
            const string code           = Wwp.Model.Interface.Constants.RuleReason.CFTJTMJOFF;
            var          ruleForcingPep = peps.FirstOrDefault(x => x.IsTmj && (x.IsReferred || x.IsEnrolled));

            var placeHolders = new object[] { ruleForcingPep.IsReferred ? "referred to" : "enrolled in", ruleForcingPep.Office.CountyAndTribe.CountyName, ruleForcingPep.Office.OfficeNumber.ToString().PadLeft(4, '0'), ruleForcingPep.EnrolledProgram?.ShortName?.Trim() };

            var msg = Smart.Format(messageCodeLevelResult.GetMessegeByCode(code), placeHolders);

            messageCodeLevelResult.AddMessegeCodeAndLevel(msg, code, CodeLevel.Error);
        }
    }
}
