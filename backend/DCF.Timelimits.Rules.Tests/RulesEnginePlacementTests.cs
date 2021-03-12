using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Dcf.Wwp.Data.Sql.Model;
using Dcf.Wwp.Model.Interface;
using DCF.Common.Dates;
using DCF.Common.Extensions;
using DCF.Common.Logging;
using DCF.Timelimits.Rules.Definitions;
using DCF.Timelimits.Rules.Domain;
using DCF.Timelimits.Rules.Scripting;
using EnumsNET;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NRules;
using NRules.Extensibility;
using NRules.RuleModel;
using Serilog;
using Serilog.Events;
using Shouldly;

namespace DCF.Timelimits.Rules.Tests
{
    [TestClass]
    public class RulesEnginePlacementTests
    {
        private static ApplicationContext _appCtx;
        private static ISessionFactory _sessionFactory;
        private ISession _session;


        private class TestRuleInterceptor : IActionInterceptor
        {
            public void Intercept(IContext context, IEnumerable<IActionInvocation> actions)
            {
                var rule = context.Rule;
                
                    var conditionDeclartions = rule.LeftHandSide.Declarations.ToList();
                    var facts = context.Facts.ToList();

                if (actions == null)
                {
                    return;
                }

                foreach (var action in actions)
                {
                    action?.Invoke();
                }
            }
        }

        public static StringBuilder RuleLog = new StringBuilder();
        public static StringWriter LogWriter = new StringWriter(RulesEnginePlacementTests.RuleLog);

        [ClassInitialize]
        public static void ClassInit(TestContext ctx)
        {
            //Log.Logger = new LoggerConfiguration().WriteTo.LiterateConsole(LogEventLevel.Verbose).CreateLogger();
            Log.Logger = new LoggerConfiguration().WriteTo.TextWriter(RulesEnginePlacementTests.LogWriter).CreateLogger();
            RulesEnginePlacementTests._appCtx = new ApplicationContext(DateTime.Now.AddMonths(-1).EndOf(DateTimeUnit.Month));
            RulesEnginePlacementTests._sessionFactory = RulesEngine.CompileTimelimitRuleNetwork();
        }



        [TestInitialize]
        public void TestInit()
        {
            this._session = RulesEnginePlacementTests._sessionFactory.CreateSession();
            RulesEngine.LogSessionEvents(this._session, LogProvider.GetLogger(this.GetType()));
            this._session.ActionInterceptor = new TestRuleInterceptor();

        }

        [ClassCleanup]
        public static void ClassDispose()
        {
            Log.CloseAndFlush();
            RulesEnginePlacementTests.LogWriter?.Close();
        }



        #region CSJ Placement Tests

        [TestMethod]
        public void RulesEngine_Should_Create_LastEmploymentPosition_Of_CJS_When_lastPlacement_Is_CSJ()
        {
            // Arrange

            var timeline = new Timeline(RulesEnginePlacementTests._appCtx);
            var placement = new Placement(ClockTypes.CSJ, RulesEnginePlacementTests._appCtx.Date.StartOf(DateTimeUnit.Month), DateTime.MaxValue);
            timeline.AddPlacement(placement);

            var ruleContext = new RuleContext();
            ruleContext.EvaluationMonth = RulesEnginePlacementTests._appCtx.Date;

            this._session.Insert(timeline);
            this._session.Insert(ruleContext);

            // Act
            this._session.Fire();

            // Assert
            var facts = this._session.Query<RuleContext>().ToList();

            facts.ShouldHaveSingleItem();

            var result = facts.First();

            result.LastEmploymentPosition.ShouldNotBeNull();
            result.LastEmploymentPosition.IsOpen.ShouldBeTrue();
            result.LastEmploymentPosition.PlacementType.HasValue.ShouldBeTrue();
            result.LastEmploymentPosition.PlacementType.Value.ShouldBe(ClockTypes.CSJ);



        }



        #endregion

        #region W2T Placement Tests
        [TestMethod]
        public void RulesEngine_Should_Create_Tick_Of_W2T_When_lastPlacement_Is_W2T()
        {
            // Arrange

            var timeline = new Timeline(RulesEnginePlacementTests._appCtx);
            var placement = new Placement(ClockTypes.W2T, RulesEnginePlacementTests._appCtx.Date.StartOf(DateTimeUnit.Month), DateTime.MaxValue);
            timeline.AddPlacement(placement);

            var ruleContext = new RuleContext();
            ruleContext.EvaluationMonth = RulesEnginePlacementTests._appCtx.Date;

            this._session.Insert(timeline);
            this._session.Insert(ruleContext);

            // Act
            this._session.Fire();

            // Assert
            var facts = this._session.Query<RuleContext>().ToList();

            facts.ShouldHaveSingleItem();

            var result = facts.First();

            result.TimelimitType.GetValueOrDefault().HasAnyFlags(ClockTypes.W2T).ShouldBeTrue();

        }

        #endregion

        #region CMC Tests

        [TestMethod]
        public void Should_Determine_Moved_Directly_When_CSJ_To_CMC()
        {
            // Arrange

            var timeline = new Timeline(RulesEnginePlacementTests._appCtx);
            var cmcPlacement = new Placement(ClockTypes.CMC, RulesEnginePlacementTests._appCtx.Date.StartOf(DateTimeUnit.Month), DateTime.MaxValue);
            var csjPlacement = new Placement(ClockTypes.CSJ, cmcPlacement.DateRange.Start.AddMonths(-1).StartOf(DateTimeUnit.Month), cmcPlacement.DateRange.Start.AddDays(-1));
            timeline.AddPlacements(new List<Placement> { csjPlacement, cmcPlacement });

            var ruleContext = new RuleContext();
            ruleContext.EvaluationMonth = RulesEnginePlacementTests._appCtx.Date;

            this._session.Insert(timeline);
            this._session.Insert(ruleContext);

            // Act
            this._session.Fire();

            // Assert
            var facts = this._session.Query<RuleContext>().ToList();

            facts.ShouldHaveSingleItem();

            var result = facts.First();
            result.MovedDirectlyIntoCmc.GetValueOrDefault().ShouldBeTrue();
        }

        [TestMethod]
        public void Should_Determine_Moved_Directly_When_NonPaidPlacemnt_To_CMC()
        {
            // Arrange

            var timeline = new Timeline(RulesEnginePlacementTests._appCtx);
            var cmcPlacement = new Placement(ClockTypes.CMC, RulesEnginePlacementTests._appCtx.Date.StartOf(DateTimeUnit.Month), DateTime.MaxValue);
            var arpPlacement = new Placement("ARP", cmcPlacement.DateRange.Start.AddMonths(-1).StartOf(DateTimeUnit.Month), cmcPlacement.DateRange.Start.AddDays(-1));
            timeline.AddPlacements(new List<Placement> { arpPlacement, cmcPlacement });

            var ruleContext = new RuleContext();
            ruleContext.EvaluationMonth = RulesEnginePlacementTests._appCtx.Date;

            this._session.Insert(timeline);
            this._session.Insert(ruleContext);

            // Act
            this._session.Fire();

            // Assert
            var facts = this._session.Query<RuleContext>().ToList();

            facts.ShouldHaveSingleItem();

            var result = facts.First();
            result.MovedDirectlyIntoCmc.GetValueOrDefault().ShouldBeTrue();
        }

        [TestMethod]
        public void Should_Determine_Not_Moved_Directly_When_CSJ_To_CMC_With_Gap()
        {
            var timeline = new Timeline(RulesEnginePlacementTests._appCtx);
            var cmcPlacement = new Placement(ClockTypes.CMC, RulesEnginePlacementTests._appCtx.Date.StartOf(DateTimeUnit.Month).AddDays(1), DateTime.MaxValue);
            var csjPlacement = new Placement(ClockTypes.CSJ, cmcPlacement.DateRange.Start.AddMonths(-1).StartOf(DateTimeUnit.Month), cmcPlacement.DateRange.Start.AddDays(-1));
            timeline.AddPlacements(new List<Placement> { csjPlacement, cmcPlacement });

            var ruleContext = new RuleContext();
            ruleContext.EvaluationMonth = RulesEnginePlacementTests._appCtx.Date;

            this._session.Insert(timeline);
            this._session.Insert(ruleContext);

            // Act
            this._session.Fire();

            // Assert
            var facts = this._session.Query<RuleContext>().ToList();

            facts.ShouldHaveSingleItem();

            var result = facts.First();
            result.MovedDirectlyIntoCmc.GetValueOrDefault().ShouldBeTrue();
        }


        

