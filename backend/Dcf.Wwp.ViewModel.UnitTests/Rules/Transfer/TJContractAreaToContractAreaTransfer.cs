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
    public class TJContractAreaToContractAreaTransfer : BaseUnitTest
    {
        [TestMethod]
        public void TransferTest()
        {

            //var peps = new List<IParticipantEnrolledProgram>();

            //var w2 = new ParticipantEnrolledProgram() as IParticipantEnrolledProgram;
            //w2.EnrolledProgramStatusCodeId = Wwp.Model.Interface.Constants.EnrolledProgramStatusCode.EnrolledId;
            //w2.EnrolledProgram = new Data.Sql.Model.EnrolledProgram();
            //w2.EnrolledProgram.ProgramCode = Wwp.Model.Interface.Constants.EnrolledProgram.W2ProgramCode;

            //var cf = new ParticipantEnrolledProgram() as IParticipantEnrolledProgram;
            //cf.EnrolledProgramStatusCodeId = Wwp.Model.Interface.Constants.EnrolledProgramStatusCode.ReferredId;
            //cf.EnrolledProgram = new Data.Sql.Model.EnrolledProgram();
            //cf.EnrolledProgram.ProgramCode = Wwp.Model.Interface.Constants.EnrolledProgram.CFProgramCode;

            //peps.Add(w2);
            //peps.Add(cf);

            //var disenrollPep = new ParticipantEnrolledProgram() as IParticipantEnrolledProgram;
            //disenrollPep.EnrolledProgramStatusCodeId = w2.EnrolledProgramStatusCodeId;
            //disenrollPep.EnrolledProgram = new Data.Sql.Model.EnrolledProgram();
            //disenrollPep.EnrolledProgram.ProgramCode = w2.EnrolledProgram.ProgramCode;


            var destinationContract = new EnrolledProgramContract();
            IContractArea orignContractArea = new ContractArea();
            orignContractArea.Id = 15;
            destinationContract.EnrolledProgramId = Dcf.Wwp.Model.Interface.Constants.EnrolledProgram.TransitionalJobsId;


            var repository = new RuleRepository();
            repository.Load(x => x.From(Assembly.GetAssembly(typeof(Dcf.Wwp.Api.Library.ViewModels.ParticipantViewModel))).Where(rule => rule.Name.Equals("TJTransfer")));

            var factory = repository.Compile();
            var session = factory.CreateSession();

            var messageCodeLevelResult = new MessageCodeLevelContext
            {
                // Querying the database once for all the applicable rule reasons.
                PossibleRuleReasons = new RuleReasonTable().RuleReasons
            };

            session.Insert(messageCodeLevelResult);
            session.Insert(destinationContract);
            session.Insert(orignContractArea);
            session.Fire();

            Assert.IsTrue(messageCodeLevelResult.CodesAndMesssegesByLevel.Count == 1);
        }
    }
}
