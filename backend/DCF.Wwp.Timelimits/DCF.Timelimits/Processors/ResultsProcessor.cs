using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using Dcf.Wwp.Api.Library.Services;
using DCF.Timelimits.Core.Processors;
using DCF.Timelimits.Tasks;
using Dcf.Wwp.Api.Library.ViewModels;
using Dcf.Wwp.Data.Sql;
using Dcf.Wwp.Data.Sql.Model;
using Dcf.Wwp.Model.Interface.Core;
using DCF.Common.Configuration;
using DCF.Common.Dates;
using DCF.Common.Exceptions;
using DCF.Common.Extensions;
using DCF.Common.Logging;
using DCF.Common.Tasks;
using DCF.Timelimits.Rules.Domain;
using DCF.Timelimts.Service;
using EnumsNET;
using IsolationLevel = System.Transactions.IsolationLevel;

namespace DCF.Timelimits.Processors
{
    [BatchTaskProcess(Priority = 10)]
    public class ResultsProcessor : BatchTaskProcessBase<ProcessTimelimitEvaluationContext, ProcessTimelimitEvaluationResult>
    {
        private readonly ITimelimitService _timelimitService;
        private readonly IDb2TimelimitService _db2TimelimitService;
        private readonly ApplicationContext _applicationContext;

        public ResultsProcessor(ITimelimitService timelimitService, IDb2TimelimitService db2TimelimitService, ApplicationContext applicationContext
            )
        {
            this._timelimitService = timelimitService;
            this._db2TimelimitService = db2TimelimitService;
            this._applicationContext = applicationContext;
            Db2TimelimitService.TransCode = "WWPBATCH";
        }

        public override async Task<ProcessTimelimitEvaluationResult> Handle(ProcessTimelimitEvaluationContext context, CancellationToken token)
        {

            var model = context.CreatedMonth;

            var results = new ProcessTimelimitEvaluationResult()
            {
                PinNumber = context.PinNumber,
                Status = JobStatus.JobProcessingSuccess,
            };

            /* following block of code is in v.1.0.12 */
            var db2Synced = false;
            this._db2TimelimitService.IsSimulated = this._applicationContext.IsSimulation;

            // Supress if null or CMC only
            if (model != null && ((model.TimeLimitTypeId == (Int32)ClockTypes.CMC || model.TimeLimitTypeId == (Int32)(ClockTypes.OPC)) && !model.StateTimelimit.GetValueOrDefault() && !model.FederalTimeLimit.GetValueOrDefault()) == false)
            {
                WwpEnttitesTransientFaultDbConfiguration.SuspendExecutionStrategy = true;
                var executionStrategy = new DcfDbExecutionStrategy(5, TimeSpan.FromSeconds(30));

                executionStrategy.Execute(() =>
                {
                    // 1. Sync Data back to DB2
                    using (var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = IsolationLevel.ReadUncommitted }, TransactionScopeAsyncFlowOption.Enabled))
                    {
                        try
                        {
                            results.t0459Record = this._db2TimelimitService.Upsert(model, context.Participant, null);
                            scope.Complete();
                            db2Synced = true;
                        }
                        catch (Exception ex)
                        {
                            this._logger.ErrorException("Error syncing DB2 record record  for {@Participant}, {@data}", ex, context.PinNumber, context.CreatedMonth);
                            throw;
                        }
                    }
                });

                WwpEnttitesTransientFaultDbConfiguration.SuspendExecutionStrategy = false;
            }

            //// 2. Create snapshot for first of the next month
            context.Timeline.TimelineDate = _applicationContext.Date.AddMonths(1).StartOf(DateTimeUnit.Month);
            var snapShot = this._timelimitService.CreateTimelimitSummary(context.Timeline, context.Participant.Id);
            if (!this._applicationContext.IsSimulation)
                await this._timelimitService.SaveEntityAsync(snapShot, token).ConfigureAwait(false);

            results.Snapshot = snapShot;

            // 3. Process Placement closure
            Boolean? closedSuccesfully = null;
            var primaryPlacements = context.PlacementData.Where(x => x.HISTORY_CD == 0 && x.W2_EPISODE_BEGIN_DATE < this._applicationContext.Date.EndOf(DateTimeUnit.Month)).Select(x => new Tuple<SpTimelimitPlacementSummaryReturnModel, Placement>(x, new Placement(x.PLACEMENT_TYPE, x.PLACEMENT_BEGIN_DATE, x.PLACEMENT_END_MONTH) { PinNumber = x.PARTICIPANT }));

