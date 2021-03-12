using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Dcf.Wwp.Api.Library.Model.Api;
using Dcf.Wwp.Data.Sql.Model;
using Dcf.Wwp.Model.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NRules;
using NRules.Fluent;

namespace Dcf.Wwp.ViewModel.UnitTests.Rules.DisEnrollment
{
    [TestClass]
    public class OpenAccommodationsWillBeClosedWarning : BaseUnitTest
    {
        [TestMethod]
        public void OpenAccommodationsWillBeClosedWarning_OneAccommodationOpenAndOneEnrolledProgram_ShowWarningAccommodationWillbeClosed()
        {
            // Assert.
            var peps = new List<IParticipantEnrolledProgram>();

            var w2 = new ParticipantEnrolledProgram() as IParticipantEnrolledProgram;
            w2.EnrolledProgramStatusCodeId = Wwp.Model.Interface.Constants.EnrolledProgramStatusCode.EnrolledId;
            w2.EnrolledProgram = new Data.Sql.Model.EnrolledProgram();
            w2.EnrolledProgram.ProgramCode = Wwp.Model.Interface.Constants.EnrolledProgram.W2ProgramCode;

            peps.Add(w2);

            var disenrollPep = new ParticipantEnrolledProgram() as IParticipantEnrolledProgram;
            disenrollPep.EnrolledProgramStatusCodeId = w2.EnrolledProgramStatusCodeId;
            disenrollPep.EnrolledProgram = new Data.Sql.Model.EnrolledProgram();
            disenrollPep.EnrolledProgram.ProgramCode = w2.EnrolledProgram.ProgramCode;

            var bds = new List<IBarrierDetail>();
            var bd = new BarrierDetail();
            bd.IsDeleted = false;
            bd.EndDate   = null;
            bd.IsAccommodationNeeded = true;
            var acc = new BarrierAccommodation();
            acc.DeleteReasonId = null;
            acc.EndDate = null;
            bd.BarrierAccommodations = new List<BarrierAccommodation>();
            bd.BarrierAccommodations.Add(acc);
            bds.Add(bd);


            var repository = new RuleRepository();
            repository.Load(x => x.From(Assembly.GetAssembly(typeof(Dcf.Wwp.Api.Library.ViewModels.ParticipantViewModel))).Where(rule => rule.Name.Equals("OpenAccommodationsWillBeClosedWarning")));


            var messageCodeLevelResult = new MessageCodeLevelContext
                                         {
                                             // Querying the database once for all the applicable rule reasons.
                                             PossibleRuleReasons = new RuleReasonTable().RuleReasons
                                         };

            var factory = repository.Compile();
            var session = factory.CreateSession();

            // Act.
            session.Insert(bds);
            session.Insert(messageCodeLevelResult);
            session.Insert(peps);
            session.Insert(disenrollPep);
            session.Fire();

            // Assert.
            Assert.IsTrue(messageCodeLevelResult.CodesAndMesssegesByLevel.Count == 1);
            Assert.IsTrue(messageCodeLevelResult.CodesAndMesssegesByLevel[0].Code == Dcf.Wwp.Model.Interface.Constants.RuleReason.DPCWA);
        }
    }
}
