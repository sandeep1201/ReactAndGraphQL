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
    [Name("CFRfaWithFCDPCoenrollment")]
    public class CFRfaWithFCDPCoenrollment : Rule
    {
        public override void Define()
        {
            RequestForAssistanceContract contract               = null;
            List<IRequestForAssistance>  rfas                   = null;
            MessageCodeLevelContext      messageCodeLevelResult = null;
            IRequestForAssistance        ruleEnforcingRfa       = null;

            When()
                .Match<MessageCodeLevelContext>(() => messageCodeLevelResult, c => c != null)
                .Match<RequestForAssistanceContract>(() => contract,          c => c.IsCF)
                .Match<List<IRequestForAssistance>>(() => rfas,               r => r.Any(rs => rs.IsFCDP && (rs.IsEnrolled || rs.IsInProgress || rs.IsReferred)));

            Then()
                // "A new RFA for FCDP cannot be created - Children First already exists".
                .Do(ctx => CreateMsg(messageCodeLevelResult));
        }

        private void CreateMsg(MessageCodeLevelContext messageCodeLevelResult)
        {
            const string code = Wwp.Model.Interface.Constants.RuleReason.CFFCDP;
            var          msg  = Smart.Format(messageCodeLevelResult.GetMessegeByCode(code));
            messageCodeLevelResult.AddMessegeCodeAndLevel(msg, code, CodeLevel.Warning);
        }


        private void CreateMsg(MessageCodeLevelContext messageCodeLevelResult, IRequestForAssistance rfa)
        {
            const string code = Wwp.Model.Interface.Constants.RuleReason.FCDP;
            var          msg  = Smart.Format(messageCodeLevelResult.GetMessegeByCode(code), rfa.RequestForAssistanceStatus.Name);
            messageCodeLevelResult.AddMessegeCodeAndLevel(msg, code, CodeLevel.Warning);
        }
    }
}