            var monthlyPlacements = primaryPlacements.Where(c => c.Item2.DateRange.Overlaps(new DateTimeRange(this._applicationContext.Date.StartOf(DateTimeUnit.Month), this._applicationContext.Date.EndOf(DateTimeUnit.Month))));
            var lastPlacementTuple = monthlyPlacements.GetMax(x => x.Item2.DateRange.End);
            if (lastPlacementTuple?.Item2 != null)
            {
                //var lastPlacement = lastPlacementTuple.Item2;
                results.LastPlacement = lastPlacementTuple.Item2;
                Boolean canClose = false;
                //var placementIsOpen = ApplicationContext.Current.Date.IsSame(DateTime.Now, DateTimeUnit.Month) ? lastPlacement.IsOpen : lastPlacement.DateRange.Contains(this._applicationContext.Date.StartOf(DateTimeUnit.Day));
                var placementIsOpen = results.LastPlacement.DateRange.End.IsAfter(ApplicationContext.Current.Date, DateTimeUnit.Month); //Don't try close ones that are already closed
                results.PlacementIsOpen = placementIsOpen;

                TimelineMonth timelineMonth = null;
                if (placementIsOpen)
                {
                    var placementCanCauseTick = results.LastPlacement.PlacementType.Value.HasAnyFlags(ClockTypes.PlacementTypes);
                    if (placementCanCauseTick)
                    {
                        var timelimit = context.CreatedMonth;
                        timelineMonth = timelimit == null ? null : TimelimitService.MapTimelimitToTimelineMonth(timelimit);

                        if (results.LastPlacement.PlacementType.Value.HasAnyFlags(ClockTypes.CMC))
                        {
                            if (timelineMonth?.ClockTypes.HasAnyFlags(ClockTypes.State) == true || timelineMonth?.ClockTypes.HasAnyFlags(ClockTypes.PlacementLimit) == true)
                            {
                                canClose = true;
                            }
                            else
                            {
                                canClose = false;
                            }
                        }
                        else
                        {
                            canClose = true;
                        }
                    }
                }


                if (canClose)
                {
                    var timeline = context.Timeline;
                    results.StateRemaining = timeline.GetRemainingMonths(ClockTypes.State);
                    results.PrimaryOutOfStateTicks = results.StateRemaining == 0;
                    //Boolean primaryOutOfClockTicks = false;
                    //Boolean secondaryOutOfStateTicks = false;
                    //Int32? otherStateParentRemaining = null;

                    if (results.LastPlacement.PlacementType.Value.HasAnyFlags(ClockTypes.CMC))
                    {
                        if (timelineMonth != null)
                        {
                            results.PrimaryOutOfStateTicks = results.PrimaryOutOfStateTicks.GetValueOrDefault() && timelineMonth.ClockTypes.HasAnyFlags(ClockTypes.State);
                            if (timelineMonth.ClockTypes.HasAnyFlags(ClockTypes.PlacementLimit))
                            {
                                results.ClockRemaining = timeline.GetRemainingMonths(timelineMonth.ClockTypes.CommonFlags(ClockTypes.PlacementLimit));
                                results.PrimaryOutOfClockTicks = results.ClockRemaining == 0;
                            }
                            else
                            {
                                results.PrimaryOutOfClockTicks = false; //CMC, STATE, FEDERAL
                            }
                        }
                        else
                        {
                            // TODO: this is an error condition. OPEN CMC but no tick. We should probably 
                            throw new DCFApplicationException("OPEN CMC placement with no tick not allowed!");
                        }
                        results.ClockUsed = timeline.GetUsedMonths(ClockTypes.CMC);
                        results.ClockMax = timeline.GetMaxMonths(ClockTypes.CMC);
                        results.ClockRemaining = timeline.GetRemainingMonths(ClockTypes.CMC);
                        results.ClockLimit = timeline.GetClockMax(ClockTypes.CMC);
                    }
                    else if (results.LastPlacement.PlacementType.Value.HasAnyFlags(ClockTypes.TEMP))
                    {
                        results.ClockRemaining = timeline.GetRemainingMonths(ClockTypes.TEMP);
                        results.PrimaryOutOfClockTicks = results.ClockRemaining == 0;
                        results.ClockUsed = timeline.GetUsedMonths(ClockTypes.TEMP);
                        results.ClockMax = timeline.GetMaxMonths(ClockTypes.TEMP);
                        results.ClockRemaining = timeline.GetRemainingMonths(ClockTypes.TEMP);
                        results.ClockLimit = timeline.GetClockMax(ClockTypes.TEMP);
                    }
                    else
                    {
                        results.ClockRemaining = timeline.GetRemainingMonths(results.LastPlacement.PlacementType.Value);
                        results.PrimaryOutOfClockTicks = results.ClockRemaining == 0;
                        results.ClockUsed = timeline.GetUsedMonths(results.LastPlacement.PlacementType.Value);
                        results.ClockMax = timeline.GetMaxMonths(results.LastPlacement.PlacementType.Value);
                        results.ClockRemaining = timeline.GetRemainingMonths(results.LastPlacement.PlacementType.Value);
                        results.ClockLimit = timeline.GetClockMax(results.LastPlacement.PlacementType.Value);
                    }

                    var otherParents = context.OtherAssistanceGroupMembers.Where(agMember => !agMember.IsChild() && agMember.ELIGIBILITY_PART_STATUS_CODE != "XA");
                    foreach (var parent in otherParents)
                    {
                        parent.Timeline.TimelineDate = timeline.TimelineDate;
                        results.OtherStateParentRemaining = parent.Timeline.GetRemainingMonths(ClockTypes.State);
                        results.SecondaryOutOfStateTicks = results.OtherStateParentRemaining == 0;
                        if (results.SecondaryOutOfStateTicks.GetValueOrDefault())
                        {
                            break;
                        }
                    }



                    var shouldClose = results.PrimaryOutOfClockTicks.GetValueOrDefault() || results.PrimaryOutOfStateTicks.GetValueOrDefault() || results.SecondaryOutOfStateTicks.GetValueOrDefault();
                    //var lastPlacementSummary = context.PlacementData.Where(x => x.HISTORY_CD == 0 && x.W2_EPISODE_END_DATE.GetValueOrDefault().IsSame(Db2TimelimitService.HighDate, DateTimeUnit.Day) && x.PLACEMENT_BEGIN_DATE.IsSameOrBefore(this._applicationContext.Date, DateTimeUnit.Day)).GetMax(x => x.HISTORY_SEQUENCE_NUMBER.GetValueOrDefault());
                    var lastPlacementSummary = lastPlacementTuple.Item1;
                    String existingFepId = lastPlacementSummary.MFWorkerId ?? " ";



                    results.PlacementType = lastPlacementSummary.PLACEMENT_TYPE;

                    results.ShouldClose = shouldClose;

                    if (shouldClose)
                    {
                        results.ClosedPlacement = lastPlacementSummary;

                        try
                        {
                            if (!this._applicationContext.IsSimulation)
                            {

                                await this._timelimitService.SpTimeLimitPlacementClosureAsync(
                                    lastPlacementSummary.CASE_NUMBER.GetValueOrDefault(),
                                    this._applicationContext.Date,
                                    "WWP",
                                    lastPlacementSummary.W2_EPISODE_BEGIN_DATE.GetValueOrDefault(),
                                    context.PinNumber,
                                    existingFepId,
                                    lastPlacementSummary.W2_EPISODE_END_DATE.GetValueOrDefault(),
                                    lastPlacementSummary.PLACEMENT_TYPE,
                                    lastPlacementSummary.PLACEMENT_BEGIN_DATE,
                                    existingFepId,
                                    this._applicationContext.Date.EndOf(DateTimeUnit.Month),
                                    lastPlacementSummary.PLACEMENT_TYPE, token).ConfigureAwait(false);
                                closedSuccesfully = true;
                            }

                            results.CaseNumber = lastPlacementSummary.CASE_NUMBER;
                            results.DatabaseDate = this._applicationContext.Date;
                            results.InputUserId = "WWP";
                            results.ExistingEpisodeBeginDate = lastPlacementSummary.W2_EPISODE_BEGIN_DATE;
                            results.ExistingFepId = existingFepId;
                            results.ExistingEpisodeEndDate = lastPlacementSummary.W2_EPISODE_END_DATE;
                            results.ExistingPlacementCode = lastPlacementSummary.PLACEMENT_TYPE;
                            results.ExistingPlacementBeginDate = lastPlacementSummary.PLACEMENT_BEGIN_DATE;

                        }
                        catch (Exception e)
                        {
                            closedSuccesfully = false;
                            this._logger.ErrorException("Error closing {placement} placement for pin number:{pin}", e, lastPlacementSummary, context.PinNumber);
                        }

                        results.ClosedSuccesfully = closedSuccesfully;
                    }
                }
            }

            // 4. Correct DB2 Count Mismatches / extension ends
            if (!db2Synced)
            {
                var ticksToUpdate = await this._timelimitService.GetLatestW2LimitsMonthsForEachClockTypeAsync(context.PinNumber, token).ConfigureAwait(false);
                var correctedTicks = this._db2TimelimitService.UpdateTicks0459(ticksToUpdate, context.Timeline, retainDate:false, fixingCountsForWwpDb2WriteBack:"Batch D2b Correction(s)");
                results.CorrectedTicks = correctedTicks;
            }


            return results;
        }
    }
}