        [TestMethod]
        public void Should_Not_Tick_Previous_Placement_WhenMovedDirectly_and_Only_Has_adult_in_AG()
        {
            // Arrange
            var dob = RulesEnginePlacementTests._appCtx.Date.AddMonths(-1);
            var adult = new AssistanceGroupMember() { BIRTH_DATE = RulesEnginePlacementTests._appCtx.Date.AddYears(-25), RELATIONSHIP = "Husband" }; // Should not trigger

            var timeline = new Timeline(RulesEnginePlacementTests._appCtx);
            var movedDirectlyPlacement = new Placement(ClockTypes.CSJ, RulesEnginePlacementTests._appCtx.Date.AddMonths(-1).StartOf(DateTimeUnit.Month), RulesEnginePlacementTests._appCtx.Date.AddMonths(-1).EndOf(DateTimeUnit.Month));
            var placement = new Placement(ClockTypes.CMC, RulesEnginePlacementTests._appCtx.Date.StartOf(DateTimeUnit.Month), DateTime.MaxValue);
            var jobsPlacement = new Placement(ClockTypes.JOBS, dob.AddDays(-304), dob.AddDays(-300));
            timeline.AddPlacement(movedDirectlyPlacement);
            timeline.AddPlacement(placement);
            timeline.AddPlacement(jobsPlacement);


            var ruleContext = new RuleContext { EvaluationMonth = RulesEnginePlacementTests._appCtx.Date };

            this._session.Insert(timeline);
            this._session.Insert(ruleContext);

            //add a Adult more then 10 months of placement
            this._session.Insert(adult);
            // Act
            this._session.Fire();

            // Assert
            var facts = this._session.Query<RuleContext>().ToList();
            facts.ShouldHaveSingleItem();
            var result = facts.First();
            result.CmcShouldTickPreviousPlacement.ShouldNotBeNull();
            result.CmcShouldTickPreviousPlacement.Value.ShouldBeFalse();
        }

        [TestMethod]
        public void should_not_tick_previous_placement_whenMovedDirectly_and_has_child_born_294_days_after_start()
        {

            // Arrange
            var dob = RulesEnginePlacementTests._appCtx.Date.AddMonths(-1);
            var child1 = new AssistanceGroupMember() { BIRTH_DATE = dob.AddDays(-10), RELATIONSHIP = "Child" };


            var timeline = new Timeline(RulesEnginePlacementTests._appCtx);
            var movedDirectlyPlacement = new Placement(ClockTypes.CSJ, RulesEnginePlacementTests._appCtx.Date.AddMonths(-1).StartOf(DateTimeUnit.Month), RulesEnginePlacementTests._appCtx.Date.AddMonths(-1).EndOf(DateTimeUnit.Month));
            var placement = new Placement(ClockTypes.CMC, RulesEnginePlacementTests._appCtx.Date.StartOf(DateTimeUnit.Month), DateTime.MaxValue);
            var jobsPlacement = new Placement(ClockTypes.JOBS, dob.AddDays(-304), dob.AddDays(-300));
            timeline.AddPlacement(movedDirectlyPlacement);
            timeline.AddPlacement(placement);
            timeline.AddPlacement(jobsPlacement);


            var ruleContext = new RuleContext { EvaluationMonth = RulesEnginePlacementTests._appCtx.Date };

            this._session.Insert(timeline);
            this._session.Insert(ruleContext);

            // have a child born exactly 294 month after 
            this._session.Insert(child1);

            // Act
            this._session.Fire();

            // Assert
            var facts = this._session.Query<RuleContext>().ToList();
            facts.ShouldHaveSingleItem();
            var result = facts.First();
            result.CmcShouldTickPreviousPlacement.ShouldNotBeNull();
            result.CmcShouldTickPreviousPlacement.Value.ShouldBeFalse();
        }

        [TestMethod]
        public void should_not_tick_previous_placement_whenMovedDirectly_and_has_child_born_304_days_after_start()
        {
            // Arrange
            var dob = RulesEnginePlacementTests._appCtx.Date.AddMonths(-1);
            var child2 = new AssistanceGroupMember() { BIRTH_DATE = dob, RELATIONSHIP = "Child" };


            var timeline = new Timeline(RulesEnginePlacementTests._appCtx);
            var movedDirectlyPlacement = new Placement(ClockTypes.CSJ, RulesEnginePlacementTests._appCtx.Date.AddMonths(-1).StartOf(DateTimeUnit.Month), RulesEnginePlacementTests._appCtx.Date.AddMonths(-1).EndOf(DateTimeUnit.Month));
            var placement = new Placement(ClockTypes.CMC, RulesEnginePlacementTests._appCtx.Date.StartOf(DateTimeUnit.Month), DateTime.MaxValue);
            var jobsPlacement = new Placement(ClockTypes.JOBS, dob.AddDays(-304), dob.AddDays(-300));
            timeline.AddPlacement(movedDirectlyPlacement);
            timeline.AddPlacement(placement);
            timeline.AddPlacement(jobsPlacement);


            var ruleContext = new RuleContext { EvaluationMonth = RulesEnginePlacementTests._appCtx.Date };

            this._session.Insert(timeline);
            this._session.Insert(ruleContext);

            // have a child born exactly 304 month after 
            this._session.Insert(child2);
            // Act
            this._session.Fire();

            // Assert
            var facts = this._session.Query<RuleContext>().ToList();
            facts.ShouldHaveSingleItem();
            var result = facts.First();
            result.CmcShouldTickPreviousPlacement.ShouldNotBeNull();
            result.CmcShouldTickPreviousPlacement.Value.ShouldBeFalse();
        }

        
        [TestMethod]
        public void Should_Determine_TickPreviousePlacement_WhenMovedDirectly_and_Has_Child_born_after_311_months_of_first_placement()
        {
            // Arrange
            var dob = RulesEnginePlacementTests._appCtx.Date.AddMonths(-1);
            var child3 = new AssistanceGroupMember() {BIRTH_DATE = dob.AddDays(1),RELATIONSHIP = "Child"};


            var timeline = new Timeline(RulesEnginePlacementTests._appCtx);
            var movedDirectlyPlacement = new Placement(ClockTypes.CSJ, RulesEnginePlacementTests._appCtx.Date.AddMonths(-1).StartOf(DateTimeUnit.Month), RulesEnginePlacementTests._appCtx.Date.AddMonths(-1).EndOf(DateTimeUnit.Month));
            var placement = new Placement(ClockTypes.CMC, RulesEnginePlacementTests._appCtx.Date.StartOf(DateTimeUnit.Month), DateTime.MaxValue);
            var jobsPlacement = new Placement(ClockTypes.JOBS, dob.AddDays(-304), dob.AddDays(-300));
            timeline.AddPlacement(movedDirectlyPlacement);
            timeline.AddPlacement(placement);
            timeline.AddPlacement(jobsPlacement);


            var ruleContext = new RuleContext {EvaluationMonth = RulesEnginePlacementTests._appCtx.Date};

            this._session.Insert(timeline);
            this._session.Insert(ruleContext);

            
            // have a child born less then exactly 311 month after eligible
            this._session.Insert(child3);
            // Act
            this._session.Fire();

            // Assert
            var facts = this._session.Query<RuleContext>().ToList();
            facts.ShouldHaveSingleItem();
            var result = facts.First();
            result.CmcShouldTickPreviousPlacement.ShouldNotBeNull();
            result.CmcShouldTickPreviousPlacement.Value.ShouldBeTrue();
        }

