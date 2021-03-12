using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using Dcf.Wwp.Data.Sql.Model;
using DCF.Common.Dates;
using DCF.Common.Extensions;
using DCF.Core;
using DCF.Timelimits.Rules.Actions;
using DCF.Timelimits.Rules.Domain;
using DCF.Timelimits.Rules.Specifications;
using NRules.Fluent.Dsl;
using EnumsNET;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NRules.RuleModel;

namespace DCF.Timelimits.Rules.Definitions
{

    #region Specs
    public class LastEmploymentPositionIsNullSpec : Specification<RuleContext>
    {
        public override Expression<Func<RuleContext, Boolean>> ToExpression()
        {
            return c => c.LastEmploymentPosition == null;
        }
    }

    public class LastEmploymentPositionIsNotNullSpec : Specification<RuleContext>
    {
        public override Expression<Func<RuleContext, Boolean>> ToExpression()
        {
            return c => c != null && c.LastEmploymentPosition != null && c.LastEmploymentPosition.PlacementType.HasValue && c.LastEmploymentPosition.PlacementType.Value != ClockTypes.None;
        }
    }

    public class LastEmploymentPositionIsSpec : AndSpecification<RuleContext>
    {

        public LastEmploymentPositionIsSpec(ClockTypes clockType)
            : base(new LastEmploymentPositionIsNotNullSpec(), new ExpressionSpecification<RuleContext>(c => c.LastEmploymentPosition.PlacementType.GetValueOrDefault().HasAnyFlags(clockType)))
        {
        }
    }

    public class FirstNonCmcEmploymentPositionIsNullSpec : Specification<RuleContext>
    {
        public override Expression<Func<RuleContext, Boolean>> ToExpression()
        {
            return c => c.FirstNonCmcEmploymentPosition == null;
        }
    }

    public class FirstNonCmcEmploymentPositionIsNotNullSpec : Specification<RuleContext>
    {
        public override Expression<Func<RuleContext, Boolean>> ToExpression()
        {
            return c => c != null && c.FirstNonCmcEmploymentPosition != null && c.FirstNonCmcEmploymentPosition.PlacementType.HasValue && c.FirstNonCmcEmploymentPosition.PlacementType.Value != ClockTypes.None;
        }
    }

    public class FirstNonCmcEmploymentPositionPositionIsSpec : AndSpecification<RuleContext>
    {

        public FirstNonCmcEmploymentPositionPositionIsSpec(ClockTypes clockType)
            : base(new FirstNonCmcEmploymentPositionIsNotNullSpec(), new ExpressionSpecification<RuleContext>(c => c.FirstNonCmcEmploymentPosition.PlacementType.GetValueOrDefault().HasAnyFlags(clockType.RemoveFlags(ClockTypes.CMC))))
        {
        }
    }

    //public class HasChildBornMoreThen304DaysFromDateSpec : Specification<IEnumerable<AssistanceGroupMember>>
    //{
    //    private DateTime _date;
    //    public HasChildBornMoreThen304DaysFromDateSpec(DateTime date)
    //    {

    //    }

    //    public override Expression<Func<IEnumerable<AssistanceGroupMember>, Boolean>> ToExpression()
    //    {
    //        return c => c.ToList().Any(x => x.RELATIONSHIP.ToUpper() == "CHILD" && x.BIRTH_DATE.HasValue && x.BIRTH_DATE.Value.DiffPrecise(this._date, DateTimeUnits.Days) > 304);

    //    }

    //    public override Boolean IsSatisfiedBy(IEnumerable<AssistanceGroupMember> obj)
    //    {
    //        var baseResult = base.IsSatisfiedBy(obj);
    //        return baseResult;
    //    }
    //}

    #endregion

    public class RuleContextOperation : ExpressionOperation<RuleContext>
    {
        public RuleContextOperation(Action<RuleContext> action) : base(action)
        {
        }
    }

    #region Context Setup RUles

    [Priority(0)]
    [Repeatability(RuleRepeatability.NonRepeatable)]
    public class LastEmploymentPositionRule : Rule
    {
        public override void Define()
        {
            RuleContext ruleContext = null;
            Timeline timeline = null;


            this.When()
                .Match(() => ruleContext, r => r.LastEmploymentPosition == null)
                .Match(() => timeline);

            this.Then()
                .Do(ctx => ctx.Update(ruleContext, r => this.UpdateLastEmploymentPosition(r, timeline)));
        }

