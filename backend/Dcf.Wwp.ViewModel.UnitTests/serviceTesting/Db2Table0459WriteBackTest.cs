using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dcf.Wwp.Api.Library.Extensions;
using Dcf.Wwp.Data.Sql.Model;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Repository;
using Dcf.Wwp.Model.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Dcf.Wwp.Api.Library.Services;
using Shouldly;
using DCF.Core;
using DCF.Timelimits.Rules.Domain;
using DCF.Timelimts.Service;

namespace Dcf.Wwp.ViewModel.UnitTests.serviceTesting
{
    [TestClass]
    public class Db2Table0459WriteBackTest : BaseUnitTest
    {
        private const string Pin = "123456789";
        private const string User = "User1";
        private Mock<IRepository> mockRepository;
        private IRepository repo;
        private IDb2TimelimitService Table0459Service;
        private ITimelimitService _timelimitService;
        private Mock<ITimelimitService> mockTimelimitService;

        [TestInitialize]
        public void TestInitialize()
        {

            mockRepository = new Mock<IRepository>();
            mockRepository.Setup(x => x.AllTicksByPin(It.IsAny<Decimal>())).Returns(() => new List<IT0459_IN_W2_LIMITS>());
            mockRepository.Setup(x => x.NewTimeLimit()).Returns(new TimeLimit());
            mockRepository.Setup(x => x.NewT0459_IN_W2_LIMITS(It.IsAny<Boolean>())).Returns(() => new T0459_IN_W2_LIMITS());
            //mockRepository.Setup(x => x.NewT0459_IN_W2_LIMITS(false)).Returns(() => new T0459_IN_W2_LIMITS());
            mockRepository.Setup(x => x.NewT0460InW2Ext(It.IsAny<Boolean>())).Returns(() => new T0460_IN_W2_EXT());
            //mockRepository.Setup(x => x.NewT0460InW2Ext(false)).Returns(() => new T0460_IN_W2_EXT());
            mockRepository.Setup(x => x.WorkerLoginDetailsByWamsId(It.IsAny<String>()))
                .Returns(new WorkerLoginDetail() { WorkerId = "XCT123" });
            repo = mockRepository.Object;

            mockTimelimitService = new Mock<ITimelimitService>(MockBehavior.Loose);
            mockTimelimitService.Setup(x => x.GetTimelineExtensionSequences(It.IsAny<Decimal>())).Returns(() => new List<ExtensionSequence>());
            mockTimelimitService.Setup(x => x.GetTimelineMonths(It.IsAny<Decimal>())).Returns(() => new List<TimelineMonth>());
            _timelimitService = this.mockTimelimitService.Object;

            Table0459Service = new TimeLimitDb2Service(Pin, repo, _timelimitService);
        }

        [TestMethod]
        public void UpsertOneCsj()
        {

            ITimeLimit timeLimit = null;
            timeLimit = repo.NewTimeLimit();

            timeLimit.TimeLimitTypeId = (Int32)ClockTypes.CSJ;
            timeLimit.ChangeReason = new ChangeReason();

            timeLimit.EffectiveMonth = DateTime.Parse("01/1/2017");
            timeLimit.ChangeReason.Code = "";
            timeLimit.FederalTimeLimit = true;
            timeLimit.StateTimelimit = true;
            timeLimit.ChangeReason.Id = 1;
            timeLimit.TwentyFourMonthLimit = true;
            timeLimit.Notes = "Notes123";

            var db2Record = Table0459Service.UpsertTick(timeLimit, Pin, User);
            var retVal = this.Table0459Service.GetAllTicks();
            Assert.AreEqual(1, retVal.Count);
            Assert.AreEqual(201701m, db2Record.BENEFIT_MM);
            Assert.AreEqual((short)1, db2Record.HISTORY_SEQ_NUM);
            Assert.AreEqual("CSJ", db2Record.CLOCK_TYPE_CD);
            Assert.AreEqual("Y", db2Record.FED_CLOCK_IND);
            Assert.AreEqual((short)1, db2Record.FED_CMP_MTH_NUM);
            Assert.AreEqual((short)60, db2Record.FED_MAX_MTH_NUM);
            Assert.AreEqual((short)0, db2Record.HISTORY_CD);
            Assert.AreEqual("", db2Record.OVERRIDE_REASON_CD);
            Assert.AreEqual(1, db2Record.TOT_CMP_MTH_NUM);
            Assert.AreEqual(60, db2Record.TOT_MAX_MTH_NUM);
            Assert.AreEqual("XCT123", db2Record.USER_ID);
            Assert.AreEqual(DateTime.Now.ToString("yyyy-MM-dd"), db2Record.UPDATED_DT.ToString("yyyy-MM-dd"));
            Assert.AreEqual("Notes123", db2Record.COMMENT_TXT);
        }