        [TestMethod]
        public void Should_Tick_CSJ_When_Moved_Directly_child_born_after_10_months_of_w2_start()
        {
            // Arrange
            var dob = RulesEnginePlacementTests._appCtx.Date.AddMonths(-1);
            var adult = new AssistanceGroupMember() { BIRTH_DATE = RulesEnginePlacementTests._appCtx.Date.AddYears(-25), RELATIONSHIP = "Wife" }; // Should not trigger
            var child1 = new AssistanceGroupMember() { BIRTH_DATE = dob.AddDays(-10), RELATIONSHIP = "Child" };
            var child2 = new AssistanceGroupMember() { BIRTH_DATE = dob, RELATIONSHIP = "Child" };
            var child3 = new AssistanceGroupMember() { BIRTH_DATE = dob.AddDays(1), RELATIONSHIP = "Child" };


            var timeline = new Timeline(RulesEnginePlacementTests._appCtx);
            var movedDirectlyPlacement = new Placement(ClockTypes.CSJ, RulesEnginePlacementTests._appCtx.Date.AddMonths(-1).StartOf(DateTimeUnit.Month), RulesEnginePlacementTests._appCtx.Date.AddMonths(-1).EndOf(DateTimeUnit.Month));
            var placement = new Placement(ClockTypes.CMC, RulesEnginePlacementTests._appCtx.Date.StartOf(DateTimeUnit.Month), DateTime.MaxValue);
            var jobsPlacement = new Placement(ClockTypes.JOBS, dob.AddDays(-304), dob.AddDays(-300));
            timeline.AddPlacement(movedDirectlyPlacement);
            timeline.AddPlacement(placement);
            timeline.AddPlacement(jobsPlacement);


            var ruleContext = new RuleContext
            {
                EvaluationMonth = RulesEnginePlacementTests._appCtx.Date,
                CmcShouldTickPreviousPlacement =  true,
                MovedDirectlyIntoCmc = true,
                FirstNonCmcEmploymentPosition = movedDirectlyPlacement
            };

            this._session.Insert(adult);
            this._session.Insert(child1);
            this._session.Insert(child2);
            this._session.Insert(child3);
            this._session.Insert(timeline);
            this._session.Insert(ruleContext);

           
            // have a child born less then exactly 311 month after eligible
            // Act
            this._session.Fire();

            // Assert
            var facts = this._session.Query<RuleContext>().ToList();
            facts.ShouldHaveSingleItem();
            var result = facts.First();
            result.TimelimitType.ShouldNotBeNull();
            result.TimelimitType.GetValueOrDefault().HasAnyFlags(ClockTypes.CSJ);
        }

        [TestMethod]
        public void Should_Not_Tick_When_Moved_Directly_From_non_employment_position()
        {
            // Arrange
            var dob = RulesEnginePlacementTests._appCtx.Date.AddMonths(-1);
            var adult = new AssistanceGroupMember() { BIRTH_DATE = RulesEnginePlacementTests._appCtx.Date.AddYears(-25), RELATIONSHIP = "Husband" }; // Should not trigger
            var child1 = new AssistanceGroupMember() { BIRTH_DATE = dob.AddDays(-10), RELATIONSHIP = "Child" };
            var child2 = new AssistanceGroupMember() { BIRTH_DATE = dob, RELATIONSHIP = "Child" };
            var child3 = new AssistanceGroupMember() { BIRTH_DATE = dob.AddDays(1), RELATIONSHIP = "Child" };


            var timeline = new Timeline(RulesEnginePlacementTests._appCtx);
            var movedDirectlyPlacement = new Placement("ARP", RulesEnginePlacementTests._appCtx.Date.AddMonths(-1).StartOf(DateTimeUnit.Month), RulesEnginePlacementTests._appCtx.Date.AddMonths(-1).EndOf(DateTimeUnit.Month));
            var placement = new Placement(ClockTypes.CMC, RulesEnginePlacementTests._appCtx.Date.StartOf(DateTimeUnit.Month), DateTime.MaxValue);
            var jobsPlacement = new Placement(ClockTypes.JOBS, dob.AddDays(-304), dob.AddDays(-300));
            timeline.AddPlacement(movedDirectlyPlacement);
            timeline.AddPlacement(placement);
            timeline.AddPlacement(jobsPlacement);


            var ruleContext = new RuleContext
            {
                EvaluationMonth = RulesEnginePlacementTests._appCtx.Date,
                //CmcShouldTickPreviousPlacement = true,
                //MovedDirectlyIntoCmc = true,
                //FirstNonCmcEmploymentPosition = movedDirectlyPlacement
            };

            this._session.Insert(adult);
            this._session.Insert(child1);
            this._session.Insert(child2);
            this._session.Insert(child3);
            this._session.Insert(timeline);
            this._session.Insert(ruleContext);


            // have a child born less then exactly 311 month after eligible
            // Act
            this._session.Fire();

            // Assert
            var facts = this._session.Query<RuleContext>().ToList();
            facts.ShouldHaveSingleItem();
            var result = facts.First();
            result.TimelimitType.ShouldNotBeNull();
            result.TimelimitType.GetValueOrDefault().ShouldBe(ClockTypes.None);
        }

        [TestMethod]
        public void should_create_CMC_Federal_only_tick_when_moved_directly_from_CSJ_and_child_born_304_days_after_start()
        {
            // Arrange
            var dob = RulesEnginePlacementTests._appCtx.Date.AddMonths(-1);
            var child1 = new AssistanceGroupMember() { BIRTH_DATE = dob.AddDays(-10), RELATIONSHIP = "Child" };


            var timeline = new Timeline(RulesEnginePlacementTests._appCtx);
            var movedDirectlyPlacement = new Placement(ClockTypes.CSJ, RulesEnginePlacementTests._appCtx.Date.AddMonths(-1).StartOf(DateTimeUnit.Month), RulesEnginePlacementTests._appCtx.Date.AddMonths(-1).EndOf(DateTimeUnit.Month));
            var placement = new Placement(ClockTypes.CMC, RulesEnginePlacementTests._appCtx.Date.StartOf(DateTimeUnit.Month), DateTime.MaxValue);
            var jobsPlacement = new Placement(ClockTypes.JOBS, dob.AddDays(-304), dob.AddDays(-300));
            timeline.AddPlacement(movedDirectlyPlacement);
            timeline.AddPlacement(placement);
            timeline.AddPlacement(jobsPlacement);


            var ruleContext = new RuleContext { EvaluationMonth = RulesEnginePlacementTests._appCtx.Date };

            this._session.Insert(timeline);
            this._session.Insert(ruleContext);

            // have a child born exactly 294 month after 
            this._session.Insert(child1);

            // Act
            this._session.Fire();

            // Assert
            var facts = this._session.Query<RuleContext>().ToList();
            facts.ShouldHaveSingleItem();
            var result = facts.First();
            result.CmcShouldTickPreviousPlacement.ShouldNotBeNull();
            result.CmcShouldTickPreviousPlacement.Value.ShouldBeFalse();



            result.TimelimitType.ShouldNotBeNull();
            result.TimelimitType.Value.ShouldBe(ClockTypes.CMC | ClockTypes.Federal);
        }


        [TestMethod]
        public void should_create_OPC_Federal_only_tick_when_moved_directly_from_CSJ_and_only_spouse_in_ag()
        {
            // Arrange
            var dob = RulesEnginePlacementTests._appCtx.Date.AddMonths(-1);
            var child1 = new AssistanceGroupMember() { BIRTH_DATE = dob.AddYears(30), PinNumber = 1234567, RELATIONSHIP = "Husband" };


            var timeline = new Timeline(RulesEnginePlacementTests._appCtx);
            var movedDirectlyPlacement = new Placement(ClockTypes.CSJ, RulesEnginePlacementTests._appCtx.Date.AddMonths(-1).StartOf(DateTimeUnit.Month), RulesEnginePlacementTests._appCtx.Date.AddMonths(-1).EndOf(DateTimeUnit.Month));
            var placement = new Placement(ClockTypes.CMC, RulesEnginePlacementTests._appCtx.Date.StartOf(DateTimeUnit.Month), DateTime.MaxValue);
            var jobsPlacement = new Placement(ClockTypes.JOBS, dob.AddDays(-304), dob.AddDays(-300));
            timeline.AddPlacement(movedDirectlyPlacement);
            timeline.AddPlacement(placement);
            timeline.AddPlacement(jobsPlacement);


            var ruleContext = new RuleContext { EvaluationMonth = RulesEnginePlacementTests._appCtx.Date };

            this._session.Insert(timeline);
            this._session.Insert(ruleContext);

            // have a child born exactly 294 month after 
            this._session.Insert(child1);

            // Act
            this._session.Fire();

            // Assert
            var facts = this._session.Query<RuleContext>().ToList();
            facts.ShouldHaveSingleItem();
            var result = facts.First();

            result.TimelimitType.ShouldNotBeNull();
            result.TimelimitType.Value.ShouldBe(ClockTypes.CMC | ClockTypes.Federal);

            result.ShouldCreateOpcTicks.GetValueOrDefault().ShouldBeTrue();

            var opcFacts = this._session.Query<OpcTick>();
            opcFacts.Count().ShouldBe(1);
            opcFacts.First().TimelimitType.ShouldBe(ClockTypes.OPC | ClockTypes.Federal);
        }

