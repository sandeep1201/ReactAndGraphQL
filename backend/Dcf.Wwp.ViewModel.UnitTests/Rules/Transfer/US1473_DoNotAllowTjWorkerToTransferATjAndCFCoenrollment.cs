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

namespace Dcf.Wwp.ViewModel.UnitTests.Rules.Transfer
{
    [TestClass]
    public class US1473_DoNotAllowTjWorkerToTransferATjAndCFCoenrollment
    {
        [TestMethod]
        public void CfEnrolledAndTransferringTj()
        {

            var peps = new List<IParticipantEnrolledProgram>();

            var cf                         = new ParticipantEnrolledProgram() as IParticipantEnrolledProgram;
            cf.EnrolledProgramStatusCodeId = Wwp.Model.Interface.Constants.EnrolledProgramStatusCode.EnrolledId;
            cf.EnrolledProgram             = new Data.Sql.Model.EnrolledProgram();
            cf.EnrolledProgram.ProgramCode = Wwp.Model.Interface.Constants.EnrolledProgram.CFProgramCode;
            cf.Office                      = new Office();
            cf.Office.Id                   = 13;
            peps.Add(cf);

            var transferContract                 = new EnrolledProgramContract();
            transferContract.EnrolledProgramId = Wwp.Model.Interface.Constants.EnrolledProgram.TransitionalJobsId;

            var repository = new RuleRepository();
            repository.Load(x => x.From(Assembly.GetAssembly(typeof(Dcf.Wwp.Api.Library.ViewModels.ParticipantViewModel))).Where(rule => rule.Name.Equals("NoTransferForTJWithCF")));

            var factory = repository.Compile();
            var session = factory.CreateSession();

            var messageCodeLevelResult = new MessageCodeLevelContext
                                         {
                                             // Querying the database once for all the applicable rule reasons.
                                             PossibleRuleReasons = new RuleReasonTable().RuleReasons
                                         };
            session.Insert(messageCodeLevelResult);
            session.Insert(peps);
            session.Insert(transferContract);
            session.Fire();

            Assert.IsTrue(messageCodeLevelResult.CodesAndMesssegesByLevel.Count == 1);
        }
    }
}