        private void UpdateLastEmploymentPosition(RuleContext context, Timeline timeline)
        {
            var placement = timeline.LastEmploymentPlacement(context.EvaluationMonth) ?? new Placement(ClockTypes.None, DateTimeRange.Empty);
            context.LastEmploymentPosition = placement;
        }
    }

    #region Federal Rules

    public class FullSanctionFlagRule : Rule
    {
        public override void Define()
        {
            RuleContext ruleContext = null;
            IEnumerable<Payment> payments = null;

            this.When()
                .Match(() => ruleContext, r => !r.PaymentsAreFullySanctioned.HasValue)

                .Query(() => payments, x =>

                    x.Match<Payment>() //Run Rule always, don't filter
                        .Collect()
                );

            this.Then().Do(ctx => this.UpdatePaymentIsFullySanctioned(ruleContext, payments.ToList(), ctx));

        }

        public void UpdatePaymentIsFullySanctioned(RuleContext ruleContext, List<Payment> payments, IContext ctx)
        {

            var currentPayments = payments.Where(x => x.EffectivePaymentMonth.IsSame(ruleContext.EvaluationMonth.AddMonths(1), DateTimeUnit.Month)).ToList();
            var nonSanctionedPayments = currentPayments.Where(x => x.OriginalPaymentAmount > 0 && !x.SanctionedToZero()).ToList();
            var sanctionedPayments = currentPayments.Where(x => x.SanctionedToZero()).ToList();
            var paymentsAreFullySanctioned = !nonSanctionedPayments.Any() && sanctionedPayments.Any();

            ruleContext.PaymentsAreFullySanctioned = paymentsAreFullySanctioned;
            ctx.Update(ruleContext);
        }
    }

    public class AlienStatusFlagRule : Rule
    {
        public override void Define()
        {
            RuleContext ruleContext = null;
            IEnumerable<AlienStatus> alienStatuses = null;

            this.When()
                .Match(() => ruleContext, r => !r.IsAlien.HasValue)

                .Query(() => alienStatuses, x =>

                    x.Match<AlienStatus>() //Run Rule always, don't filter
                        .Collect()
                );

            this.Then().Do(ctx => this.UpdateIsAlien(ruleContext, alienStatuses.ToList(), ctx));

        }

        public void UpdateIsAlien(RuleContext ruleContext, List<AlienStatus> alienStatuses, IContext ctx)
        {
            var monthlyAlienStatuses = alienStatuses.Where(x => x.DateRange.End.IsSameOrAfter(ruleContext.EvaluationMonth, DateTimeUnit.Month)).ToList();
            var isAlienForThisMonth = monthlyAlienStatuses.Any();

            ruleContext.IsAlien = isAlienForThisMonth;
            ctx.Update(ruleContext);

        }
    }

    public class ShouldCreateOtherParentTickRule : Rule
    {
        public override void Define()
        {
            RuleContext ruleContext = null;
            IEnumerable<AssistanceGroupMember> parents = null;
            this.When()
                .Match(() => ruleContext, r => !r.ShouldCreateOpcTicks.HasValue)
                .Query(() => parents, x =>
                    x.Match<AssistanceGroupMember>()
                        .Where(y => !y.IsChild()).Collect()

                );

            this.Then()
                .Do(ctx => this.UpdateShouldCreateOPCTick(ruleContext, parents.ToList(), ctx));
        }

        public void UpdateShouldCreateOPCTick(RuleContext ruleContext, List<AssistanceGroupMember> parents, IContext ctx)
        {
            if (parents?.Any() == true)
            {
                ruleContext.ShouldCreateOpcTicks = true;
                ctx.Update(ruleContext);
                foreach (var parent in parents)
                {
                    ctx.Insert(new OpcTick() { parent = parent });
                }
            }
            else
            {
                ruleContext.ShouldCreateOpcTicks = false;
            }
        }
    }

    #endregion


    //[Repeatability(RuleRepeatability.NonRepeatable)]
    //public class PreviousPlacementRule : Rule
    //{
    //    public override void Define()
    //    {
    //        RuleContext ruleContext = null;
    //        Timeline timeline = null;


    //        this.When()
    //            .Match(() => ruleContext, r => r.PreviousPlacement == null)
    //            .Match(() => timeline);

