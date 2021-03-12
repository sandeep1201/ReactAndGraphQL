using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.Api.Library.Enums;
using Dcf.Wwp.Api.Library.Model.Api;
using Dcf.Wwp.Data.Sql.Model;
using Dcf.Wwp.Model.Interface;
using NRules.Fluent.Dsl;
using SmartFormat;

namespace Dcf.Wwp.Api.Library.Rules.PreTransferCheck
{
    [Tag("PreTransferCheck")]
    [Name("W2CFCoenrollmentInMke")]
    public class W2CFCoenrollmentInMke : Rule
    {
        //US1470
        public override void Define()
        {
            EnrolledProgramContract           transferContract       = null;
            List<IParticipantEnrolledProgram> peps                   = null;
            MessageCodeLevelContext           messageCodeLevelResult = null;

            // When we are transferring W-2 with CF co-enrolled in Milwaukee. US1474 blocks CF transfer so only way to transfer is with a W-2.
            When()              
                .Match<EnrolledProgramContract>(() => transferContract, tc => tc                                                                     != null && tc.IsW2)
                .Match<List<IParticipantEnrolledProgram>>(() => peps, pe => pe.FirstOrDefault(c => c.IsCF && (c.IsEnrolled || c.IsReferred) && c.IsInMilwaukee) != null)
                .Match<List<IParticipantEnrolledProgram>>(() => peps, pe => pe.FirstOrDefault(c => c.IsW2 && c.IsEnrolled && c.IsInMilwaukee) != null)
                .Match(() => messageCodeLevelResult,                    mCLR => mCLR                                                                 != null);

            Then()
                   // Warning of code TRANW2CFM - CF will be transferred with W-2. Please inform Children First Case Manager that participant has been transferred.
                .Do(ctx => CreateMsg(messageCodeLevelResult));
        }

        private void CreateMsg(MessageCodeLevelContext messageCodeLevelResult)
        {
            const string code = Wwp.Model.Interface.Constants.RuleReason.TRANW2CFM;

            var msg = messageCodeLevelResult.GetMessegeByCode(code);

            messageCodeLevelResult.AddMessegeCodeAndLevel(msg, code, CodeLevel.Warning);
        }
    }

}