        [TestMethod]
        public void should_create_OPC_only_tick_when_moved_directly_from_CSJ_and_only_non_spouse_adult_in_ag()
        {
            // Arrange
            var dob = RulesEnginePlacementTests._appCtx.Date.AddMonths(-1);
            var child1 = new AssistanceGroupMember() { BIRTH_DATE = dob.AddYears(30), RELATIONSHIP = "FRIEND" };


            var timeline = new Timeline(RulesEnginePlacementTests._appCtx);
            var movedDirectlyPlacement = new Placement(ClockTypes.CSJ, RulesEnginePlacementTests._appCtx.Date.AddMonths(-1).StartOf(DateTimeUnit.Month), RulesEnginePlacementTests._appCtx.Date.AddMonths(-1).EndOf(DateTimeUnit.Month));
            var placement = new Placement(ClockTypes.CMC, RulesEnginePlacementTests._appCtx.Date.StartOf(DateTimeUnit.Month), DateTime.MaxValue);
            var jobsPlacement = new Placement(ClockTypes.JOBS, dob.AddDays(-304), dob.AddDays(-300));
            timeline.AddPlacement(movedDirectlyPlacement);
            timeline.AddPlacement(placement);
            timeline.AddPlacement(jobsPlacement);


            var ruleContext = new RuleContext { EvaluationMonth = RulesEnginePlacementTests._appCtx.Date };

            this._session.Insert(timeline);
            this._session.Insert(ruleContext);

            // have a child born exactly 294 month after 
            this._session.Insert(child1);

            // Act
            this._session.Fire();

            // Assert
            var facts = this._session.Query<RuleContext>().ToList();
            facts.ShouldHaveSingleItem(); 
            var result = facts.First();
            result.CmcShouldTickPreviousPlacement.ShouldNotBeNull();
            result.CmcShouldTickPreviousPlacement.Value.ShouldBeFalse();

            result.TimelimitType.ShouldNotBeNull();
            result.TimelimitType.Value.ShouldBe(ClockTypes.CMC | ClockTypes.Federal);

            result.ShouldCreateOpcTicks.GetValueOrDefault().ShouldBeTrue();

            var opcFacts = this._session.Query<OpcTick>();
            opcFacts.Count().ShouldBe(0); //engine should remove OPC only ticks 
        }

        #endregion

        #region Payment/Federal Tests
        [TestMethod]
        public void Tick_Should_Be_Federal_with_regular_payment()
        {
            var timeline = new Timeline(RulesEnginePlacementTests._appCtx);
            var placement = new Placement(ClockTypes.CSJ, RulesEnginePlacementTests._appCtx.Date.StartOf(DateTimeUnit.Month), DateTime.MaxValue);
            timeline.AddPlacement(placement);
            var payment = new Payment()
            {
                EffectivePaymentMonth = RulesEnginePlacementTests._appCtx.Date.AddMonths(1),
                OriginalPaymentAmount = 650.13m,
                OrignalCheckAmount = 650.13m,
                AdjustedNetAmount = 650.13m,
                PayPeriodBeginDate = RulesEnginePlacementTests._appCtx.Date.AddMonths(-1).AddDays(15),
                PayPeriodEndDate = RulesEnginePlacementTests._appCtx.Date.AddDays(15)
            };

            var ruleContext = new RuleContext {EvaluationMonth = RulesEnginePlacementTests._appCtx.Date,};

            this._session.Insert(timeline);
            this._session.Insert(ruleContext);
            this._session.Insert(payment);


            // Act
            this._session.Fire();

            // Assert
            var facts = this._session.Query<RuleContext>().ToList();
            facts.ShouldHaveSingleItem();
            var result = facts.First();
            result.TimelimitType.ShouldNotBeNull();
            result.TimelimitType.GetValueOrDefault().HasFlag(ClockTypes.Federal).ShouldBeTrue();
        }

        [TestMethod]
        public void Tick_Should_Be_Federal_with_partial_sanction()
        {
            var timeline = new Timeline(RulesEnginePlacementTests._appCtx);
            var placement = new Placement(ClockTypes.CSJ, RulesEnginePlacementTests._appCtx.Date.StartOf(DateTimeUnit.Month), DateTime.MaxValue);
            timeline.AddPlacement(placement);
            var payment = new Payment()
            {
                EffectivePaymentMonth = RulesEnginePlacementTests._appCtx.Date.AddMonths(1),
                OriginalPaymentAmount = 650.13m,
                OrignalCheckAmount = 1.40m,
                AdjustedNetAmount = 650.13m,
                PayPeriodBeginDate = RulesEnginePlacementTests._appCtx.Date.AddMonths(-1).AddDays(15),
                PayPeriodEndDate = RulesEnginePlacementTests._appCtx.Date.AddDays(15)
            };

            var ruleContext = new RuleContext { EvaluationMonth = RulesEnginePlacementTests._appCtx.Date, };

            this._session.Insert(timeline);
            this._session.Insert(ruleContext);
            this._session.Insert(payment);


            // Act
            this._session.Fire();

            // Assert
            var facts = this._session.Query<RuleContext>().ToList();
            facts.ShouldHaveSingleItem();
            var result = facts.First();
            result.TimelimitType.ShouldNotBeNull();
            result.TimelimitType.GetValueOrDefault().HasFlag(ClockTypes.Federal).ShouldBeTrue();
        }

        public void Tick_Should_Be_Federal_with_delayed_payment()
        {
            var timeline = new Timeline(RulesEnginePlacementTests._appCtx);
            var placement = new Placement(ClockTypes.CSJ, RulesEnginePlacementTests._appCtx.Date.AddMonths(-1).AddDays(20), DateTime.MaxValue);
            timeline.AddPlacement(placement);
            var delayedPayment = new Payment()
            {
                EffectivePaymentMonth = RulesEnginePlacementTests._appCtx.Date.AddMonths(1).AddDays(7),
                OriginalPaymentAmount = 650.13m,
                OrignalCheckAmount = 1.40m,
                AdjustedNetAmount = 650.13m,
                PayPeriodBeginDate = RulesEnginePlacementTests._appCtx.Date.AddMonths(-1).AddDays(15),
                PayPeriodEndDate = RulesEnginePlacementTests._appCtx.Date.AddDays(15)
            };

            var ruleContext = new RuleContext { EvaluationMonth = RulesEnginePlacementTests._appCtx.Date, };

            this._session.Insert(timeline);
            this._session.Insert(ruleContext);
            this._session.Insert(delayedPayment);


            // Act
            this._session.Fire();

            // Assert
            var facts = this._session.Query<RuleContext>().ToList();
            facts.ShouldHaveSingleItem();
            var result = facts.First();
            result.TimelimitType.ShouldNotBeNull();
            result.TimelimitType.GetValueOrDefault().HasFlag(ClockTypes.Federal).ShouldBeTrue();
        }

