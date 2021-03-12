using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using Dcf.Wwp.Api.Library.Extensions;
using Dcf.Wwp.Api.Library.Services;
using Dcf.Wwp.Data.Sql.Model;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Repository;
using DCF.Common.Dates;
using DCF.Common.Extensions;
using DCF.Timelimits.Rules.Domain;
using DCF.Timelimts.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.EntityFramework;
using Shouldly;

namespace DCF.Timelimits.Service
{
    [TestClass]
    public class Db2TimelimitServiceUpdateTests
    {
        [TestInitialize]
        public void TestInit()
        {
            this._moqContext = DbContextMockFactory.Create<WwpEntities>();
            this._moqContext = new Mock<WwpEntities>();
            this.dbContextFactory = () => this._moqContext.Object;
            this._repoFactory = () =>
            {
                var mockRepo = new Mock<Repository>(this.dbContextFactory());
                mockRepo.CallBase = true;
                return mockRepo.Object;
            };
        }

        private List<TimeLimit> timelmits = new List<TimeLimit>();
        private List<TimeLimitExtension> extensions = new List<TimeLimitExtension>();
        private List<T0459_IN_W2_LIMITS> _legacyTicks = new List<T0459_IN_W2_LIMITS>();
        private List<T0460_IN_W2_EXT> _legacyExtensions = new List<T0460_IN_W2_EXT>();

        private Mock<WwpEntities> _moqContext;
        private Func<WwpEntities> dbContextFactory;

        public Participant testParticipant = new Participant() { Id = 1, PinNumber = 1111, FirstName = "Test", LastName = "UnitTestUser" };
        private DateTime? _emptyMonth;
        private DateTime? _emptyCSJMonth;
        private DateTime? _emptyW2TMonth;
        private Func<Repository> _repoFactory;

