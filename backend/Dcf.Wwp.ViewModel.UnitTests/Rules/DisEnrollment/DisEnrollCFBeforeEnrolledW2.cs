using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Dcf.Wwp.Api.Library.Enums;
using Dcf.Wwp.Api.Library.Model.Api;
using Dcf.Wwp.Data.Sql.Model;
using Dcf.Wwp.Model.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NRules;
using NRules.Fluent;


namespace Dcf.Wwp.ViewModel.UnitTests.Rules.DisEnrollment
{


    [TestClass]
    public class DisEnrollCFBeforeEnrolledW2 : BaseUnitTest
    {
        [TestMethod]
        public void DisEnrollCFBeforeEnrolledW2_DisEnrollCFEnrolledWhenW2Enrolled_ShowNoErrorsOrWarningsAllowCFDisenrollment()
        {

            var peps = new List<IParticipantEnrolledProgram>();

            var w2                         = new ParticipantEnrolledProgram() as IParticipantEnrolledProgram;
            w2.EnrolledProgramStatusCodeId = Wwp.Model.Interface.Constants.EnrolledProgramStatusCode.EnrolledId;
            w2.EnrolledProgram             = new Data.Sql.Model.EnrolledProgram();
            w2.EnrolledProgram.ProgramCode = Wwp.Model.Interface.Constants.EnrolledProgram.W2ProgramCode;

            var cf                         = new ParticipantEnrolledProgram() as IParticipantEnrolledProgram;
            cf.EnrolledProgramStatusCodeId = Wwp.Model.Interface.Constants.EnrolledProgramStatusCode.EnrolledId;
            cf.EnrolledProgram             = new Data.Sql.Model.EnrolledProgram();
            cf.EnrolledProgram.ProgramCode = Wwp.Model.Interface.Constants.EnrolledProgram.CFProgramCode;

            peps.Add(w2);
            peps.Add(cf);

            var disenrollPep                         = new ParticipantEnrolledProgram() as IParticipantEnrolledProgram;
            disenrollPep.EnrolledProgramStatusCodeId = cf.EnrolledProgramStatusCodeId;
            disenrollPep.EnrolledProgram             = new Data.Sql.Model.EnrolledProgram();
            disenrollPep.EnrolledProgram.ProgramCode = cf.EnrolledProgram.ProgramCode;

            var repository = new RuleRepository();
            repository.Load(x => x.From(Assembly.GetAssembly(typeof(Dcf.Wwp.Api.Library.ViewModels.ParticipantViewModel))).Where(rule => rule.IsTagged("Disenrollment")));

            var factory = repository.Compile();
            var session = factory.CreateSession();

            var messageCodeLevelResult = new MessageCodeLevelContext
                                         {
                                             // Querying the database once for all the applicable rule reasons.
                                             PossibleRuleReasons = new RuleReasonTable().RuleReasons
                                         };
            session.Insert(messageCodeLevelResult);
            session.Insert(peps);
            session.Insert(disenrollPep);
            session.Fire();

            Assert.IsTrue(messageCodeLevelResult.CodesAndMesssegesByLevel.Count == 0);
        }


        [TestMethod]
        public void DisEnrollCFBeforeEnrolledW2_DisEnrollCFEnrolledWhenW2EnrolledWithOpenActicity_ShowNoErrorsButAWarningAllowCFDisenrollment()
        {

            // Arrange.
            var peps = new List<IParticipantEnrolledProgram>();

            var w2 = new ParticipantEnrolledProgram() as IParticipantEnrolledProgram;
            w2.EnrolledProgramStatusCodeId = Wwp.Model.Interface.Constants.EnrolledProgramStatusCode.EnrolledId;
            w2.EnrolledProgram = new Data.Sql.Model.EnrolledProgram();
            w2.EnrolledProgram.ProgramCode = Wwp.Model.Interface.Constants.EnrolledProgram.W2ProgramCode;

            var cf = new ParticipantEnrolledProgram() as IParticipantEnrolledProgram;
            cf.EnrolledProgramStatusCodeId = Wwp.Model.Interface.Constants.EnrolledProgramStatusCode.EnrolledId;
            cf.EnrolledProgram = new Data.Sql.Model.EnrolledProgram();
            cf.EnrolledProgram.ProgramCode = Wwp.Model.Interface.Constants.EnrolledProgram.CFProgramCode;

            peps.Add(w2);
            peps.Add(cf);

            var preCheck = new SP_PreCheckDisenrollment_Result();
            preCheck.ActivityOpen = true;

            var disenrollPep = new ParticipantEnrolledProgram() as IParticipantEnrolledProgram;
            disenrollPep.EnrolledProgramStatusCodeId = cf.EnrolledProgramStatusCodeId;
            disenrollPep.EnrolledProgram = new Data.Sql.Model.EnrolledProgram();
            disenrollPep.EnrolledProgram.ProgramCode = cf.EnrolledProgram.ProgramCode;

            var repository = new RuleRepository();
            repository.Load(x => x.From(Assembly.GetAssembly(typeof(Dcf.Wwp.Api.Library.ViewModels.ParticipantViewModel))).Where(rule => rule.IsTagged("Disenrollment")));
     
            var messageCodeLevelResult = new MessageCodeLevelContext
            {
                // Querying the database once for all the applicable rule reasons.
                PossibleRuleReasons = new RuleReasonTable().RuleReasons
            };

            // Act.
            var factory = repository.Compile();
            var session = factory.CreateSession();
            session.Insert(preCheck);
            session.Insert(messageCodeLevelResult);
            session.Insert(peps);
            session.Insert(disenrollPep);
            session.Fire();

            // Assert.
            Assert.IsTrue(messageCodeLevelResult.CodesAndMesssegesByLevel.Count == 1);
            Assert.IsTrue(messageCodeLevelResult.CodesAndMesssegesByLevel.Count(x => x.Level == CodeLevel.Warning) == 1);

            Assert.IsTrue(messageCodeLevelResult.CodesAndMesssegesByLevel.Count(x => x.Message == "There are open activities for this participant. Close program specific activities prior to disenrollment.") == 1);
        }
    }
}