        public void Tick_Should_Not_Be_Federal_with_delayed_sanctioned_payment()
        {
            var timeline = new Timeline(RulesEnginePlacementTests._appCtx);
            var placement = new Placement(ClockTypes.CSJ, RulesEnginePlacementTests._appCtx.Date.AddMonths(-1).AddDays(20), DateTime.MaxValue);
            timeline.AddPlacement(placement);
            var delayedSanctionedPayment = new Payment()
            {
                EffectivePaymentMonth = RulesEnginePlacementTests._appCtx.Date.AddMonths(1),
                OriginalPaymentAmount = 650.13m,
                OrignalCheckAmount = 0m,
                AdjustedNetAmount = 0m,
                PayPeriodBeginDate = RulesEnginePlacementTests._appCtx.Date.AddMonths(-1).AddDays(15),
                PayPeriodEndDate = RulesEnginePlacementTests._appCtx.Date.AddDays(15)
            };

            var ruleContext = new RuleContext { EvaluationMonth = RulesEnginePlacementTests._appCtx.Date, };

            this._session.Insert(timeline);
            this._session.Insert(ruleContext);
            this._session.Insert(delayedSanctionedPayment);


            // Act
            this._session.Fire();

            // Assert
            var facts = this._session.Query<RuleContext>().ToList();
            facts.ShouldHaveSingleItem();
            var result = facts.First();
            result.TimelimitType.ShouldNotBeNull();
            result.TimelimitType.GetValueOrDefault().HasFlag(ClockTypes.Federal).ShouldBeFalse();
        }

        [TestMethod]
        public void Tick_Should_Not_Be_Federal_with_Full_sanction()
        {
            var timeline = new Timeline(RulesEnginePlacementTests._appCtx);
            var placement = new Placement(ClockTypes.CSJ, RulesEnginePlacementTests._appCtx.Date.StartOf(DateTimeUnit.Month), DateTime.MaxValue);
            timeline.AddPlacement(placement);
            // payment without adjustment from pulldown
            var payment1 = new Payment()
            {
                EffectivePaymentMonth = RulesEnginePlacementTests._appCtx.Date.AddMonths(1),
                OriginalPaymentAmount = 650.13m,
                OrignalCheckAmount = 0,
                AdjustedNetAmount = 650.13m,
                PayPeriodBeginDate = RulesEnginePlacementTests._appCtx.Date.AddMonths(-1).AddDays(15),
                PayPeriodEndDate = RulesEnginePlacementTests._appCtx.Date.AddDays(15)
            };
            // payment with adjustment from pulldown
            var payment2 = new Payment()
            {
                EffectivePaymentMonth = RulesEnginePlacementTests._appCtx.Date.AddMonths(1),
                OriginalPaymentAmount = 650.13m,
                OrignalCheckAmount = 0,
                AdjustedNetAmount = 0,
                PayPeriodBeginDate = RulesEnginePlacementTests._appCtx.Date.AddMonths(-1).AddDays(15),
                PayPeriodEndDate = RulesEnginePlacementTests._appCtx.Date.AddDays(15)
            };

            var ruleContext = new RuleContext { EvaluationMonth = RulesEnginePlacementTests._appCtx.Date, };

            this._session.Insert(timeline);
            this._session.Insert(ruleContext);
            this._session.Insert(payment1);
            this._session.Insert(payment2);


            // Act
            this._session.Fire();

            // Assert
            var facts = this._session.Query<RuleContext>().ToList();
            facts.ShouldHaveSingleItem();
            var result = facts.First();
            result.TimelimitType.ShouldNotBeNull();
            result.TimelimitType.GetValueOrDefault().HasFlag(ClockTypes.Federal).ShouldBeFalse();
        }

        [TestMethod]
        public void Tick_Should_Not_Be_Federal_with_Tnp_Placement()
        {
            var timeline = new Timeline(RulesEnginePlacementTests._appCtx);
            var placement = new Placement(ClockTypes.TNP, RulesEnginePlacementTests._appCtx.Date.StartOf(DateTimeUnit.Month), DateTime.MaxValue);
            timeline.AddPlacement(placement);

            var ruleContext = new RuleContext { EvaluationMonth = RulesEnginePlacementTests._appCtx.Date, };

            this._session.Insert(timeline);
            this._session.Insert(ruleContext);

            // Act
            this._session.Fire();

            // Assert
            var facts = this._session.Query<RuleContext>().ToList();
            facts.ShouldHaveSingleItem();
            var result = facts.First();
            result.TimelimitType.ShouldNotBeNull();
            result.TimelimitType.GetValueOrDefault().HasFlag(ClockTypes.Federal).ShouldBeFalse();
        }

        [TestMethod]
        public void Tick_Should_Not_Be_Federal_with_Tmp_Placement()
        {
            var timeline = new Timeline(RulesEnginePlacementTests._appCtx);
            var placement = new Placement(ClockTypes.TMP, RulesEnginePlacementTests._appCtx.Date.StartOf(DateTimeUnit.Month), DateTime.MaxValue);
            timeline.AddPlacement(placement);

            var ruleContext = new RuleContext { EvaluationMonth = RulesEnginePlacementTests._appCtx.Date, };

            this._session.Insert(timeline);
            this._session.Insert(ruleContext);

            // Act
            this._session.Fire();

            // Assert
            var facts = this._session.Query<RuleContext>().ToList();
            facts.ShouldHaveSingleItem();
            var result = facts.First();
            result.TimelimitType.ShouldNotBeNull();
            result.TimelimitType.GetValueOrDefault().HasFlag(ClockTypes.Federal).ShouldBeFalse();

        }

        [TestMethod]
        public void Tick_Should_Be_Federal_with_Csj_to_Tnp_Placement()
        {
            var timeline = new Timeline(RulesEnginePlacementTests._appCtx);
            var placement1 = new Placement(ClockTypes.CSJ, RulesEnginePlacementTests._appCtx.Date.StartOf(DateTimeUnit.Month), new DateTime(RulesEnginePlacementTests._appCtx.Date.Year, RulesEnginePlacementTests._appCtx.Date.Month,15));
            var placement2 = new Placement(ClockTypes.TNP, placement1.DateRange.End.AddDays(1), DateTime.MaxValue);
            timeline.AddPlacement(placement1);
            timeline.AddPlacement(placement2);

            var ruleContext = new RuleContext { EvaluationMonth = RulesEnginePlacementTests._appCtx.Date };

            this._session.Insert(timeline);
            this._session.Insert(ruleContext);

            // Act
            this._session.Fire();

            // Assert
            var facts = this._session.Query<RuleContext>().ToList();
            facts.ShouldHaveSingleItem();
            var result = facts.First();
            result.TimelimitType.ShouldNotBeNull();
            result.TimelimitType.GetValueOrDefault().HasFlag(ClockTypes.Federal).ShouldBeTrue();

        }

        [TestMethod]
        public void Tick_Should_Be_Federal_with_Csj_to_Tmp_Placement()
        {
            var timeline = new Timeline(RulesEnginePlacementTests._appCtx);
            var placement1 = new Placement(ClockTypes.CSJ, RulesEnginePlacementTests._appCtx.Date.StartOf(DateTimeUnit.Month), new DateTime(RulesEnginePlacementTests._appCtx.Date.Year, RulesEnginePlacementTests._appCtx.Date.Month, 15));
            var placement2 = new Placement(ClockTypes.TMP, placement1.DateRange.End.AddDays(1), DateTime.MaxValue);
            timeline.AddPlacement(placement1);
            timeline.AddPlacement(placement2);

            var ruleContext = new RuleContext { EvaluationMonth = RulesEnginePlacementTests._appCtx.Date };

            this._session.Insert(timeline);
            this._session.Insert(ruleContext);

            // Act
            this._session.Fire();

            // Assert
            var facts = this._session.Query<RuleContext>().ToList();
            facts.ShouldHaveSingleItem();
            var result = facts.First();
            result.TimelimitType.ShouldNotBeNull();
            result.TimelimitType.GetValueOrDefault().HasFlag(ClockTypes.Federal).ShouldBeTrue();

        }