        private void setup1()
        {

            var setupService = new Db2TimelimitService(null, null);
            setupService.IsSimulated = true;

            var last = new T0459_IN_W2_LIMITS();
            var effMonth = DateTime.Now.AddMonths(-63).StartOf(DateTimeUnit.Month);
            var id = 1;

            #region  add first 24 CSJ's
            for (var i = 1; i <= 24; i++)
            {
                effMonth = effMonth.AddMonths(1);
                //var clockType = i < 44 ? ClockTypes.CSJ : i > 50 && i<55 ? ClockTypes.TMP : ClockTypes.W2T;

                var tick = new T0459_IN_W2_LIMITS()
                {
                    BENEFIT_MM = Decimal.Parse(effMonth.ToStringMonthYearComposite()),
                    CLOCK_TYPE_CD = "CSJ",
                    FED_CLOCK_IND = "Y",
                    HISTORY_CD = 0,
                    COMMENT_TXT = "Test Data",
                    CRE_TRAN_CD = "WWPTest",
                    FED_MAX_MTH_NUM = 60,
                    TOT_MAX_MTH_NUM = 60,
                    WW_MAX_MTH_NUM = 24,
                    HISTORY_SEQ_NUM = 1,
                    Id = id++,
                    PIN_NUM = testParticipant.PinNumber.Value,
                };

                tick.FED_CMP_MTH_NUM = (Int16)(last.FED_CMP_MTH_NUM + 1);
                tick.TOT_CMP_MTH_NUM = (Int16)(last.OT_CMP_MTH_NUM + 1);
                tick.WW_CMP_MTH_NUM = (Int16)(last.WW_CMP_MTH_NUM + 1);

                var timelimit = new TimeLimit()
                {
                    TimeLimitTypeId = (Int32)ClockTypes.CSJ,
                    CreatedDate = effMonth.AddDays(-1),
                    Participant = this.testParticipant,
                    ParticipantID = this.testParticipant.Id,
                    FederalTimeLimit = tick.FED_CLOCK_IND == "Y",
                    EffectiveMonth = effMonth,
                    StateTimelimit = true,
                    TwentyFourMonthLimit = true

                };
                this.timelmits.Add(timelimit);
                this._legacyTicks.Add(tick);

                last = tick;
            }
            #endregion

            #region Add CSJ (12-month) Extension

            var extension = new TimeLimitExtension()
            {
                TimeLimitTypeId = (Int16)ClockTypes.CSJ,
                BeginMonth = effMonth.AddMonths(1),
                EndMonth = effMonth.AddMonths(13),
                CreatedDate = effMonth.AddDays(3),
                DecisionDate = effMonth.AddDays(3),
                Participant = this.testParticipant,
                ParticipantId = this.testParticipant.Id,
                ApprovalReasonId = 1,
                ApprovalReason = new ApprovalReason() { Code = "COO", Id = 1 },
                ExtensionDecisionId = (Int32)Rules.Domain.ExtensionDecision.Approve
            };
            var t0460 = new T0460_IN_W2_EXT();
            t0460.PIN_NUM = extension.Participant.PinNumber.Value;
            t0460.HISTORY_CD = 0;
            t0460.UPDATED_DT = DateTime.Now;
            t0460.CLOCK_TYPE_CD = "CSJ";
            t0460.AGY_DCSN_DT = DateTime.Now;
            t0460.BENEFIT_MM = last.BENEFIT_MM;
            t0460.EXT_SEQ_NUM = 1;
            t0460.HISTORY_SEQ_NUM = 1;
            t0460.USER_ID = "TESTWWP";
            t0460.AGY_DCSN_CD = "ERA";
            t0460.EXT_BEG_MM = Convert.ToDecimal(extension.BeginMonth.ToStringMonthYearComposite());
            t0460.EXT_END_MM = Convert.ToDecimal(extension.EndMonth.ToStringMonthYearComposite());
            t0460.STA_DCSN_CD = "AAA";

            this.extensions.Add(extension);
            this._legacyExtensions.Add(t0460);

            #endregion

            #region  add 11 more  CSJ's with one non-participation month in the middle (spanning 12 months)
            for (var i = 1; i <= 12; i++)
            {
                effMonth = effMonth.AddMonths(1);
                //var clockType = i < 44 ? ClockTypes.CSJ : i > 50 && i<55 ? ClockTypes.TMP : ClockTypes.W2T;

                var tick = new T0459_IN_W2_LIMITS()
                {
                    BENEFIT_MM = Decimal.Parse(effMonth.ToStringMonthYearComposite()),
                    CLOCK_TYPE_CD = "CSJ",
                    FED_CLOCK_IND = "Y",
                    HISTORY_CD = 0,
                    COMMENT_TXT = "Test Data",
                    CRE_TRAN_CD = "WWPTest",
                    FED_MAX_MTH_NUM = 60,
                    TOT_MAX_MTH_NUM = 60,
                    WW_MAX_MTH_NUM = 36,
                    HISTORY_SEQ_NUM = 1,
                    Id = id++,
                    PIN_NUM = testParticipant.PinNumber.Value,
                };

                tick.FED_CMP_MTH_NUM = (Int16)(last.FED_CMP_MTH_NUM + 1);
                tick.TOT_CMP_MTH_NUM = (Int16)(last.OT_CMP_MTH_NUM + 1);
                tick.WW_CMP_MTH_NUM = (Int16)(last.WW_CMP_MTH_NUM + 1);

                var timelimit = new TimeLimit()
                {
                    TimeLimitTypeId = (Int32)ClockTypes.CSJ,
                    CreatedDate = effMonth.AddDays(-1),
                    Participant = this.testParticipant,
                    ParticipantID = this.testParticipant.Id,
                    FederalTimeLimit = tick.FED_CLOCK_IND == "Y",
                    EffectiveMonth = effMonth,
                    StateTimelimit = true,
                    TwentyFourMonthLimit = true

                };
                if (i == 8)
                {
                    this._emptyCSJMonth = effMonth;
                    continue;

                }
                this.timelmits.Add(timelimit);
                this._legacyTicks.Add(tick);

                last = tick;
            }
            #endregion

            #region Add second CSJ (12-month) Extension

            var extension2 = new TimeLimitExtension()
            {
                TimeLimitTypeId = (Int16)ClockTypes.CSJ,
                BeginMonth = effMonth.AddMonths(1),
                EndMonth = effMonth.AddMonths(13),
                CreatedDate = effMonth.AddDays(3),
                DecisionDate = effMonth.AddDays(3),
                Participant = this.testParticipant,
                ParticipantId = this.testParticipant.Id,
                ApprovalReasonId = 1,
                ApprovalReason = new ApprovalReason() { Code = "COO", Id = 1 },
                ExtensionDecisionId = (Int32)Rules.Domain.ExtensionDecision.Approve
            };
            var t04602 = new T0460_IN_W2_EXT();
            t0460.PIN_NUM = extension.Participant.PinNumber.Value;
            t0460.HISTORY_CD = 0;
            t0460.UPDATED_DT = DateTime.Now;
            t0460.CLOCK_TYPE_CD = "CSJ";
            t0460.AGY_DCSN_DT = DateTime.Now;
            t0460.BENEFIT_MM = last.BENEFIT_MM;
            t0460.EXT_SEQ_NUM = 1;
            t0460.HISTORY_SEQ_NUM = 1;
            t0460.USER_ID = "TESTWWP";
            t0460.AGY_DCSN_CD = "ERA";
            t0460.EXT_BEG_MM = Convert.ToDecimal(extension.BeginMonth.ToStringMonthYearComposite());
            t0460.EXT_END_MM = Convert.ToDecimal(extension.EndMonth.ToStringMonthYearComposite());
            t0460.STA_DCSN_CD = "AAA";

            this.extensions.Add(extension2);
            this._legacyExtensions.Add(t04602);

            #endregion

            #region  add 12 more  CSJ's
            for (var i = 1; i <= 12; i++)
            {
                effMonth = effMonth.AddMonths(1);
                //var clockType = i < 44 ? ClockTypes.CSJ : i > 50 && i<55 ? ClockTypes.TMP : ClockTypes.W2T;


                var tick = new T0459_IN_W2_LIMITS()
                {
                    BENEFIT_MM = Decimal.Parse(effMonth.ToStringMonthYearComposite()),
                    CLOCK_TYPE_CD = "CSJ",
                    FED_CLOCK_IND = "Y",
                    HISTORY_CD = 0,
                    COMMENT_TXT = "Test Data",
                    CRE_TRAN_CD = "WWPTest",
                    FED_MAX_MTH_NUM = 60,
                    TOT_MAX_MTH_NUM = 60,
                    WW_MAX_MTH_NUM = 48,
                    HISTORY_SEQ_NUM = 1,
                    Id = id++,
                    PIN_NUM = testParticipant.PinNumber.Value,
                };

                tick.FED_CMP_MTH_NUM = (Int16)(last.FED_CMP_MTH_NUM + 1);
                tick.TOT_CMP_MTH_NUM = (Int16)(last.OT_CMP_MTH_NUM + 1);
                tick.WW_CMP_MTH_NUM = (Int16)(last.WW_CMP_MTH_NUM + 1);

                var timelimit = new TimeLimit()
                {
                    TimeLimitTypeId = (Int32)ClockTypes.CSJ,
                    CreatedDate = effMonth.AddDays(-1),
                    Participant = this.testParticipant,
                    ParticipantID = this.testParticipant.Id,
                    FederalTimeLimit = tick.FED_CLOCK_IND == "Y",
                    EffectiveMonth = effMonth,
                    StateTimelimit = true,
                    TwentyFourMonthLimit = true

                };
                this.timelmits.Add(timelimit);
                this._legacyTicks.Add(tick);

                last = tick;
            }
            #endregion

            // no we are at 48 CSJ with two extensions

            // add gap month so we know where to add ticks to change counts!
            effMonth = effMonth.AddMonths(1);
            this._emptyMonth = effMonth;

            #region Add 9 W2T spanning 10 months
            for (var i = 1; i <= 10; i++)
            {
                effMonth = effMonth.AddMonths(1);
                //var clockType = i < 44 ? ClockTypes.CSJ : i > 50 && i<55 ? ClockTypes.TMP : ClockTypes.W2T;


                var tick = new T0459_IN_W2_LIMITS()
                {
                    BENEFIT_MM = Decimal.Parse(effMonth.ToStringMonthYearComposite()),
                    CLOCK_TYPE_CD = "W2T",
                    FED_CLOCK_IND = "Y",
                    HISTORY_CD = 0,
                    COMMENT_TXT = "Test Data",
                    CRE_TRAN_CD = "WWPTest",
                    FED_MAX_MTH_NUM = 60,
                    TOT_MAX_MTH_NUM = 60,
                    WW_MAX_MTH_NUM = 48,
                    HISTORY_SEQ_NUM = 1,
                    Id = id++,
                    PIN_NUM = testParticipant.PinNumber.Value,
                };

                tick.FED_CMP_MTH_NUM = (Int16)(last.FED_CMP_MTH_NUM + 1);
                tick.TOT_CMP_MTH_NUM = (Int16)(last.OT_CMP_MTH_NUM + 1);
                tick.WW_CMP_MTH_NUM = (Int16)(last.WW_CMP_MTH_NUM + 1);

                var timelimit = new TimeLimit()
                {
                    TimeLimitTypeId = (Int32)ClockTypes.W2T,
                    CreatedDate = effMonth.AddDays(-1),
                    Participant = this.testParticipant,
                    ParticipantID = this.testParticipant.Id,
                    FederalTimeLimit = tick.FED_CLOCK_IND == "Y",
                    EffectiveMonth = effMonth,
                    StateTimelimit = true,
                    TwentyFourMonthLimit = true

                };
                if (i == 3)
                {
                    this._emptyW2TMonth = effMonth;
                    continue;
                }

                this.timelmits.Add(timelimit);
                this._legacyTicks.Add(tick);

                last = tick;
            }
            #endregion

            #region Add 6 month State Extension
            var extension3 = new TimeLimitExtension()
            {
                TimeLimitTypeId = (Int16)ClockTypes.CSJ,
                BeginMonth = effMonth.AddMonths(3),
                EndMonth = effMonth.AddMonths(9),
                CreatedDate = effMonth.AddDays(3),
                DecisionDate = effMonth.AddDays(3),
                Participant = this.testParticipant,
                ParticipantId = this.testParticipant.Id,
                ApprovalReasonId = 1,
                ApprovalReason = new ApprovalReason() { Code = "COO", Id = 1 },
                ExtensionDecisionId = (Int32)Rules.Domain.ExtensionDecision.Approve
            };
            var t04603 = new T0460_IN_W2_EXT();
            t0460.PIN_NUM = extension.Participant.PinNumber.Value;
            t0460.HISTORY_CD = 0;
            t0460.UPDATED_DT = DateTime.Now;
            t0460.CLOCK_TYPE_CD = "60M0";
            t0460.AGY_DCSN_DT = DateTime.Now;
            t0460.BENEFIT_MM = last.BENEFIT_MM;
            t0460.EXT_SEQ_NUM = 1;
            t0460.HISTORY_SEQ_NUM = 1;
            t0460.USER_ID = "TESTWWP";
            t0460.AGY_DCSN_CD = "ERA";
            t0460.EXT_BEG_MM = Convert.ToDecimal(extension.BeginMonth.ToStringMonthYearComposite());
            t0460.EXT_END_MM = Convert.ToDecimal(extension.EndMonth.ToStringMonthYearComposite());
            t0460.STA_DCSN_CD = "AAA";

            this.extensions.Add(extension3);
            this._legacyExtensions.Add(t04603);
            #endregion

            #region Add a TMP for giggles
            effMonth = effMonth.AddMonths(1);
            //var clockType = i < 44 ? ClockTypes.CSJ : i > 50 && i<55 ? ClockTypes.TMP : ClockTypes.W2T;


            var tick2 = new T0459_IN_W2_LIMITS()
            {
                BENEFIT_MM = Decimal.Parse(effMonth.ToStringMonthYearComposite()),
                CLOCK_TYPE_CD = "TMP",
                FED_CLOCK_IND = "Y",
                HISTORY_CD = 0,
                COMMENT_TXT = "Test Data",
                CRE_TRAN_CD = "WWPTest",
                FED_MAX_MTH_NUM = 60,
                TOT_MAX_MTH_NUM = 60,
                WW_MAX_MTH_NUM = 48,
                HISTORY_SEQ_NUM = 1,
                Id = id++,
                PIN_NUM = testParticipant.PinNumber.Value,
            };

            tick2.FED_CMP_MTH_NUM = (Int16)(last.FED_CMP_MTH_NUM + 1);
            tick2.TOT_CMP_MTH_NUM = (Int16)(last.OT_CMP_MTH_NUM + 1);
            tick2.WW_CMP_MTH_NUM = (Int16)(last.WW_CMP_MTH_NUM + 1);

            var timelimit2 = new TimeLimit()
            {
                TimeLimitTypeId = (Int32)ClockTypes.TMP,
                CreatedDate = effMonth.AddDays(-1),
                Participant = this.testParticipant,
                ParticipantID = this.testParticipant.Id,
                FederalTimeLimit = tick2.FED_CLOCK_IND == "Y",
                EffectiveMonth = effMonth,
                StateTimelimit = true,
                TwentyFourMonthLimit = true

            };
            this.timelmits.Add(timelimit2);
            this._legacyTicks.Add(tick2);

            last = tick2;
            #endregion

            //this._moqContext.MockSetFor<TimeLimit>(this.timelmits);
            //this._moqContext.MockSetFor<TimeLimitExtension>(this.extensions);
            //this._moqContext.MockSetFor<T0459_IN_W2_LIMITS>(this._legacyTicks);
            //this._moqContext.MockSetFor<T0460_IN_W2_EXT>(this._legacyExtensions);
            //this._moqContext.MockSetFor<WorkerLoginDetail>(new List<WorkerLoginDetail>() {new WorkerLoginDetail() {UserId = "fras0132", WorkerId = "XOXOXO"}});
            //this._moqContext.MockSetFor<ChangeReason>(new List<ChangeReason>());
            //this._moqContext.MockSetFor<ApprovalReason>(new List<ApprovalReason>());
            //this._moqContext.MockSetFor<DenialReason>(new List<DenialReason>());

            this._moqContext.Setup(x => x.TimeLimits).Returns(new Mock<DbSet<TimeLimit>>().SetupData(this.timelmits.ToList()).Object);
            this._moqContext.Setup(x => x.TimeLimitExtensions).Returns(new Mock<DbSet<TimeLimitExtension>>().SetupData(this.extensions.ToList()).Object);
            this._moqContext.Setup(x => x.T0459_IN_W2_LIMITS).Returns(new Mock<DbSet<T0459_IN_W2_LIMITS>>().SetupData(this._legacyTicks.ToList()).Object);
            this._moqContext.Setup(x => x.T0460_IN_W2_EXT).Returns(new Mock<DbSet<T0460_IN_W2_EXT>>().SetupData(this._legacyExtensions.ToList()).Object);
            //this._moqContext.Setup(x => x.WorkerLoginDetails).Returns(new Mock<DbSet<WorkerLoginDetail>>().SetupData(new List<WorkerLoginDetail>() { new WorkerLoginDetail() { UserId = "fras0132", WorkerId = "XOXOXO" } }).Object);
            this._moqContext.Setup(x => x.ChangeReasons).Returns(new Mock<DbSet<ChangeReason>>().SetupData(new List<ChangeReason>()).Object);
            this._moqContext.Setup(x => x.ApprovalReasons).Returns(new Mock<DbSet<ApprovalReason>>().SetupData(new List<ApprovalReason>()).Object);
            this._moqContext.Setup(x => x.DenialReasons).Returns(new Mock<DbSet<DenialReason>>().SetupData(new List<DenialReason>()).Object);

        }