        [TestMethod]
        public void UpsertTwoCsjs()
        {

            UpsertOneCsj();

            ITimeLimit timeLimit = null;
            timeLimit = repo.NewTimeLimit();

            timeLimit.TimeLimitTypeId = (Int32)ClockTypes.CSJ;
            timeLimit.ChangeReason = new ChangeReason();

            timeLimit.EffectiveMonth = DateTime.Parse("02/1/2017");
            timeLimit.ChangeReason.Code = "";
            timeLimit.FederalTimeLimit = true;
            timeLimit.StateTimelimit = true;
            timeLimit.ChangeReason.Id = 1;
            timeLimit.TwentyFourMonthLimit = true;
            timeLimit.Notes = "Notes123";

            var db2Record = Table0459Service.UpsertTick(timeLimit, Pin, User);
            var retVal = this.Table0459Service.GetAllTicks();

            Assert.AreEqual(2, retVal.Count);
            Assert.AreEqual(201702m, db2Record.BENEFIT_MM);
            Assert.AreEqual((short)1, db2Record.HISTORY_SEQ_NUM);
            Assert.AreEqual("CSJ", db2Record.CLOCK_TYPE_CD);
            Assert.AreEqual("Y", db2Record.FED_CLOCK_IND);
            Assert.AreEqual((short)2, db2Record.FED_CMP_MTH_NUM);
            Assert.AreEqual((short)60, db2Record.FED_MAX_MTH_NUM);
            Assert.AreEqual((short)0, db2Record.HISTORY_CD);
            Assert.AreEqual("", db2Record.OVERRIDE_REASON_CD);
            Assert.AreEqual(2, db2Record.TOT_CMP_MTH_NUM);
            Assert.AreEqual(60, db2Record.TOT_MAX_MTH_NUM);
            Assert.AreEqual("XCT123", db2Record.USER_ID);
            Assert.AreEqual(DateTime.Now.ToString("yyyy-MM-dd"), db2Record.UPDATED_DT.ToString("yyyy-MM-dd"));
            Assert.AreEqual("Notes123", db2Record.COMMENT_TXT);


        }

        [TestMethod]
        public void UpsertTwoCsjsThenDeleteFirstOne()
        {
            UpsertTwoCsjs();

            ITimeLimit timeLimit = null;
            timeLimit = repo.NewTimeLimit();

            timeLimit.TimeLimitTypeId = (Int32)ClockTypes.None;
            timeLimit.ChangeReason = new ChangeReason();

            timeLimit.EffectiveMonth = DateTime.Parse("01/1/2017");
            timeLimit.ChangeReason.Code = "";
            timeLimit.FederalTimeLimit = false;
            timeLimit.StateTimelimit = false;
            timeLimit.ChangeReason.Id = 1;
            timeLimit.TwentyFourMonthLimit = false;
            timeLimit.Notes = "Notes123";

            var db2Record = Table0459Service.UpsertTick(timeLimit, Pin, User);
            var retVal = this.Table0459Service.GetAllTicks();

            Assert.AreEqual(3, retVal.Count);
            var noneRecord = retVal.First(x => x.BENEFIT_MM == 201701m && x.HISTORY_SEQ_NUM == 1);
            noneRecord.ShouldNotBeNull();
            noneRecord.HISTORY_CD = 9;
        }