    //        this.Then()
    //            .Do(ctx => ctx.Update(ruleContext, r => this.UpdatePreviousPlacement(r, timeline)));
    //    }

    //    public void UpdatePreviousPlacement(RuleContext ruleContext, Timeline timeline)
    //    {
    //        var previousPlacement = this.GetPreviousPlacement(timeline, ruleContext.LastEmploymentPosition, false);
    //    }
    //}

    [Priority(5)]
    public class FirstNonCmcEmploymentPositionRule : Rule
    {
        public override void Define()
        {
            RuleContext ruleContext = null;
            Timeline timeline = null;


            this.When()
                .Match(() => ruleContext, r => this.FirstNonCmcEmploymentPositionIsNull(r))
                .Match(() => timeline);
            //.Match(() => ruleContext, new LastEmploymentPositionIsNotNullSpec(), new LastEmploymentPositionIsSpec(ClockTypes.CMC));

            this.Then()
                .Do(ctx => ctx.Update(ruleContext, r => this.UpdateFirstNonCmcEmploymentPosition(r, timeline)));


        }
        private Boolean FirstNonCmcEmploymentPositionIsNull(RuleContext context)
        {
            return context.FirstNonCmcEmploymentPosition == null;
        }

        private void UpdateFirstNonCmcEmploymentPosition(RuleContext context, Timeline timeline)
        {
            var placement = timeline.FirstEmploymentPlacement(false) ?? new Placement(ClockTypes.None, DateTimeRange.Empty);
            context.FirstNonCmcEmploymentPosition = placement;
        }
    }

    #endregion

    #region CMC Setup Rules




    [Priority(10)]
    public class MovedDirectlyIntoCmc : Rule
    {

        public override void Define()
        {
            RuleContext ruleContext = null;
            Timeline timeline = null;

            this.When()
                .Match(() => ruleContext, r => !r.MovedDirectlyIntoCmc.HasValue, r => r.LastEmploymentPosition != null)
                .Match(() => timeline);

            this.Then().Do(ctx => this.UpdateMovedDirectlyIntoCmc(ruleContext, timeline, ctx));

        }

        public void UpdateMovedDirectlyIntoCmc(RuleContext context, Timeline timeline, IContext actionContext)
        {
            var previousPlacement = timeline.GetPreviousPlacement(context.LastEmploymentPosition, false);
            var movedDirectlyIntoCmc = previousPlacement != null && previousPlacement.PlacementType.GetValueOrDefault() != ClockTypes.None;
            context.MovedDirectlyIntoCmc = movedDirectlyIntoCmc && new LastEmploymentPositionIsSpec(ClockTypes.CMC).IsSatisfiedBy(context);
            actionContext.Update(context);
        }
    }

    [Priority(10)]
    public class HasChildBorn10monthsAfterPaidw2StartRule : Rule
    {
        public override void Define()
        {
            RuleContext ruleContext = null;
            IEnumerable<AssistanceGroupMember> children = null;

            this.When()
                .Match(() => ruleContext,
                    r => !ruleContext.HasChildBorn10monthsAfterPaidw2Start.HasValue,
                    r => ruleContext.LastEmploymentPosition != null)
                .Query(() => children, x => // Where assistance group has children!
                    x.Match<AssistanceGroupMember>()
                        .Where(y => y.IsChild())
                        .Collect()
                );

            this.Then()
                .Do(ctx => this.UpdateHasChildBorn10monthsAfterPaidw2Start(ruleContext, children, ctx));

        }

        public void UpdateHasChildBorn10monthsAfterPaidw2Start(RuleContext context, IEnumerable<AssistanceGroupMember> children, IContext ctx)
        {
            Boolean firstNonCmcEmploymentPositionIsPlacementType = context.FirstNonCmcEmploymentPosition != null && context.FirstNonCmcEmploymentPosition.PlacementType.GetValueOrDefault().HasAnyFlags(ClockTypes.PlacementTypes);

            var hasChildBornAfter304daysofw2Start = false;
            if (firstNonCmcEmploymentPositionIsPlacementType)
            {
                var eligbiltyDate = context.FirstNonCmcEmploymentPosition.DateRange.Start;
                var assistanceGroupMembers = children as IList<AssistanceGroupMember> ?? children.ToList();
                foreach (var child in assistanceGroupMembers)
                {
                    var isChild = child.RELATIONSHIP.ToUpper() == "CHILD";
                    var dayDiff = child.BIRTH_DATE?.DiffPrecise(eligbiltyDate, DateTimeUnits.Days);
                    var childIsBorn304plusDaysAftereligibility = child.BIRTH_DATE > eligbiltyDate && dayDiff > 304;
                    if (isChild && childIsBorn304plusDaysAftereligibility)
                    {
                        hasChildBornAfter304daysofw2Start = true;
                        break;
                    }
                }
            }

            context.HasChildBorn10monthsAfterPaidw2Start = hasChildBornAfter304daysofw2Start;
            ctx.Update(context);
        }
    }