        [TestMethod]
        public void Db2service_UpdateT0459ModelCounts_should_fix_counts_to_calculated_used()
        {
            this.setup1();
            // arrange
            using (var repo = new Repository(this.dbContextFactory(), null)) // added 'null' as second parm for now
            using (var timelimitsService = new TimelimitService(this.dbContextFactory()))
            using (var db2service = new Db2TimelimitService(repo, timelimitsService))
            {
                var timeline = timelimitsService.GetTimeline(this.testParticipant.Id);
                db2service.IsSimulated = true;
                var effMonth = Convert.ToDecimal(DateTime.Now.AddMonths(-13).ToStringMonthYearComposite());
                // act
                var testTick = repo.GetW2LimitByMonth(effMonth, this.testParticipant.PinNumber.Value);
                testTick.ShouldNotBeNull();

                var startCount = testTick.WW_CMP_MTH_NUM;
                testTick.WW_CMP_MTH_NUM = (Int16)(testTick.WW_CMP_MTH_NUM - 8);//screw up counts


                db2service.UpdateT0459ModelCounts((IT0459_IN_W2_LIMITS)testTick.Clone(), timeline,testTick);

                testTick.WW_CMP_MTH_NUM.ShouldBe(startCount);
            }
        }

        [TestMethod]
        public void Db2Service_UpdateT0459ModelCounts_should_update_latest_ticks_with_state_totals()
        {
            this.setup1();
            // arrange
            using (var repo = new Repository(this.dbContextFactory(), null)) // added 'null'
            using (var timelimitsService = new TimelimitService(this.dbContextFactory()))
            using (var db2service = new Db2TimelimitService(repo, timelimitsService))
            {
                db2service.IsSimulated = true;

                var oldList = repo.GetW2LimitsByPin(this.testParticipant.PinNumber.Value);
                var oldListLatestList = repo.GetLatestW2LimitsMonthsForEachClockType(this.testParticipant.PinNumber.Value);

                var timeline = timelimitsService.GetTimeline(this.testParticipant.Id);
                // ensure that latest list has the max counts
                oldListLatestList.ForEach(x => db2service.UpdateT0459ModelCounts(x, timeline, null));

                var lastTmpTick = repo.GetLatestW2LimitsByClockType(this.testParticipant.PinNumber.Value, ClockTypes.TMP);
                var lastCsjTick = repo.GetLatestW2LimitsByClockType(this.testParticipant.PinNumber.Value, ClockTypes.CSJ);
                var lastw2Tick = repo.GetLatestW2LimitsByClockType(this.testParticipant.PinNumber.Value, ClockTypes.W2T);

                // Make sure state used is the same
                lastTmpTick.TOT_CMP_MTH_NUM.ShouldBe(lastCsjTick.TOT_CMP_MTH_NUM);
                lastCsjTick.TOT_CMP_MTH_NUM.ShouldBe(lastw2Tick.TOT_CMP_MTH_NUM);

                // Make sure state max is the same
                lastTmpTick.TOT_MAX_MTH_NUM.ShouldBe(lastCsjTick.TOT_MAX_MTH_NUM);
                lastCsjTick.TOT_MAX_MTH_NUM.ShouldBe(lastw2Tick.TOT_MAX_MTH_NUM);
            }
        }