        [TestMethod]
        public void Tick_should_not_be_federal_when_alien()
        {
            var timeline = new Timeline(RulesEnginePlacementTests._appCtx);
            var placement = new Placement(ClockTypes.CSJ, RulesEnginePlacementTests._appCtx.Date.StartOf(DateTimeUnit.Month), DateTime.MaxValue);
            timeline.AddPlacement(placement);


            var ruleContext = new RuleContext { EvaluationMonth = RulesEnginePlacementTests._appCtx.Date, };

            // expired primary and other parent alienStatuses
            var alienStatus1 = new AlienStatus(RulesEnginePlacementTests._appCtx.Date.AddMonths(-3), RulesEnginePlacementTests._appCtx.Date);
            this._session.Insert(alienStatus1);
            this._session.Insert(timeline);
            this._session.Insert(ruleContext);


            // Act
            this._session.Fire();

            // Assert
            var facts = this._session.Query<RuleContext>().ToList();
            facts.ShouldHaveSingleItem();
            var result = facts.First();
            result.TimelimitType.ShouldNotBeNull();
            result.TimelimitType.Value.HasFlag(ClockTypes.Federal).ShouldBeFalse();
        }

        [TestMethod]
        public void Tick_should_be_federal_when_alien_status_expired()
        {
            var timeline = new Timeline(RulesEnginePlacementTests._appCtx);
            var placement = new Placement(ClockTypes.CSJ, RulesEnginePlacementTests._appCtx.Date.StartOf(DateTimeUnit.Month), DateTime.MaxValue);
            timeline.AddPlacement(placement);


            var ruleContext = new RuleContext { EvaluationMonth = RulesEnginePlacementTests._appCtx.Date, };

            // expired primary and other parent alienStatuses
            var alienStatus1 = new AlienStatus(RulesEnginePlacementTests._appCtx.Date.AddMonths(-3), RulesEnginePlacementTests._appCtx.Date.AddMonths(-2));
            this._session.Insert(alienStatus1);
            this._session.Insert(timeline);
            this._session.Insert(ruleContext);


            // Act
            this._session.Fire();

            // Assert
            var facts = this._session.Query<RuleContext>().ToList();
            facts.ShouldHaveSingleItem();
            var result = facts.First();
            result.TimelimitType.ShouldNotBeNull();
            result.TimelimitType.Value.HasFlag(ClockTypes.Federal).ShouldBeTrue();
        }

        #endregion

        #region TEMP placement Test

        [TestMethod]
        public void RulesEngine_should_Create_TMP_ClockTypes()
        {
            // Arrange

            var timeline = new Timeline(RulesEnginePlacementTests._appCtx);
            var firstPlacement = new Placement(ClockTypes.TMP, RulesEnginePlacementTests._appCtx.Date.StartOf(DateTimeUnit.Month).AddDays(1), DateTime.MaxValue);
            timeline.AddPlacement(firstPlacement);

            var ruleContext = new RuleContext();
            ruleContext.EvaluationMonth = RulesEnginePlacementTests._appCtx.Date;

            this._session.Insert(timeline);
            this._session.Insert(ruleContext);

            // Act
            this._session.Fire();

            // Assert
            var facts = this._session.Query<RuleContext>().ToList();
            facts.ShouldHaveSingleItem();
            var result = facts.First();
            result.TimelimitType.GetValueOrDefault().HasAnyFlags(ClockTypes.TEMP).ShouldBeTrue();
            result.TimelimitType.GetValueOrDefault().HasAnyFlags(ClockTypes.TMP).ShouldBeTrue();
        }



        [TestMethod]
        public void RulesEngine_should_Create_TNP_ClockTypes()
        {
            // Arrange

            var timeline = new Timeline(RulesEnginePlacementTests._appCtx);
            var firstPlacement = new Placement(ClockTypes.TNP, RulesEnginePlacementTests._appCtx.Date.StartOf(DateTimeUnit.Month).AddDays(1), DateTime.MaxValue);
            timeline.AddPlacement(firstPlacement);

            var ruleContext = new RuleContext();
            ruleContext.EvaluationMonth = RulesEnginePlacementTests._appCtx.Date;

            this._session.Insert(timeline);
            this._session.Insert(ruleContext);

            // Act
            this._session.Fire();

            // Assert
            var facts = this._session.Query<RuleContext>().ToList();
            facts.ShouldHaveSingleItem();
            var result = facts.First();
            result.TimelimitType.GetValueOrDefault().HasAnyFlags(ClockTypes.TEMP).ShouldBeTrue();
            result.TimelimitType.GetValueOrDefault().HasAnyFlags(ClockTypes.TNP).ShouldBeTrue();


        }
        #endregion

        #region Placement Switching Tests
        [TestMethod]
        public void RulesEngine_Should_Determine_ClockType_When_Switching_in_same_month()
        {
            // Arrange

            var timeline = new Timeline(RulesEnginePlacementTests._appCtx);
            var firstPlacement = new Placement(ClockTypes.CSJ, RulesEnginePlacementTests._appCtx.Date.StartOf(DateTimeUnit.Month), RulesEnginePlacementTests._appCtx.Date.StartOf(DateTimeUnit.Month));
            var secondPlacement = new Placement(ClockTypes.W2T, RulesEnginePlacementTests._appCtx.Date.StartOf(DateTimeUnit.Month).AddDays(1), DateTime.MaxValue);
            timeline.AddPlacement(firstPlacement);
            timeline.AddPlacement(secondPlacement);

            var ruleContext = new RuleContext();
            ruleContext.EvaluationMonth = RulesEnginePlacementTests._appCtx.Date;

            this._session.Insert(timeline);
            this._session.Insert(ruleContext);

            // Act
            this._session.Fire();

            // Assert
            var facts = this._session.Query<RuleContext>().ToList();
            facts.ShouldHaveSingleItem();
            var result = facts.First();
            result.TimelimitType.GetValueOrDefault().HasAnyFlags(ClockTypes.W2T).ShouldBeTrue();


            // Try ending in the current month instead of "open"
            timeline.Placements.Clear();
            secondPlacement.DateRange = new DateTimeRange(secondPlacement.DateRange.Start, secondPlacement.DateRange.Start.AddDays(10));
            this._session.Update(timeline);
            this._session.Fire();

            // Assert
            facts = this._session.Query<RuleContext>().ToList();
            facts.ShouldHaveSingleItem();
            result = facts.First();
            result.TimelimitType.GetValueOrDefault().HasAnyFlags(ClockTypes.W2T).ShouldBeTrue();


        }

        [TestMethod]
        public void RulesEngine_Should_Determine_ClockType_When_Placed_At_end_of_same_month()
        {
            // Arrange

            var timeline = new Timeline(RulesEnginePlacementTests._appCtx);
            var firstPlacement = new Placement(ClockTypes.CSJ, RulesEnginePlacementTests._appCtx.Date.EndOf(DateTimeUnit.Month), RulesEnginePlacementTests._appCtx.Date.AddMonths(1));
            timeline.AddPlacement(firstPlacement);

            var ruleContext = new RuleContext();
            ruleContext.EvaluationMonth = RulesEnginePlacementTests._appCtx.Date;

            this._session.Insert(timeline);
            this._session.Insert(ruleContext);

            // Act
            this._session.Fire();

            // Assert
            var facts = this._session.Query<RuleContext>().ToList();
            facts.ShouldHaveSingleItem();
            var result = facts.First();
            result.TimelimitType.GetValueOrDefault().HasAnyFlags(ClockTypes.CSJ).ShouldBeTrue();

        }

        [TestMethod]
        public void RulesEngine_Should_Determine_ClockType_When_switching_to_unpaid_placement()
        {
            // Arrange

            var timeline = new Timeline(RulesEnginePlacementTests._appCtx);
            var firstPlacement = new Placement(ClockTypes.CSJ, RulesEnginePlacementTests._appCtx.Date.StartOf(DateTimeUnit.Month), RulesEnginePlacementTests._appCtx.Date.EndOf(DateTimeUnit.Month).AddDays(-2));
            var secondPlacement = new Placement("CMF", RulesEnginePlacementTests._appCtx.Date.EndOf(DateTimeUnit.Month).AddDays(-1), DateTime.MaxValue);
            timeline.AddPlacement(firstPlacement);
            timeline.AddPlacement(secondPlacement);

            var ruleContext = new RuleContext();
            ruleContext.EvaluationMonth = RulesEnginePlacementTests._appCtx.Date;

            this._session.Insert(timeline);
            this._session.Insert(ruleContext);

            // Act
            this._session.Fire();

            // Assert
            var facts = this._session.Query<RuleContext>().ToList();
            facts.ShouldHaveSingleItem();
            var result = facts.First();
            result.TimelimitType.GetValueOrDefault().HasAnyFlags(ClockTypes.CSJ).ShouldBeTrue();

        }