    [Priority(15)]
    public class CmcShouldTickPreviousPlacementRule : Rule
    {
        public override void Define()
        {
            RuleContext ruleContext = null;
            IEnumerable<AssistanceGroupMember> children = null;
            Timeline timeline = null;
            this.When()
                .Match(() => timeline)
                .Match(() => ruleContext,
                        r => !ruleContext.CmcShouldTickPreviousPlacement.HasValue,
                        r => ruleContext.HasChildBorn10monthsAfterPaidw2Start.HasValue,
                        r => ruleContext.MovedDirectlyIntoCmc.HasValue,
                        r => ruleContext.LastEmploymentPosition != null)
                .Query(() => children, x => // Where assistance group has children!
                    x.Match<AssistanceGroupMember>()
                        .Where(y => y.IsChild())
                        .Collect()
                );

            this.Then()
                .Do(ctx => this.UpdateCmcShouldTickPreviousPlacement(ruleContext, children,timeline, ctx));

        }

        public void UpdateCmcShouldTickPreviousPlacement(RuleContext context, IEnumerable<AssistanceGroupMember> children, Timeline timeline, IContext ctx)
        {
            var isCmc = new LastEmploymentPositionIsSpec(ClockTypes.CMC).IsSatisfiedBy(context);
            //var firstNonCmcEmploymentPositionIsPlacementType = context.FirstNonCmcEmploymentPosition != null  && context.FirstNonCmcEmploymentPosition.PlacementType.GetValueOrDefault().HasAnyFlags(ClockTypes.PlacementTypes);

            //var hasChildBornAfter304daysofw2Start = false;
            //if (isCmc)
            //{
            //    var eligbiltyDate = context.FirstNonCmcEmploymentPosition.DateRange.Start;
            //    var assistanceGroupMembers = children as IList<AssistanceGroupMember> ?? children.ToList();
            //    foreach (var child in assistanceGroupMembers)
            //    {
            //        var isChild = child.RELATIONSHIP.ToUpper() == "CHILD";
            //        var dayDiff = child.BIRTH_DATE?.DiffPrecise(eligbiltyDate, DateTimeUnits.Days);
            //        var childIsBorn304plusDaysAftereligibility = dayDiff > 304;
            //        if (isChild && childIsBorn304plusDaysAftereligibility)
            //        {
            //            hasChildBornAfter304daysofw2Start = true;
            //            break;
            //        }
            //    }
            //}
            var previousPlacement = timeline.GetPreviousPlacement(context.LastEmploymentPosition, false);
            var previousPlacementIsPaidPlacement = previousPlacement?.PlacementType.GetValueOrDefault().HasAnyFlags(ClockTypes.PlacementLimit);
            context.CmcShouldTickPreviousPlacement = context.MovedDirectlyIntoCmc.GetValueOrDefault() && previousPlacementIsPaidPlacement.GetValueOrDefault() && context.HasChildBorn10monthsAfterPaidw2Start.GetValueOrDefault();
            if (isCmc)
            {
                ctx.Update(context);
            }
        }
    }

    #endregion


    //Most rules setup this rule's context
    [Priority(20)]
    public class CreateEmploymentPositionClockTypeRule : Rule
    {
        public override void Define()
        {
            RuleContext ruleContext = null;
            Timeline timeline = null;
            this.When()
                .Match(() => timeline)
                .Match(() => ruleContext,
                        r => !r.TimelimitType.HasValue,
                        new LastEmploymentPositionIsNotNullSpec(),
                        r => r.IsAlien.HasValue,
                        r => r.PaymentsAreFullySanctioned.HasValue,
                        r => r.CmcShouldTickPreviousPlacement.HasValue,
                        r => r.HadPreviousPaidPlacementInMonth.HasValue,
                        r => r.HasChildBorn10monthsAfterPaidw2Start.HasValue)

                .Match(() => ruleContext, r => r.LastEmploymentPosition != null);

            this.Then()
                .Do(ctx => ctx.Update(ruleContext, (r) => this.AddPlacementFlags(r, timeline)));

        }