        [TestMethod]

        public void Db2Service_Adding_CSJ_tick_should_update_subsequent_ticks_with_context_aware_counts()
        {
            this.setup1();

            //var last = this._legacyTicks.FirstOrDefault(x => x.BENEFIT_MM.ToString() == this._emptyCSJMonth.Value.AddMonths(-1).ToStringMonthYearComposite());
            //var tick2 = new T0459_IN_W2_LIMITS()
            //{
            //    BENEFIT_MM = Decimal.Parse(this._emptyCSJMonth.ToStringMonthYearComposite()),
            //    CLOCK_TYPE_CD = "CSJ",
            //    FED_CLOCK_IND = "Y",
            //    HISTORY_CD = 0,
            //    COMMENT_TXT = "Test Data",
            //    CRE_TRAN_CD = "WWPTest",
            //    FED_MAX_MTH_NUM = 60,
            //    TOT_MAX_MTH_NUM = 60,
            //    WW_MAX_MTH_NUM = 48,
            //    HISTORY_SEQ_NUM = 1,
            //    Id = 75,
            //    PIN_NUM = testParticipant.PinNumber.Value,
            //};

            //tick2.FED_CMP_MTH_NUM = (Int16)(last.FED_CMP_MTH_NUM + 1);
            //tick2.TOT_CMP_MTH_NUM = (Int16)(last.OT_CMP_MTH_NUM + 1);
            //tick2.WW_CMP_MTH_NUM = (Int16)(last.WW_CMP_MTH_NUM + 1);

            var timelimit2 = new TimeLimit()
            {
                TimeLimitTypeId = (Int32)ClockTypes.CSJ,
                CreatedDate = this._emptyCSJMonth.Value.AddDays(-1),
                Participant = this.testParticipant,
                ParticipantID = this.testParticipant.Id,
                FederalTimeLimit = true,
                EffectiveMonth = this._emptyCSJMonth,
                StateTimelimit = true,
                TwentyFourMonthLimit = true

            };
            // arrange
            using (var repo = new Repository(this.dbContextFactory(), null))
            using (var timelimitsService = new TimelimitService(this.dbContextFactory()))
            using (var db2service = new Db2TimelimitService(repo, timelimitsService))
            {
                //db2service.IsSimulated = true;

                var oldList = repo.GetW2LimitsByPin(this.testParticipant.PinNumber.Value).Select(x => (T0459_IN_W2_LIMITS)x.Clone()).ToList();
                var oldlatestList = repo.GetLatestW2LimitsMonthsForEachClockType(this.testParticipant.PinNumber.Value).Select(x => (T0459_IN_W2_LIMITS)x.Clone()).ToList();

                var timeline = timelimitsService.GetTimeline(this.testParticipant.Id);
                // ensure that latest list has the max counts
                oldlatestList.ForEach(x => db2service.UpdateT0459ModelCounts(x, timeline,null));


                var newTick = db2service.Upsert(timelimit2, this.testParticipant, "fras0132");

                timeline.AddTimelineMonth(TimelimitService.MapTimelimitToTimelineMonth(timelimit2));

                var lastTmpTick = repo.GetLatestW2LimitsByClockType(this.testParticipant.PinNumber.Value, ClockTypes.TMP);
                var lastCsjTick = repo.GetLatestW2LimitsByClockType(this.testParticipant.PinNumber.Value, ClockTypes.CSJ);
                var lastw2Tick = repo.GetLatestW2LimitsByClockType(this.testParticipant.PinNumber.Value, ClockTypes.W2T);

                var allTicks = repo.GetW2LimitsByPin(this.testParticipant.PinNumber.Value);
                for (var index = 0; index < allTicks.Count; index++)
                {
                    var tick = allTicks[index];
                    if (tick == newTick)
                    {
                        continue;

                    }
                    else if (tick.BENEFIT_MM < newTick.BENEFIT_MM && !oldlatestList.Any(x => x.BENEFIT_MM == tick.BENEFIT_MM))
                    {
                        // find original
                        var oldTick = oldList.FirstOrDefault(x => x.BENEFIT_MM == tick.BENEFIT_MM);
                        // Make sure we didn't upadte any old ticks
                        tick.AreSemanticallyEqual(oldTick).ShouldBeTrue();
                    }
                    else if (tick.BENEFIT_MM > newTick.BENEFIT_MM)
                    {
                        if (oldlatestList.Any(x => x.BENEFIT_MM == tick.BENEFIT_MM))
                        {
                            if (tick.CLOCK_TYPE_CD == "CSJ")
                            {
                                // Federal should be the same as the timeline calcualtion and incremented by 1
                                tick.FED_CMP_MTH_NUM.ShouldBe((Int16) timeline.GetUsedMonths(ClockTypes.Federal).Value);
                                tick.FED_CMP_MTH_NUM.ShouldBe((Int16) (oldlatestList.First(x => x.CLOCK_TYPE_CD == "CSJ").FED_CMP_MTH_NUM + 1));
                                tick.FED_MAX_MTH_NUM.ShouldBe((Int16) timeline.FederalMax.Value);
                            }
                        }
                        else
                        {
                            var clockType = Placement.GetPlacementTypeFromCode(tick.CLOCK_TYPE_CD);
                            var benefitMM = DateTime.ParseExact(tick.BENEFIT_MM.ToString(), "yyyyMM", CultureInfo.InvariantCulture);
                            var prevBenefitMM = Convert.ToDecimal(benefitMM.AddMonths(-1).ToStringMonthYearComposite());
                            var prev = allTicks.FirstOrDefault(x => x.BENEFIT_MM == prevBenefitMM);
                            if (prev != null)
                            {

                                var originalTimlineDate = timeline.TimelineDate;
                                timeline.TimelineDate = benefitMM;

                                // Check Federal
                                tick.FED_CMP_MTH_NUM.ShouldBe((Int16)timeline.GetUsedMonths(ClockTypes.Federal).Value);
                                tick.FED_CMP_MTH_NUM.ShouldBe((Int16)(prev.FED_CMP_MTH_NUM + 1));
                                tick.FED_MAX_MTH_NUM.ShouldBe((Int16)timeline.FederalMax.Value);

                                // Check State
                                tick.TOT_CMP_MTH_NUM.ShouldBe((Int16) timeline.GetUsedMonths(ClockTypes.State).Value);
                                tick.TOT_CMP_MTH_NUM.ShouldBe((Int16) (prev.TOT_CMP_MTH_NUM + 1));
                                tick.TOT_MAX_MTH_NUM.ShouldBe((Int16) timeline.GetMaxMonths(ClockTypes.State).Value);


                                //Check clock
                                tick.WW_CMP_MTH_NUM.ShouldBe((Int16) timeline.GetUsedMonths(clockType).Value);
                                tick.WW_CMP_MTH_NUM.ShouldBe((Int16) (prev.WW_CMP_MTH_NUM + 1));
                                tick.WW_MAX_MTH_NUM.ShouldBe((Int16) timeline.GetMaxMonths(clockType).Value);
                                if (prev.CLOCK_TYPE_CD != tick.CLOCK_TYPE_CD)
                                {
                                    //Check clock
                                    tick.WW_CMP_MTH_NUM.ShouldBe((Int16) 1);
                                    tick.WW_MAX_MTH_NUM.ShouldBe((Int16) timeline.GetClockMax(clockType).Value);
                                }

                                timeline.TimelineDate = originalTimlineDate;
                            }
                        }
                    }
                }
            }
        }

    }
}