        [TestMethod]
        public void UpsertTwoCsjSecondInPast()
        {
            UpsertOneCsj();

            ITimeLimit timeLimit = null;
            timeLimit = repo.NewTimeLimit();

            timeLimit.TimeLimitTypeId = (Int32)ClockTypes.CSJ;
            timeLimit.ChangeReason = new ChangeReason();

            timeLimit.EffectiveMonth = DateTime.Parse("12/1/2016");
            timeLimit.ChangeReason.Code = "";
            timeLimit.FederalTimeLimit = true;
            timeLimit.StateTimelimit = true;
            timeLimit.ChangeReason.Id = 1;
            timeLimit.TwentyFourMonthLimit = true;
            timeLimit.Notes = "Notes2";

            var db2Record = Table0459Service.UpsertTick(timeLimit, Pin, User);
            var retVal = this.Table0459Service.GetAllTicks();

            var pastRecord = retVal.First(x => x.BENEFIT_MM == 201612m && x.HISTORY_CD == 0 && x.HISTORY_SEQ_NUM == 1);
            var newestRecord = retVal.First(x => x.BENEFIT_MM == 201701m && x.HISTORY_CD == 0 && x.HISTORY_SEQ_NUM == 2);

            Assert.AreEqual(3, retVal.Count);
            Assert.AreEqual(201612m, pastRecord.BENEFIT_MM);
            Assert.AreEqual(2, newestRecord.FED_CMP_MTH_NUM);
            //Assert.AreEqual((short)1, db2Record.HISTORY_SEQ_NUM);
            //Assert.AreEqual("CSJ", db2Record.CLOCK_TYPE_CD);
            //Assert.AreEqual("Y", db2Record.FED_CLOCK_IND);
            //Assert.AreEqual((short)2, db2Record.FED_CMP_MTH_NUM);
            //Assert.AreEqual((short)60, db2Record.FED_MAX_MTH_NUM);
            //Assert.AreEqual((short)0, db2Record.HISTORY_CD);
            //Assert.AreEqual("", db2Record.OVERRIDE_REASON_CD);
            //Assert.AreEqual(2, db2Record.TOT_CMP_MTH_NUM);
            //Assert.AreEqual(60, db2Record.TOT_MAX_MTH_NUM);
            //Assert.AreEqual("XCT123", db2Record.USER_ID);
            //Assert.AreEqual(DateTime.Now.ToString("yyyy-MM-dd"), db2Record.UPDATED_DT.ToString("yyyy-MM-dd"));
            //Assert.AreEqual("Notes123", db2Record.COMMENT_TXT);

            Assert.AreEqual(2, newestRecord.TOT_CMP_MTH_NUM);
            Assert.AreEqual(2, newestRecord.TOT_CMP_MTH_NUM);
        }


        [TestMethod]
        public void UpsertThreeCsjsThirdDoesntTickFed()
        {
            UpsertTwoCsjs();

            ITimeLimit timeLimit = null;
            timeLimit = repo.NewTimeLimit();

            timeLimit.TimeLimitTypeId = (Int32)ClockTypes.CSJ;
            timeLimit.ChangeReason = new ChangeReason();

            timeLimit.EffectiveMonth = DateTime.Parse("03/1/2017");
            timeLimit.ChangeReason.Code = "";
            timeLimit.FederalTimeLimit = false;
            timeLimit.StateTimelimit = true;
            timeLimit.ChangeReason.Id = 1;
            timeLimit.TwentyFourMonthLimit = true;
            timeLimit.Notes = "Notes123";

            var db2Record = Table0459Service.UpsertTick(timeLimit, Pin, User);
            var retVal = this.Table0459Service.GetAllTicks();

            Assert.AreEqual(3, retVal.Count);
            Assert.AreEqual(201703m, db2Record.BENEFIT_MM);
            Assert.AreEqual((short)1, db2Record.HISTORY_SEQ_NUM);
            Assert.AreEqual("CSJ", db2Record.CLOCK_TYPE_CD);
            Assert.AreEqual("N", db2Record.FED_CLOCK_IND);
            Assert.AreEqual((short)2, db2Record.FED_CMP_MTH_NUM);
            Assert.AreEqual((short)60, db2Record.FED_MAX_MTH_NUM);
            Assert.AreEqual((short)0, db2Record.HISTORY_CD);
            Assert.AreEqual("", db2Record.OVERRIDE_REASON_CD);
            Assert.AreEqual(3, db2Record.TOT_CMP_MTH_NUM);
            Assert.AreEqual(60, db2Record.TOT_MAX_MTH_NUM);
            Assert.AreEqual("XCT123", db2Record.USER_ID);
            Assert.AreEqual(DateTime.Now.ToString("yyyy-MM-dd"), db2Record.UPDATED_DT.ToString("yyyy-MM-dd"));
            Assert.AreEqual("Notes123", db2Record.COMMENT_TXT);
        }