        public void AddPlacementFlags(RuleContext context, Timeline timeline)
        {

            var placementFlagToAdd = context.LastEmploymentPosition.PlacementType.GetValueOrDefault().CommonFlags(ClockTypes.PlacementTypes);

            //add State
            placementFlagToAdd = placementFlagToAdd.CombineFlags(ClockTypes.State);


            // determine if Federal (Alien, or Payment Sanctioned. 
            var isNotFederal = (placementFlagToAdd.HasAnyFlags(ClockTypes.TEMP) && context.HadPreviousPaidPlacementInMonth == false) || context.IsAlien == true || context.PaymentsAreFullySanctioned == true;


            if (placementFlagToAdd.HasAnyFlags(ClockTypes.CMC))
            {
                // Determine if we are making a federal only CMC Tick (i.e. remove state)
                if (context.HasChildBorn10monthsAfterPaidw2Start == false)
                {
                    //CMC Is Not federal if we didn't move diretly from !
                    placementFlagToAdd = placementFlagToAdd.RemoveFlags(ClockTypes.State);
                }

                // CMC is federal only when moved directly from W2T/CSJ
                if (!isNotFederal)
                {
                    var previousPlacement = timeline.GetPreviousPlacement(context.LastEmploymentPosition, false);
                    if (previousPlacement == null || !previousPlacement.PlacementType.GetValueOrDefault().HasAnyFlags(ClockTypes.W2T | ClockTypes.CSJ))
                    {
                        isNotFederal = true;
                    }
                }
            }

            //add federal flag
            placementFlagToAdd = isNotFederal ? placementFlagToAdd : placementFlagToAdd.CombineFlags(ClockTypes.Federal);


            context.TimelimitType = context.TimelimitType.GetValueOrDefault().RemoveFlags(ClockTypes.PlacementTypes).CombineFlags(placementFlagToAdd);
        }
    }

    #region W2TRules

    //[Priority(20)]
    //[Repeatability(RuleRepeatability.NonRepeatable)]
    //public class CreateW2TRule : Rule
    //{
    //    public override void Define()
    //    {
    //        RuleContext ruleContext = null;
    //        this.When()
    //            .Match(() => ruleContext, r => r.LastPlacement != ClockTypes.None)
    //            .Or(x =>
    //            {
    //                x.Match(() => ruleContext, r => r.LastPlacement.HasAnyFlags(ClockTypes.W2T));

    //                //CMC Rule
    //                //x.Match(() => ruleContext, r => r.LastEmploymentPosition != null && r.LastEmploymentPosition.PlacementType.GetValueOrDefault().HasAnyFlags(ClockTypes.CMC) && true/*Moved Directly*/ );
    //            });
    //        this.Then()
    //            .Do(ctx => ctx.Update<RuleContext>(ruleContext, r => r.AddPlacementFlag(ClockTypes.W2T)));
    //    }
    //}

    #endregion

    #region CSJ Rules

    //    [Priority(20)]
    //[Repeatability(RuleRepeatability.NonRepeatable)]
    //public class CreateCSJRule : Rule
    //{
    //    public override void Define()
    //    {
    //        RuleContext ruleContext = null;
    //        this.When()
    //            .Match(() => ruleContext, r => r.LastPlacement != ClockTypes.None)
    //            .Or(x =>
    //            {
    //                x.Match(() => ruleContext, r => r.LastEmploymentPosition != null && r.LastEmploymentPosition.PlacementType.GetValueOrDefault().HasAnyFlags(ClockTypes.CSJ));
    //                //CMC Rule
    //                x.Match(() => ruleContext, r => r.LastEmploymentPosition != null && r.LastEmploymentPosition.PlacementType.GetValueOrDefault().HasAnyFlags(ClockTypes.CMC) && r.MovedDirectlyIntoCMC.GetValueOrDefault() /*Moved Directly*/ );
    //            });
    //        this.Then()
    //            .Do(ctx => ruleContext.AddPlacementFlag(ClockTypes.CSJ));
    //    }
    //}

    #endregion

    #region Temp Rules