        [TestMethod]
        public void RulesEngine_Should_Not_Determine_ClockType_When_in_unpaid_placement()
        {
            // Arrange

            var timeline = new Timeline(RulesEnginePlacementTests._appCtx);
            var firstPlacement = new Placement("CMF", RulesEnginePlacementTests._appCtx.Date.StartOf(DateTimeUnit.Month), RulesEnginePlacementTests._appCtx.Date.EndOf(DateTimeUnit.Month).AddDays(-2));
            timeline.AddPlacement(firstPlacement);

            var ruleContext = new RuleContext();
            ruleContext.EvaluationMonth = RulesEnginePlacementTests._appCtx.Date;

            this._session.Insert(timeline);
            this._session.Insert(ruleContext);

            // Act
            this._session.Fire();

            // Assert
            var facts = this._session.Query<RuleContext>().ToList();
            facts.ShouldHaveSingleItem();
            var result = facts.First();
            result.TimelimitType.GetValueOrDefault().ShouldBe(ClockTypes.None);

        }
        #endregion

        #region First Employment Position Tests

        [TestMethod]
        public void RulesEngine_Should_Not_Determine_First_Employment_Position_when_last_placement_is_not_CMC()
        {
            // Arrange
            var firstDate = RulesEnginePlacementTests._appCtx.Date.AddDays(-309);

            var timeline = new Timeline(RulesEnginePlacementTests._appCtx);
            var placement = new Placement(ClockTypes.CSJ, RulesEnginePlacementTests._appCtx.Date.StartOf(DateTimeUnit.Month), DateTime.MaxValue);
            var jobsPlacement = new Placement(ClockTypes.JOBS, firstDate, firstDate.AddMonths(3));


            timeline.AddPlacement(placement);
            timeline.AddPlacement(jobsPlacement);

            var ruleContext = new RuleContext();
            ruleContext.EvaluationMonth = RulesEnginePlacementTests._appCtx.Date;

            this._session.Insert(timeline);
            this._session.Insert(ruleContext);

            // Act
            this._session.Fire();

            // Assert
            var facts = this._session.Query<RuleContext>().ToList();

            facts.ShouldHaveSingleItem();

            var result = facts.First();
            result.FirstNonCmcEmploymentPosition.ShouldNotBeNull();
            result.FirstNonCmcEmploymentPosition.PlacementType.ShouldBe(ClockTypes.JOBS);



        }

        [TestMethod]
        public void RulesEngine_Should_Determine_First_Employment_Position_when_last_placement_is_CMC()
        {
            // Arrange
            var firstDate = RulesEnginePlacementTests._appCtx.Date.AddDays(-309);

            var timeline = new Timeline(RulesEnginePlacementTests._appCtx);
            var placement = new Placement(ClockTypes.CMC, RulesEnginePlacementTests._appCtx.Date.StartOf(DateTimeUnit.Month), DateTime.MaxValue);
            var jobsPlacement = new Placement(ClockTypes.JOBS, firstDate, firstDate.AddMonths(3));
            var tnpPlacement = new Placement(ClockTypes.TNP, jobsPlacement.DateRange.End.AddDays(1), jobsPlacement.DateRange.End.AddDays(1));
            var tmpPlacement = new Placement(ClockTypes.TMP, tnpPlacement.DateRange.End.AddMonths(1), tnpPlacement.DateRange.End.AddMonths(1).AddDays(2));
            var csjPlacement = new Placement(ClockTypes.TMP, tmpPlacement.DateRange.End.AddMonths(1), tmpPlacement.DateRange.End.AddMonths(1).AddDays(2));
            var w2TPlacement = new Placement(ClockTypes.TMP, csjPlacement.DateRange.End.AddMonths(1), csjPlacement.DateRange.End.AddMonths(1).AddDays(2));

            timeline.AddPlacement(placement);
            timeline.AddPlacement(jobsPlacement);
            timeline.AddPlacement(tnpPlacement);
            timeline.AddPlacement(tmpPlacement);
            timeline.AddPlacement(csjPlacement);
            timeline.AddPlacement(w2TPlacement);


            var ruleContext = new RuleContext();
            ruleContext.EvaluationMonth = RulesEnginePlacementTests._appCtx.Date;

            this._session.Insert(timeline);
            this._session.Insert(ruleContext);

            // Act
            this._session.Fire();

            // Assert
            var facts = this._session.Query<RuleContext>().ToList();

            facts.ShouldHaveSingleItem();

            var result = facts.First();

            result.FirstNonCmcEmploymentPosition.ShouldNotBeNull();
            result.FirstNonCmcEmploymentPosition.ShouldBe(jobsPlacement);


        }

        [TestMethod]
        public void RulesEngine_Should_Determine_OLD_First_Employment_Position_when_last_placement_is_CMC()
        {
            // Arrange
            var firstDate = RulesEnginePlacementTests._appCtx.Date.AddYears(-7);

            var timeline = new Timeline(RulesEnginePlacementTests._appCtx);
            var placement = new Placement(ClockTypes.CMC, RulesEnginePlacementTests._appCtx.Date.StartOf(DateTimeUnit.Month), DateTime.MaxValue);
            var jobsPlacement = new Placement(ClockTypes.JOBS, firstDate, firstDate.AddMonths(3));
            var tnpPlacement = new Placement(ClockTypes.TNP, jobsPlacement.DateRange.End.AddDays(1), jobsPlacement.DateRange.End.AddDays(1));
            var tmpPlacement = new Placement(ClockTypes.TMP, tnpPlacement.DateRange.End.AddMonths(1), tnpPlacement.DateRange.End.AddMonths(1).AddDays(2));
            var csjPlacement = new Placement(ClockTypes.TMP, tmpPlacement.DateRange.End.AddMonths(1), tmpPlacement.DateRange.End.AddMonths(1).AddDays(2));
            var w2TPlacement = new Placement(ClockTypes.TMP, csjPlacement.DateRange.End.AddMonths(1), csjPlacement.DateRange.End.AddMonths(1).AddDays(2));

            timeline.AddPlacement(placement);
            timeline.AddPlacement(jobsPlacement);
            timeline.AddPlacement(tnpPlacement);
            timeline.AddPlacement(tmpPlacement);
            timeline.AddPlacement(csjPlacement);
            timeline.AddPlacement(w2TPlacement);


            var ruleContext = new RuleContext();
            ruleContext.EvaluationMonth = RulesEnginePlacementTests._appCtx.Date;

            this._session.Insert(timeline);
            this._session.Insert(ruleContext);

            // Act
            this._session.Fire();

            // Assert
            var facts = this._session.Query<RuleContext>().ToList();

            facts.ShouldHaveSingleItem();

            var result = facts.First();

            result.FirstNonCmcEmploymentPosition.ShouldNotBeNull();
            result.FirstNonCmcEmploymentPosition.ShouldBe(jobsPlacement);


        }


        #endregion

        #region OPC Tests

        [TestMethod]
        public void Should_Determine_OPC_Tick_is_needed_when_other_parent_in_ag()
        {
            var timeline = new Timeline(RulesEnginePlacementTests._appCtx);
            var placement = new Placement(ClockTypes.CSJ, RulesEnginePlacementTests._appCtx.Date.StartOf(DateTimeUnit.Month), DateTime.MaxValue);
            timeline.AddPlacement(placement);

            var otherParent = new AssistanceGroupMember() {RELATIONSHIP = "Wife"};

            var ruleContext = new RuleContext { EvaluationMonth = RulesEnginePlacementTests._appCtx.Date, };


            this._session.Insert(otherParent);
            this._session.Insert(timeline);
            this._session.Insert(ruleContext);


            // Act
            this._session.Fire();

            // Assert
            var facts = this._session.Query<RuleContext>().ToList();
            facts.ShouldHaveSingleItem();
            var result = facts.First();
            result.ShouldCreateOpcTicks.ShouldNotBeNull();
            result.ShouldCreateOpcTicks.Value.ShouldBeTrue();
        }