        [TestMethod]
        public void UpsertThreeCsjsFourthDoesntTickFedWithCMC()
        {
            UpsertThreeCsjsThirdDoesntTickFed();

            ITimeLimit timeLimit = null;
            timeLimit = repo.NewTimeLimit();

            timeLimit.TimeLimitTypeId = (Int32)ClockTypes.CMC;
            timeLimit.ChangeReason = new ChangeReason();

            timeLimit.EffectiveMonth = DateTime.Parse("04/1/2017");
            timeLimit.ChangeReason.Code = "";
            timeLimit.FederalTimeLimit = true;
            timeLimit.StateTimelimit = true;
            timeLimit.ChangeReason.Id = 1;
            timeLimit.TwentyFourMonthLimit = false;
            timeLimit.Notes = "Notes123";

            var db2Record = Table0459Service.UpsertTick(timeLimit, Pin, User);
            var retVal = this.Table0459Service.GetAllTicks();
            Assert.AreEqual(4, retVal.Count(x=>x.HISTORY_CD == 0));
            Assert.AreEqual(201704m, db2Record.BENEFIT_MM);
            Assert.AreEqual((short)1, db2Record.HISTORY_SEQ_NUM);
            Assert.AreEqual("CMC", db2Record.CLOCK_TYPE_CD);
            Assert.AreEqual("Y", db2Record.FED_CLOCK_IND);
            Assert.AreEqual((short)3, db2Record.FED_CMP_MTH_NUM);
            Assert.AreEqual((short)60, db2Record.FED_MAX_MTH_NUM);
            Assert.AreEqual((short)0, db2Record.HISTORY_CD);
            Assert.AreEqual("", db2Record.OVERRIDE_REASON_CD);
            Assert.AreEqual(4, db2Record.TOT_CMP_MTH_NUM);
            Assert.AreEqual(60, db2Record.TOT_MAX_MTH_NUM);
            Assert.AreEqual(0, db2Record.WW_MAX_MTH_NUM);
            Assert.AreEqual(3, db2Record.WW_CMP_MTH_NUM);
            Assert.AreEqual("XCT123", db2Record.USER_ID);
            Assert.AreEqual(DateTime.Now.ToString("yyyy-MM-dd"), db2Record.UPDATED_DT.ToString("yyyy-MM-dd"));
            Assert.AreEqual("Notes123", db2Record.COMMENT_TXT);
        }