    public class TempIsFederalWhenOtherEmploymentPositionUsedRule : Rule
    {
        public override void Define()
        {
            RuleContext ruleContext = null;
            Timeline timeline = null;
            this.When()
                .Match(() => timeline)
                .Match(() => ruleContext, r => !r.HadPreviousPaidPlacementInMonth.HasValue);
            this.Then()
                .Do(ctx => this.UpdateHadPreviousPaidPlacementInMonth(ruleContext, timeline, ctx));
        }

        public void UpdateHadPreviousPaidPlacementInMonth(RuleContext ruleContext, Timeline timeline, IContext ctx)
        {
            var monthlyPlacements = timeline.Placements[ruleContext.EvaluationMonth] ?? new List<Placement>();
            var otherPlacementTypes = monthlyPlacements
                .Where(x => x.PlacementType.GetValueOrDefault().HasAnyFlags(ClockTypes.PlacementLimit.RemoveFlags(ruleContext.LastEmploymentPosition.PlacementType.GetValueOrDefault()))).ToList();
            ruleContext.PreviousPlacement = otherPlacementTypes.GetMax(x => x.DateRange.End);
            ruleContext.HadPreviousPaidPlacementInMonth = otherPlacementTypes.Any();
            ctx.Update(ruleContext);
        }
    }

    #endregion

    #region CMC Rules

    [Priority(35)]
    public class CreateNonCmcPlacmentTickRule : Rule
    {
        public override void Define()
        {
            RuleContext ruleContext = null;
            Timeline timeline = null;
            this.When()
                .Match(() => timeline)
                .Match(() => ruleContext,
                r => r.CmcShouldTickPreviousPlacement == true,
                r => r.TimelimitType.HasValue
                //new LastEmploymentPositionIsNotNullSpec(),
                //new LastEmploymentPositionIsSpec(ClockTypes.CMC),
                //new FirstNonCMCEmploymentPositionPositionIsSpec(ClockTypes.PlacementTypes),
                //new HasChildBornMoreThen304DaysFromDateSpec(ruleContext.EvaluationMonth),
                //r=>r.MovedDirectlyIntoCMC.GetValueOrDefault()
                );

            this.Then()
                .Do(ctx => this.UpdateCmcTickToBePlacementTick(ruleContext, timeline, ctx));
        }

        public void UpdateCmcTickToBePlacementTick(RuleContext ruleContext, Timeline timeline, IContext actionCtx)
        {
            var previousPlacement = timeline.GetPreviousPlacement(ruleContext.LastEmploymentPosition, false);
            var contextClockType = ruleContext.TimelimitType;
            if (previousPlacement.PlacementType == ClockTypes.Other || previousPlacement.PlacementType == ClockTypes.None)
            {
                ruleContext.TimelimitType = ClockTypes.None;
                //Stop Evaluation
                actionCtx.Halt();
            }
            else
            {
                var clockType = contextClockType.GetValueOrDefault().RemoveFlags(ClockTypes.PlacementTypes).CombineFlags(previousPlacement.PlacementType.GetValueOrDefault());



                var shouldUpdate = contextClockType != clockType;
                ruleContext.TimelimitType = clockType;
                if (shouldUpdate)
                    actionCtx.Update(ruleContext);
            }

        }
    }
    #endregion

    #region OPC Rules

    //public class OpcTickShouldBeStateRule : Rule
    //{
    //    public override void Define()
    //    {
    //        RuleContext ruleContext = null;
    //        IEnumerable<OpcTick> otherParentTicks = null;
    //        this.When()
    //            .Match(() => ruleContext, r => r.ShouldCreateOpcTicks.HasValue, r => r.TimelimitType.GetValueOrDefault().HasAnyFlags(ClockTypes.PlacementTypes))
    //            .Query(() => otherParentTicks, x =>
    //                 x.Match<OpcTick>()
    //                     .Collect()
    //                     .Where(c => c.Any())

    //            );
    //        this.Then()
    //            .Do(ctx => this.UpdateOpcTickShouldBeState(ruleContext, otherParentTicks.ToList(), ctx));
    //    }

    //    public void UpdateOpcTickShouldBeState(RuleContext ruleContext, List<OpcTick> otherParentTicks, IContext ctx)
    //    {
    //        foreach (var opcTick in otherParentTicks)
    //        {
    //            var originalClockType = opcTick.TimelimitType;