        [TestMethod]
        public void Should_Determine_Correct_OPC_Tick_is_needed_when_primary_in_CSJ()
        {
            var timeline = new Timeline(RulesEnginePlacementTests._appCtx);
            var placement = new Placement(ClockTypes.CSJ, RulesEnginePlacementTests._appCtx.Date.StartOf(DateTimeUnit.Month), DateTime.MaxValue);
            timeline.AddPlacement(placement);

            var otherParent = new AssistanceGroupMember() { RELATIONSHIP = "Husband" };

            var ruleContext = new RuleContext { EvaluationMonth = RulesEnginePlacementTests._appCtx.Date, };

            // expired primary and other parent alienStatuses
            var alienStatus1 = new AlienStatus(RulesEnginePlacementTests._appCtx.Date.AddMonths(-3), RulesEnginePlacementTests._appCtx.Date.AddMonths(-2));
            var alientStatus2 = new AlienStatus(alienStatus1.DateRange.Start,alienStatus1.DateRange.End);

            otherParent.AlienStatuses.Add(alientStatus2);
            this._session.Insert(alienStatus1);

            this._session.Insert(otherParent);
            this._session.Insert(timeline);
            this._session.Insert(ruleContext);


            // Act
            this._session.Fire();

            // Assert
            var facts = this._session.Query<OpcTick>().ToList();
            facts.ShouldNotBeEmpty();

            var parentTick = facts.FirstOrDefault(x => x.parent == otherParent);
            parentTick.ShouldNotBeNull();

            parentTick.TimelimitType.HasFlag(ClockTypes.OPC).ShouldBeTrue();
            parentTick.TimelimitType.HasFlag(ClockTypes.State).ShouldBeTrue();
            parentTick.TimelimitType.HasFlag(ClockTypes.Federal).ShouldBeTrue();

        }

        [TestMethod]
        public void Should_Determine_non_federal_OPC_Tick_is_needed_when_primary_is_alien()
        {
            var timeline = new Timeline(RulesEnginePlacementTests._appCtx);
            var placement = new Placement(ClockTypes.CSJ, RulesEnginePlacementTests._appCtx.Date.StartOf(DateTimeUnit.Month), DateTime.MaxValue);
            timeline.AddPlacement(placement);

            var otherParent = new AssistanceGroupMember() { RELATIONSHIP = "Husband" };

            var ruleContext = new RuleContext { EvaluationMonth = RulesEnginePlacementTests._appCtx.Date, };

            this._session.Insert(new AlienStatus(RulesEnginePlacementTests._appCtx.Date.AddMonths(-3), RulesEnginePlacementTests._appCtx.Date));
            this._session.Insert(otherParent);
            this._session.Insert(timeline);
            this._session.Insert(ruleContext);


            // Act
            this._session.Fire();

            // Assert
            var facts = this._session.Query<OpcTick>().ToList();
            facts.ShouldNotBeEmpty();

            var parentTick = facts.FirstOrDefault(x => x.parent == otherParent);
            parentTick.ShouldNotBeNull();

            parentTick.TimelimitType.HasFlag(ClockTypes.OPC).ShouldBeTrue();
            parentTick.TimelimitType.HasFlag(ClockTypes.Federal).ShouldBeFalse();

        }

        [TestMethod]
        public void Should_Determine_non_federal_OPC_Tick_is_needed_when_otherparent_is_alien()
        {
            var timeline = new Timeline(RulesEnginePlacementTests._appCtx);
            var placement = new Placement(ClockTypes.CSJ, RulesEnginePlacementTests._appCtx.Date.StartOf(DateTimeUnit.Month), DateTime.MaxValue);
            timeline.AddPlacement(placement);

            var otherParent = new AssistanceGroupMember() { RELATIONSHIP = "Wife" };

            var ruleContext = new RuleContext { EvaluationMonth = RulesEnginePlacementTests._appCtx.Date, };

            otherParent.AlienStatuses.Add(new AlienStatus(RulesEnginePlacementTests._appCtx.Date.AddMonths(-3), RulesEnginePlacementTests._appCtx.Date.AddMonths(3)));
            this._session.Insert(otherParent);
            this._session.Insert(timeline);
            this._session.Insert(ruleContext);


            // Act
            this._session.Fire();

            // Assert
            var facts = this._session.Query<OpcTick>().ToList();
            facts.ShouldNotBeEmpty();

            var parentTick = facts.FirstOrDefault(x => x.parent == otherParent);
            parentTick.ShouldNotBeNull();

            parentTick.TimelimitType.HasFlag(ClockTypes.OPC).ShouldBeTrue();
            parentTick.TimelimitType.HasFlag(ClockTypes.Federal).ShouldBeFalse();

        }

        [TestMethod]
        public void Should_Determine_non_federal_OPC_Tick_is_needed_when_both_parents_are_aliens()
        {
            var timeline = new Timeline(RulesEnginePlacementTests._appCtx);
            var placement = new Placement(ClockTypes.CSJ, RulesEnginePlacementTests._appCtx.Date.StartOf(DateTimeUnit.Month), DateTime.MaxValue);
            timeline.AddPlacement(placement);

            var otherParent = new AssistanceGroupMember() { RELATIONSHIP = "WIFE" };

            var ruleContext = new RuleContext { EvaluationMonth = RulesEnginePlacementTests._appCtx.Date, };
            var alientStatus1 = new AlienStatus(RulesEnginePlacementTests._appCtx.Date.AddMonths(-3), RulesEnginePlacementTests._appCtx.Date.AddMonths(3));
            var alientStatus2 = new AlienStatus(RulesEnginePlacementTests._appCtx.Date.AddMonths(-3), RulesEnginePlacementTests._appCtx.Date.AddMonths(3));
            otherParent.AlienStatuses.Add(alientStatus2);
            this._session.Insert(alientStatus1);
            this._session.Insert(otherParent);
            this._session.Insert(timeline);
            this._session.Insert(ruleContext);


            // Act
            this._session.Fire();

            // Assert
            var facts = this._session.Query<OpcTick>().ToList();
            facts.ShouldNotBeEmpty();

            var parentTick = facts.FirstOrDefault(x => x.parent == otherParent);
            parentTick.ShouldNotBeNull();

            parentTick.TimelimitType.HasFlag(ClockTypes.OPC).ShouldBeTrue();
            parentTick.TimelimitType.HasFlag(ClockTypes.Federal).ShouldBeFalse();

        }

        [TestMethod]
        public void Should_Determine_non_federal_OPC_Tick_is_needed_when_parents_are_not_married()
        {
            var timeline = new Timeline(RulesEnginePlacementTests._appCtx);
            var placement = new Placement(ClockTypes.CSJ, RulesEnginePlacementTests._appCtx.Date.StartOf(DateTimeUnit.Month), DateTime.MaxValue);
            timeline.AddPlacement(placement);

            var otherParent = new AssistanceGroupMember() { RELATIONSHIP = "friend" };

            var ruleContext = new RuleContext { EvaluationMonth = RulesEnginePlacementTests._appCtx.Date, };
            this._session.Insert(otherParent);
            this._session.Insert(timeline);
            this._session.Insert(ruleContext);


            // Act
            this._session.Fire();

            // Assert
            var facts = this._session.Query<OpcTick>().ToList();
            facts.ShouldNotBeEmpty();

            var parentTick = facts.FirstOrDefault(x => x.parent == otherParent);
            parentTick.ShouldNotBeNull();

            parentTick.TimelimitType.HasFlag(ClockTypes.OPC).ShouldBeTrue();
            parentTick.TimelimitType.HasFlag(ClockTypes.Federal).ShouldBeFalse();

        }

        #endregion 
    }
}
