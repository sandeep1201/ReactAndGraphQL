using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.Api.Library.Enums;
using Dcf.Wwp.Api.Library.Model.Api;
using Dcf.Wwp.Model.Interface;
using NRules.Fluent.Dsl;
using SmartFormat;

namespace Dcf.Wwp.Api.Library.Rules.RFA.PreCheck
{
    [Tag("RFA-PreCheck")]
    [Name("TJRfaWithTmjConenrollment")]
    public class TJRfaWithTmjConenrollment : Rule
    {
        public override void Define()
        {
            RequestForAssistanceContract contract = null;
            List<IRequestForAssistance>  rfas     = null;
            MessageCodeLevelContext messageCodeLevelResult = null;

            IRequestForAssistance ruleEnforcingRfa = null;

            When()
                .Match<MessageCodeLevelContext>(() => messageCodeLevelResult, c => c != null)
                .Match<RequestForAssistanceContract>(() => contract, c => c.IsTJ)
                .Or(x => x
                    .Match<IParticipant>(p => p.ParticipantEnrolledPrograms.Any(program => program.IsTmj && (program.IsEnrolled || program.IsReferred)))
                    .And(xx => xx
                        .Match<List<IRequestForAssistance>>(() => rfas, r => r.Any(rs => rs.IsTMJ && (rs.IsEnrolled || rs.IsInProgress || rs.IsReferred)))));
                        // .Let(() => ruleEnforcingRfa, () => rfas.FirstOrDefault(rs => rs.IsTMJ && (rs.IsEnrolled || rs.IsInProgress || rs.IsReferred)));
                        // TODO: .Let returns null if there are no RFAs and throws an exception. Figure out how to nest this so it only runs when rfas exist.

            Then()
                // "Individual is currently enrolled in TMJ. TMJ and TJ cannot be co-enrolled.".
                .Do(ctx => CreateMsg(messageCodeLevelResult));
        }

        private void CreateMsg(MessageCodeLevelContext messageCodeLevelResult)
        {
            const string code = Wwp.Model.Interface.Constants.RuleReason.TJENR;
            var msg = Smart.Format(messageCodeLevelResult.GetMessegeByCode(code));
            messageCodeLevelResult.AddMessegeCodeAndLevel(msg, code, CodeLevel.Warning);
        }

        private void CreateMsg(MessageCodeLevelContext messageCodeLevelResult, IRequestForAssistance rfa)
        {
            var code = "";
  
            if (rfa.IsEnrolled)
                // Individual is currently enrolled in TMJ. TMJ and TJ cannot be co-enrolled.
                code = Wwp.Model.Interface.Constants.RuleReason.TMJENR;
            else if (rfa.IsReferred)
                // Individual has been referred to TMJ. TMJ and TJ cannot be co-enrolled.
                code = Wwp.Model.Interface.Constants.RuleReason.TMJREF;
            else if(rfa.IsInProgress)
                // There is a RFA In Progress for TMJ. TMJ and TJ cannot be co-enrolled.
                code = Wwp.Model.Interface.Constants.RuleReason.TMJIP;

            var msg = messageCodeLevelResult.GetMessegeByCode(code);
            messageCodeLevelResult.AddMessegeCodeAndLevel(msg, code, CodeLevel.Warning);
        }
    }
}
