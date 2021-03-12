using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.Api.Library.Model.Api;
using Dcf.Wwp.Data.Sql.Model;
using Dcf.Wwp.Model.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NRules;
using NRules.Fluent;

namespace Dcf.Wwp.ViewModel.UnitTests.Rules.RFA
{
    [TestClass]
    public class TMJCFOfficeMatch : BaseUnitTest
    {
        [TestMethod]
        public void RfaValidate_TmjEnrolledCfRfaNotMatchingTMJOffice_ShowErrorCFShouldBeInTheSameOffice()
        {

            var peps = new List<IParticipantEnrolledProgram>();

            var tmj = new ParticipantEnrolledProgram() as IParticipantEnrolledProgram;
            tmj.EnrolledProgramStatusCodeId = Wwp.Model.Interface.Constants.EnrolledProgramStatusCode.EnrolledId;
            tmj.EnrolledProgram = new Data.Sql.Model.EnrolledProgram();
            tmj.EnrolledProgram.ProgramCode = Wwp.Model.Interface.Constants.EnrolledProgram.TmjProgramCode;
            tmj.Office = new Office();
            tmj.Office.Id = 13;
            peps.Add(tmj);

            var rfa = new RequestForAssistanceContract();
            rfa.WorkProgramOfficeId = 11;

            var repository = new RuleRepository();
            repository.Load(x => x.From(Assembly.GetAssembly(typeof(Dcf.Wwp.Api.Library.ViewModels.ParticipantViewModel))).Where(rule => rule.Name.Equals("TMJCFOfficeMatch")));

            var factory = repository.Compile();
            var session = factory.CreateSession();

            var messageCodeLevelResult = new MessageCodeLevelContext
            {
                // Querying the database once for all the applicable rule reasons.
                PossibleRuleReasons = new RuleReasonTable().RuleReasons
            };
            session.Insert(messageCodeLevelResult);
            session.Insert(peps);
            session.Insert(rfa);
            session.Fire();

            Assert.IsTrue(messageCodeLevelResult.CodesAndMesssegesByLevel.Count == 1);
        }

        [TestMethod]
        public void RfaValidate_TmjEnrolledCfRfaIsMatchingTMJOffice_DontShowErrorCFShouldBeInTheSameOffice()
        {

            var peps = new List<IParticipantEnrolledProgram>();

            var tmj                         = new ParticipantEnrolledProgram() as IParticipantEnrolledProgram;
            tmj.EnrolledProgramStatusCodeId = Wwp.Model.Interface.Constants.EnrolledProgramStatusCode.EnrolledId;
            tmj.EnrolledProgram             = new Data.Sql.Model.EnrolledProgram();
            tmj.EnrolledProgram.ProgramCode = Wwp.Model.Interface.Constants.EnrolledProgram.TmjProgramCode;
            tmj.Office                      = new Office();
            tmj.Office.Id                   = 13;
            peps.Add(tmj);

            var rfa                 = new RequestForAssistanceContract();
            rfa.WorkProgramOfficeId = 13;

            var repository = new RuleRepository();
            repository.Load(x => x.From(Assembly.GetAssembly(typeof(Dcf.Wwp.Api.Library.ViewModels.ParticipantViewModel))).Where(rule => rule.Name.Equals("TMJCFOfficeMatch")));

            var factory = repository.Compile();
            var session = factory.CreateSession();

            var messageCodeLevelResult = new MessageCodeLevelContext
                                         {
                                             // Querying the database once for all the applicable rule reasons.
                                             PossibleRuleReasons = new RuleReasonTable().RuleReasons
                                         };
            session.Insert(messageCodeLevelResult);
            session.Insert(peps);
            session.Insert(rfa);
            session.Fire();

            Assert.IsTrue(messageCodeLevelResult.CodesAndMesssegesByLevel.Count == 0);
        }
    }
}