        [TestMethod]
        public void UpsertThreeCsjsThirdDoesntTickFedWithCMCThenGoBackAndUpdateFirstCSJToNonFed()
        {
            UpsertThreeCsjsFourthDoesntTickFedWithCMC();

            ITimeLimit timeLimit = null;
            timeLimit = repo.NewTimeLimit();

            timeLimit.TimeLimitTypeId = (Int32)ClockTypes.CSJ;
            timeLimit.ChangeReason = new ChangeReason();

            timeLimit.EffectiveMonth = DateTime.Parse("01/1/2017");
            timeLimit.ChangeReason.Code = "";
            timeLimit.FederalTimeLimit = false;
            timeLimit.StateTimelimit = true;
            timeLimit.ChangeReason.Id = 1;
            timeLimit.TwentyFourMonthLimit = true;
            timeLimit.Notes = "Notes123";

            var db2Record = Table0459Service.UpsertTick(timeLimit, Pin, User);
            var retVal = this.Table0459Service.GetAllTicks();

            // All.
            Assert.AreEqual(8, retVal.Count);
            Assert.AreEqual(2, retVal.Count(x => x.BENEFIT_MM == 201701m));

            // New.
            Assert.AreEqual(1, retVal.Count(x => x.BENEFIT_MM == 201701m && x.HISTORY_CD == 0 && x.HISTORY_SEQ_NUM == 2));
            Assert.AreEqual(0,
                retVal.First(x => x.BENEFIT_MM == 201701m && x.HISTORY_CD == 0 && x.HISTORY_SEQ_NUM == 2)
                    .FED_CMP_MTH_NUM);

            Assert.AreEqual(1,
                retVal.First(x => x.BENEFIT_MM == 201701m && x.HISTORY_CD == 0 && x.HISTORY_SEQ_NUM == 2)
                    .TOT_CMP_MTH_NUM);

            Assert.AreEqual("N",
                retVal.First(x => x.BENEFIT_MM == 201701m && x.HISTORY_CD == 0 && x.HISTORY_SEQ_NUM == 2).FED_CLOCK_IND);

            Assert.AreEqual(1,
                retVal.First(x => x.BENEFIT_MM == 201702m && x.HISTORY_CD == 0 && x.HISTORY_SEQ_NUM == 2)
                    .FED_CMP_MTH_NUM);
            Assert.AreEqual(60,
                retVal.First(x => x.BENEFIT_MM == 201702m && x.HISTORY_CD == 0 && x.HISTORY_SEQ_NUM == 2)
                    .TOT_MAX_MTH_NUM);
            Assert.AreEqual(2,
                retVal.First(x => x.BENEFIT_MM == 201702m && x.HISTORY_CD == 0 && x.HISTORY_SEQ_NUM == 2)
                    .TOT_CMP_MTH_NUM);
            Assert.AreEqual("Y",
                retVal.First(x => x.BENEFIT_MM == 201702m && x.HISTORY_CD == 0 && x.HISTORY_SEQ_NUM == 2).FED_CLOCK_IND);

            Assert.AreEqual(3,
                retVal.First(x => x.BENEFIT_MM == 201703m && x.HISTORY_CD == 0 && x.HISTORY_SEQ_NUM == 2)
                    .TOT_CMP_MTH_NUM);

            // Old.
            Assert.AreEqual(1,
                retVal.First(x => x.BENEFIT_MM == 201701m && x.HISTORY_CD == 9 && x.HISTORY_SEQ_NUM == 1)
                    .FED_CMP_MTH_NUM);
            Assert.AreEqual(1,
                retVal.First(x => x.BENEFIT_MM == 201701m && x.HISTORY_CD == 9 && x.HISTORY_SEQ_NUM == 1)
                    .TOT_CMP_MTH_NUM);




            //Assert.AreEqual(db2Record.BENEFIT_MM, 201704m);
            //Assert.AreEqual(db2Record.HISTORY_SEQ_NUM, (short)1);
            //Assert.AreEqual(db2Record.CLOCK_TYPE_CD, "CMC");
            //Assert.AreEqual(db2Record.FED_CLOCK_IND, "Y");
            //Assert.AreEqual(db2Record.FED_CMP_MTH_NUM, (short)3);
            //Assert.AreEqual(db2Record.FED_MAX_MTH_NUM, (short)60);
            //Assert.AreEqual(db2Record.HISTORY_CD, (short)0);
            //Assert.AreEqual(db2Record.OVERRIDE_REASON_CD, "1");
            //Assert.AreEqual(db2Record.TOT_CMP_MTH_NUM, 4);
            //Assert.AreEqual(db2Record.TOT_MAX_MTH_NUM, 60);
            //Assert.AreEqual(db2Record.WW_MAX_MTH_NUM, 24);
            //Assert.AreEqual(db2Record.WW_CMP_MTH_NUM, 3);
            //Assert.AreEqual(db2Record.USER_ID, "XCT123");
            //Assert.AreEqual(db2Record.UPDATED_DT.ToString("yyyy-MM-dd"), DateTime.Now.ToString("yyyy-MM-dd"));
            //Assert.AreEqual(db2Record.COMMENT_TXT, "Notes123");
        }

        [TestMethod]
        public void UpsertOneExtension()
        {
            ITimeLimitExtension extensionModel = null;
            extensionModel = new TimeLimitExtension();

            extensionModel.TimeLimitTypeId = (Int32)ClockTypes.CSJ;
            extensionModel.ExtensionSequence = 1;
            extensionModel.ExtensionDecisionId = 1;
            extensionModel.ApprovalReason = new ApprovalReason()
            {
                Code = "apr"
            };
            extensionModel.DecisionDate = DateTime.Parse("01/1/2017");
            extensionModel.BeginMonth = DateTime.Parse("01/1/2017");
            extensionModel.EndMonth = DateTime.Parse("02/1/2017");
            extensionModel.ApprovalReasonId = 1;

            var db2Record = Table0459Service.UpsertExtension(extensionModel, Pin, User);
            var retVal = this.Table0459Service.GetAllExtensions();

            Assert.AreEqual(1, retVal.Count);
            Assert.AreEqual(Convert.ToDecimal(DateTime.Now.ToStringMonthYearComposite()), db2Record.BENEFIT_MM);
            Assert.AreEqual(201701, db2Record.EXT_BEG_MM);
            Assert.AreEqual(201702, db2Record.EXT_END_MM);
            Assert.AreEqual("XCT123", db2Record.USER_ID);
            Assert.AreEqual(0, db2Record.HISTORY_CD);
            Assert.AreEqual(1, db2Record.HISTORY_SEQ_NUM); 
            Assert.AreEqual(1, db2Record.EXT_SEQ_NUM);
            //Assert.AreEqual("", db2Record.STA_DCSN_CD);
        }