    //            if (ruleContext.TimelimitType.GetValueOrDefault().HasFlag(ClockTypes.State))
    //            {
    //                opcTick.TimelimitType = opcTick.TimelimitType.CombineFlags(ClockTypes.State | ClockTypes.OPC);
    //            }

    //            if (opcTick.TimelimitType != originalClockType)
    //            {
    //                ctx.Update(opcTick);
    //            }
    //        }
    //    }
    //}

    public class UpdateOPCTicksRule : Rule
    {
        public override void Define()
        {
            RuleContext ruleContext = null;
            IEnumerable<OpcTick> otherParentTicks = null;
            this.When()
                .Match(() => ruleContext, r => r.ShouldCreateOpcTicks.HasValue, r => r.TimelimitType.GetValueOrDefault().HasAnyFlags(ClockTypes.PlacementTypes))
                .Query(() => otherParentTicks, x =>
                    x.Match<OpcTick>()
                        .Collect()
                        .Where(c => c.Any())
                );

            this.Then()
                .Do(ctx => this.UpdateOpcTickShouldBeFederal(ruleContext, otherParentTicks.ToList(), ctx));

        }

        public void UpdateOpcTickShouldBeFederal(RuleContext ruleContext, List<OpcTick> otherParentTicks, IContext ctx)
        {
            foreach (var opcTick in otherParentTicks)
            {
                var originalClockType = opcTick.TimelimitType;
                
                var isMarried = opcTick.parent.IsSpouse();
                var isAlien = ruleContext.IsAlien == true || opcTick.parent.AlienStatuses.Any(x => x.DateRange.Contains(ruleContext.EvaluationMonth.Date));
                var isPrimaryGettingFederal = ruleContext.TimelimitType.GetValueOrDefault().HasFlag(ClockTypes.Federal);

                // determine if it should be state
                if (isMarried && !isAlien && isPrimaryGettingFederal)
                {
                    opcTick.TimelimitType = opcTick.TimelimitType.CombineFlags(ClockTypes.Federal | ClockTypes.OPC);
                }

                //determine if it should be federal
                if (ruleContext.TimelimitType.GetValueOrDefault().HasFlag(ClockTypes.State))
                {
                    opcTick.TimelimitType = opcTick.TimelimitType.CombineFlags(ClockTypes.State | ClockTypes.OPC);
                }

                // remove OPC tick if non-federal/non-state OPC tick 
                if (!opcTick.TimelimitType.HasAnyFlags(ClockTypes.Federal | ClockTypes.State))
                {
                    opcTick.TimelimitType = ClockTypes.None;
                    ctx.Retract(opcTick);
                }
                else if (opcTick.TimelimitType != originalClockType)
                {
                    ctx.Update(opcTick);
                }

            }
        }
    }

    #endregion


    public class OpcTick
    {
        public ClockTypes TimelimitType { get; set; }
        public AssistanceGroupMember parent { get; set; }
    }

    public class RuleContext
    {
        public DateTime EvaluationMonth { get; set; }
        public Placement PreviousPlacement { get; set; }
        public Placement LastEmploymentPosition { get; set; }
        public Placement FirstNonCmcEmploymentPosition { get; set; }
        public Boolean? HadPreviousPaidPlacementInMonth { get; set; }
        public List<ClockTypes> Placements { get; set; }
        public Boolean? MovedDirectlyIntoCmc { get; set; }
        public Boolean? HasChildBorn10monthsAfterPaidw2Start { get; set; }
        public Boolean? CmcShouldTickPreviousPlacement { get; set; }
        public Boolean? IsAlien { get; set; }
        public Boolean? PaymentsAreFullySanctioned { get; set; }
        public ClockTypes? TimelimitType
        {
            get;
            set;
        }
        public Boolean? ShouldCreateOpcTicks { get; set; }



        public RuleContext UpdateCmcShouldTickPreviousPlacement(Boolean value)
        {
            this.CmcShouldTickPreviousPlacement = value;
            return this;
        }


        public void Reset()
        {
            var evaluationMonth = this.EvaluationMonth;
            foreach (var prop in this.GetType().GetProperties())
            {
                if (prop.CanWrite)
                {
                    prop.SetValue(this, prop.PropertyType.GetDefaultValue());
                }
            }
            this.EvaluationMonth = evaluationMonth;
        }
    }

}