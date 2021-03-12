﻿using System.Collections.Generic;
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
    [Name("TjRfaWithTjCoenrollment")]
    public class TjRfaWithTjCoenrollment : Rule
    {
        public override void Define()
        {
            RequestForAssistanceContract contract               = null;
            List<IRequestForAssistance>  rfas                   = null;
            MessageCodeLevelContext      messageCodeLevelResult = null;
            IRequestForAssistance        ruleEnforcingRfa       = null;

            When()
                .Match<MessageCodeLevelContext>(() => messageCodeLevelResult, c => c != null)
                .Match<RequestForAssistanceContract>(() => contract, c => c.IsTJ)
                .Or(x => x
                    .And(xx => xx
                        .Match<IParticipant>(p => p.ParticipantEnrolledPrograms.Any(program => program.IsTJ && (program.IsEnrolled || program.IsReferred)))
                        .Match<List<IRequestForAssistance>>(() => rfas, r => !r.Any(rs => rs.IsTJ && (rs.IsEnrolled || rs.IsInProgress || rs.IsReferred))))
                    .And(xx => xx
                        .Match<List<IRequestForAssistance>>(() => rfas, r => r.Any(rs => rs.IsTJ && (rs.IsEnrolled || rs.IsInProgress || rs.IsReferred)))));
                        // .Let(() => ruleEnforcingRfa, () => rfas.FirstOrDefault(rs => rs.IsTJ && (rs.IsEnrolled || rs.IsInProgress || rs.IsReferred)));
                        // TODO: .Let returns null if there are no RFAs and throws an exception. Figure out how to nest this so it only runs when rfas exist.

            Then()
                // "A new RFA for TJ cannot be created - TJ already exists".
                .Do(ctx => CreateMsg(messageCodeLevelResult));
        }

        private void CreateMsg(MessageCodeLevelContext messageCodeLevelResult)
        {
            const string code = Wwp.Model.Interface.Constants.RuleReason.TJTJ;
            var msg = Smart.Format(messageCodeLevelResult.GetMessegeByCode(code));
            messageCodeLevelResult.AddMessegeCodeAndLevel(msg, code, CodeLevel.Warning);
        }


        private void CreateMsg(MessageCodeLevelContext messageCodeLevelResult, IRequestForAssistance rfa)
        {
            const string code = Wwp.Model.Interface.Constants.RuleReason.TJTJ;
            var          msg  = Smart.Format(messageCodeLevelResult.GetMessegeByCode(code), rfa.RequestForAssistanceStatus.Name);
            messageCodeLevelResult.AddMessegeCodeAndLevel(msg, code, CodeLevel.Warning);
        }
    }
}