        [TestMethod]
        public void UpsertOneExtensionThenAnother()
        {
            UpsertOneExtension();

            ITimeLimitExtension extensionModel = null;
            extensionModel = new TimeLimitExtension();

            extensionModel.TimeLimitTypeId = (Int32)ClockTypes.CSJ;

            extensionModel.ExtensionSequence = 1;
            extensionModel.ExtensionDecisionId = 1;
            extensionModel.ApprovalReason = new ApprovalReason()
            {
                Code = "apr"
            };
            extensionModel.DecisionDate = DateTime.Parse("03/1/2018");
            extensionModel.BeginMonth = DateTime.Parse("03/1/2018");
            extensionModel.EndMonth = DateTime.Parse("04/1/2018");
            extensionModel.ApprovalReasonId = 1;

            var db2Record = Table0459Service.UpsertExtension(extensionModel, Pin, User);
            var retVal = this.Table0459Service.GetAllExtensions();

            Assert.AreEqual(2, retVal.Count);
            Assert.AreEqual(Convert.ToDecimal(DateTime.Now.ToStringMonthYearComposite()), db2Record.BENEFIT_MM);
            Assert.AreEqual(201803, db2Record.EXT_BEG_MM);
            Assert.AreEqual(201804, db2Record.EXT_END_MM);
            Assert.AreEqual("XCT123", db2Record.USER_ID);
            Assert.AreEqual(9, db2Record.HISTORY_CD);
            Assert.AreEqual(0, db2Record.HISTORY_CD);
            Assert.AreEqual(1, db2Record.HISTORY_SEQ_NUM);
            Assert.AreEqual(1, db2Record.HISTORY_SEQ_NUM);
            Assert.AreEqual(2, db2Record.EXT_SEQ_NUM);
            //Assert.AreEqual(201702, db2Record.EXT_END_MM);
            //Assert.AreEqual("", db2Record.STA_DCSN_CD);
        }



        [TestMethod]
        public void UpsertOneExtensionThenAnotherThenChangeClockType()
        {
            UpsertOneExtensionThenAnother();

            ITimeLimitExtension extensionModel = null;
            extensionModel = new TimeLimitExtension();


            extensionModel.TimeLimitTypeId = (Int32)ClockTypes.W2T;
            extensionModel.ExtensionSequence = 1;
            extensionModel.ExtensionDecisionId = 1;
            extensionModel.ApprovalReason = new ApprovalReason()
            {
                Code = "apr"
            };
            extensionModel.DecisionDate = DateTime.Parse("03/1/2019");
            extensionModel.BeginMonth = DateTime.Parse("03/1/2019");
            extensionModel.EndMonth = DateTime.Parse("04/1/2019");
            extensionModel.ApprovalReasonId = 1;

            var db2Record = Table0459Service.UpsertExtension(extensionModel, Pin, User);
            var retVal = this.Table0459Service.GetAllExtensions();

            Assert.AreEqual(3, retVal.Count);

            Assert.AreEqual(9, db2Record.HISTORY_CD);
            Assert.AreEqual(0, db2Record.HISTORY_CD);
            Assert.AreEqual(0, db2Record.HISTORY_CD);

            Assert.AreEqual(1, db2Record.HISTORY_SEQ_NUM);
            Assert.AreEqual(1, db2Record.HISTORY_SEQ_NUM);
            Assert.AreEqual(1, db2Record.HISTORY_SEQ_NUM);

            Assert.AreEqual(1, db2Record.EXT_SEQ_NUM);
            Assert.AreEqual(2, db2Record.EXT_SEQ_NUM);
            Assert.AreEqual(1, db2Record.EXT_SEQ_NUM);

        }

        [TestMethod]
        public void UpsertOneExtensionThenAnotherThenChangeClockTypeThenDeleteFirst()
        {
            UpsertOneExtensionThenAnotherThenChangeClockType();
            ITimeLimitExtension extensionModel = null;
            extensionModel = new TimeLimitExtension();
            extensionModel.TimeLimitTypeId = (Int32)ClockTypes.CSJ;
            extensionModel.ExtensionSequence = 1;
            extensionModel.ExtensionDecisionId = 1;
            extensionModel.ApprovalReason = new ApprovalReason()
            {
                Code = "apr"
            };
            extensionModel.DecisionDate = DateTime.Parse("01/1/2017");
            extensionModel.BeginMonth = DateTime.Parse("01/1/2017");
            extensionModel.EndMonth = DateTime.Parse("02/1/2017");
            extensionModel.ApprovalReasonId = 1;

            extensionModel.IsDeleted = true;

            var retVal = Table0459Service.UpsertExtension(extensionModel, Pin, User);
        }

        [TestMethod]
        public void Update_existing_tick_should_not_increment_state_counter()
        {
            ITimeLimit timeLimit = null;
            timeLimit = repo.NewTimeLimit();

            timeLimit.TimeLimitTypeId = (Int32)ClockTypes.CSJ;
            timeLimit.ChangeReason = new ChangeReason();

            timeLimit.EffectiveMonth = DateTime.Parse("01/1/2017");
            timeLimit.ChangeReason.Code = "";
            timeLimit.FederalTimeLimit = true;
            timeLimit.StateTimelimit = true;
            timeLimit.ChangeReason.Id = 1;
            timeLimit.TwentyFourMonthLimit = true;
            timeLimit.Notes = "Notes123";

            IT0459_IN_W2_LIMITS db2Record;
            //Create a few "Old" ticks
            timeLimit.EffectiveMonth = timeLimit.EffectiveMonth.Value.AddMonths(-1);
            db2Record = this.Table0459Service.UpsertTick(timeLimit, Db2Table0459WriteBackTest.Pin, Db2Table0459WriteBackTest.User);
            timeLimit.EffectiveMonth = timeLimit.EffectiveMonth.Value.AddMonths(-1);
            db2Record = Table0459Service.UpsertTick(timeLimit, Pin, User);
            timeLimit.EffectiveMonth = timeLimit.EffectiveMonth.Value.AddMonths(-1);
            db2Record = Table0459Service.UpsertTick(timeLimit, Pin, User);

            timeLimit.EffectiveMonth = new DateTime(2017,1,1);
            db2Record = Table0459Service.UpsertTick(timeLimit, Pin, User);

            var retVal = this.Table0459Service.GetAllTicks();
            retVal.ShouldNotBeEmpty();
            retVal.Count(x => x.HISTORY_CD == 0).ShouldBe(4);

            db2Record = retVal.Where(x => x.CLOCK_TYPE_CD =="CSJ" && x.HISTORY_CD == 0).GetMax(c=>c.BENEFIT_MM);
            db2Record.ShouldNotBeNull();
            db2Record.TOT_CMP_MTH_NUM.ShouldBe((short)4 );
            db2Record.TOT_MAX_MTH_NUM.ShouldBe((short)60);
            db2Record.FED_CMP_MTH_NUM.ShouldBe((short)4);

            timeLimit.FederalTimeLimit = false;
            db2Record = this.Table0459Service.UpsertTick(timeLimit, Db2Table0459WriteBackTest.Pin, Db2Table0459WriteBackTest.User);
            retVal = this.Table0459Service.GetAllTicks();
            retVal.ShouldNotBeEmpty();
            retVal.Count(x => x.HISTORY_CD == 0).ShouldBe(4);

            db2Record = retVal.Where(x => x.CLOCK_TYPE_CD == "CSJ" && x.HISTORY_CD == 0).GetMax(c => c.BENEFIT_MM);

            db2Record.ShouldNotBeNull();
            db2Record.TOT_CMP_MTH_NUM.ShouldBe((short)4);
            db2Record.TOT_MAX_MTH_NUM.ShouldBe((short)60);
            db2Record.FED_CMP_MTH_NUM.ShouldBe((short)3);
        }
    }
}





