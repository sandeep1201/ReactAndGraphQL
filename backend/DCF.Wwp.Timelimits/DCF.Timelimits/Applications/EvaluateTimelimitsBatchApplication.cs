using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Autofac;
using Dcf.Wwp.Api.Library.ViewModels;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Repository;
using Dcf.Wwp.Model.Interface.Services;
using DCF.Common.Dates;
using DCF.Common.Extensions;
using DCF.Common.Logging;
using DCF.Common.Tasks;
using DCF.Core;
using DCF.Timelimits.Core.Processors;
using DCF.Timelimits.Core.Tasks;
using DCF.Timelimits.Rules.Definitions;
using DCF.Timelimits.Rules.Domain;
using DCF.Timelimits.Rules.Scripting;
using DCF.Timelimits.Tasks;
using DCF.Timelimts.Service;
using EnumsNET;
using MediatR;
using Nito.AsyncEx;
using NRules;
using NRules.Fluent;
using OfficeOpenXml;
using Serilog;
using Serilog.Context;

namespace DCF.Timelimits
{
    public class EvaluateTimelimitsBatchApplication : BatchApplication<Decimal, EvaluateTimelimitsTaskContext, EvaluateTimelimitsTaskResult>
    {


        internal List<Decimal> pinsToProcess = new List<decimal>();

        public EvaluateTimelimitsBatchApplication(ApplicationContext context) : base(context)
        {

            //this.OnInitialized = this.Initialize;
            this.OnContainerInitialized = this.ContainerInitialized;
        }

        private void ContainerInitialized(ContainerBuilder containerBuilder)
        {
            ISessionFactory ruleFactory = this.CompileTimelimitRuleNetwork();
            containerBuilder.RegisterInstance(ruleFactory).As<ISessionFactory>().SingleInstance();
        }

        internal ISessionFactory CompileTimelimitRuleNetwork()
        {
            return RulesEngine.CompileTimelimitRuleNetwork();
        }

        //public Task Initialize(IContainer containerBuilder)
        //{

        //}

        public async Task GetPinsToProcess()
        {
            if (!this.Context.inputPins.Any())
            {
                using (var timelimitService = this.Container.Resolve<ITimelimitService>())
                {
                    this._logger.Information("No Pins provided to process. Searching Database for active participants:");
                    var pinsToProcess = await timelimitService.GetTimelimitPinsToProcessAsync(this.Context.Date, this.Context.JobQueuePartion, this._token).ConfigureAwait(false);
                    this.Context.inputPins.AddRange(pinsToProcess);
                    this._logger.Information($"Found [{this.Context.inputPins.Count}] pins to process:");
                }
            }
            else
            {
                this._logger.Information($"Processing [{this.Context.inputPins.Count}] provided pin(s):");
            }

            foreach (var pin in this.Context.inputPins)
            {
                this._logger.Information("Pin selected {pin} ", pin);
                this.pinsToProcess.Add(pin);
            }
        }

        public override void CreateProcessingQueue()
        {
            this._queue = new ActionBlock<EvaluateTimelimitsTaskContext>(async queuedItem =>
            {
                await this.ProcessItemAsync(queuedItem).ConfigureAwait(false);

            }, new ExecutionDataflowBlockOptions { CancellationToken = this._token, MaxDegreeOfParallelism = this.Context.MaxDegreeOfParallelism, SingleProducerConstrained = true, BoundedCapacity = 3 });
        }

        internal override async Task StartProducerQueue()
        {
            try
            {
                await this.GetPinsToProcess().ConfigureAwait(false);
                var tasks = new List<Task<EvaluateTimelimitsTaskContext>>();
                this._logger.Information("Starting Producer queue: Generating items to process...");

                foreach (var pinNumber in this.pinsToProcess)
                {
                    using (LogContext.PushProperty("Pin", pinNumber.ToString()))
                    {
                        if (pinNumber == 0)
                        {
                            this._logger.Debug("Error getting another pin out of the collection queue, waiting a few seconds and trying again if available.");
                            await this.SleepAsync(token: this._token).ConfigureAwait(false); // wait a little bit and try again in case the AsyncCollection completed
                            continue;
                        }

                        await this.GetQueueItemAsync(pinNumber).ContinueWith(async (x) =>
                        {
                            if (x.Result.Status == JobStatus.ReadyForJobProcessing)
                            {
                                this.UpdateJobStatusAsync(x.Result, JobStatus.QueuedForJobProcessing);

                                await this._queue.SendAsync(x.Result); //send item to the queue, will wait to items fall out of the queue(this.Context.MaxDegreeOfParallelism)
                                this._logger.Information($"Adding item to processing queue with ID: {x.Result.GetItemIdentifier()}");
                            }
                            else
                            {
                                this.UpdateJobStatusAsync(x.Result, x.Result.Status.GetValueOrDefault());
                                this._logger.Information($"Skipped processing item with ID (marked as {x.Result.Status.GetValueOrDefault()}): {x.Result.GetItemIdentifier()}");
                            }

                            return x.Result;
                        }, TaskContinuationOptions.OnlyOnRanToCompletion).Unwrap().ConfigureAwait(false);
                    }

                    //tasks.Add(queueItemTask);
                }

                //await getPinsTask.ConfigureAwait(false);
                //await Task.WhenAll(tasks).ConfigureAwait(false);
                this._logger.Information($"Stopping Producer queue: All processing Items generated. Generated [{tasks.Count}] items to process. ");
                this._queue.Complete();
            }
            catch (Exception e)
            {
                this._logger.Error(e, $"Stopping Producer queue: Error generating Items. ");
                this._queue?.Complete();

                throw;
            }
        }

        internal override void WriteJobOutput()
        {
            var file_path = Path.Combine(this.Context.OutputPath, $"eval_TL_{this.Context.JobQueuePartion}_{this.Context.Date:MMMM_yyyy}_{Guid.NewGuid().ToString("N").Substring(0, 10)}.xlsx");
            try
            {
                var outputPackage = new ExcelPackage();
                var rowNum = 2;
                var resultSheet = outputPackage.Workbook.Worksheets.Add("Results");
                foreach (var kvp in this.appOutput)
                {
                    Decimal dPin = kvp.Key;
                    ClockTypes actualResult;
                    // Find the matching pin in the results
                    //actualResult = context == null ? ClockTypes.None : context.TimelimitType.GetValueOrDefault();
                    actualResult = kvp.Value.RuleContext.TimelimitType.GetValueOrDefault();

                    //ClockTypes expectedClockType = this.expectedPinOutput.Where(kvp => kvp.Value != null && kvp.Value.Contains(dPin)).Select(x => x.Key).FirstOrDefault();

                    if (kvp.Value.Errors.Any())
                    {
                        resultSheet.Cells[rowNum, 4].Value = "F";
                    }
                    else
                    {
                        resultSheet.Cells[rowNum, 4].Value = "P";
                    }

                    resultSheet.Cells[rowNum, 3].Value = FlagEnums.FormatFlags(actualResult);

                    resultSheet.Cells[rowNum, 1].Value = dPin;

                    if (resultSheet.Cells[rowNum, 4].GetValue<String>() == "F")
                    {
                        resultSheet.Cells[rowNum, 4].Style.Font.Bold = true;
                        resultSheet.Cells[rowNum, 4].Style.Font.Color.SetColor(Color.FromArgb(100, 244, 67, 54));
                        resultSheet.Cells[rowNum, 1].Style.Font.Bold = true;
                        resultSheet.Cells[rowNum, 1].Style.Font.Color.SetColor(Color.FromArgb(100, 244, 67, 54));
                    }
                    else
                    {
                        resultSheet.Cells[rowNum, 4].Style.Font.Color.SetColor(Color.FromArgb(100, 3, 169, 244));
                        resultSheet.Cells[rowNum, 1].Style.Font.Color.SetColor(Color.FromArgb(100, 3, 169, 244));
                    }
                    //TODO: Dump ruleContext Data

                    rowNum++;
                }


                var file = new FileInfo(file_path);
                outputPackage.SaveAs(file);
            }
            catch (Exception e)
            {
                this._logger.Warning(e, $"Error writing output file \"${file_path}\" for {nameof(EvaluateTimelimitsBatchApplication)}.");
            }
        }

        protected override void LogJobAsProccessed(EvaluateTimelimitsTaskContext queuedItem, EvaluateTimelimitsTaskResult results, TimeSpan stopWatchElapsed)
        {
            base.LogJobAsProccessed(queuedItem, results, stopWatchElapsed);
            this.appOutput.AddOrUpdate(results.PinNumber, results, (k, v) => results);

            foreach (var opcTick in results.OtherParentData)
            {
                var ruleContext = new RuleContext() { TimelimitType = opcTick.TimelimitType, EvaluationMonth = results.RuleContext.EvaluationMonth, IsAlien = results.RuleContext.IsAlien, PaymentsAreFullySanctioned = results.RuleContext.PaymentsAreFullySanctioned, ShouldCreateOpcTicks = results.RuleContext.ShouldCreateOpcTicks };
                var result = new EvaluateTimelimitsTaskResult
                {
                    RuleContext = ruleContext,
                    PinNumber = opcTick.parent.PinNumber.GetValueOrDefault(),
                    EvaluatedData = new TimelineMonth(ruleContext.EvaluationMonth, ClockTypes.OPC, opcTick.TimelimitType.HasAnyFlags(ClockTypes.Federal), opcTick.TimelimitType.HasAnyFlags(ClockTypes.State), opcTick.TimelimitType.HasAnyFlags(ClockTypes.PlacementLimit)),
                    Status = JobStatus.JobProcessingSuccess
                };
                this.appOutput.AddOrUpdate(opcTick.parent.PinNumber.GetValueOrDefault(), result, (k, v) => result);
            }
        }


        //internal async Task<TimelimitEvaluationResultTaskContext> GetQueueItemAsync(EvaluateTimelimitsTaskContext context, EvaluateTimelimitsTaskResult result)
        //{
        //    var task = new TimelimitEvaluationResultTaskContext();
        //    task.Result = new TimelimitEvaluationResultTaskResult();
        //    task.Result.PinNumber = result.PinNumber;
        //    task.PinNumber = result.PinNumber;
        //    using (var timelimitService = this.Container.Resolve<ITimelimitService>())
        //    {
        //        task.Participant = await timelimitService.GetParticipantAsync(task.PinNumber, this._token);
        //    }

        //    task.Result.CreatedMonth = result.EvaluatedData;
        //    var extensions = context.Timeline.GetExtensions(ClockTypes.ExtensableTypes);
        //    var extensionApprovals = extensions.Where(x => x.ExtensionDecision == ExtensionDecision.Approve);
        //    // if there are more then one, assume they did something wierd and get the max last decision
        //    task.Result.activeExtension = extensionApprovals.Where(x=>x.HasStarted && !x.HasElapsed) .GetMax(x => x.DecisionDate);
        //    task.Result.Status = JobStatus.CreatingJobForProcessing;

        //    return task;
        //}



        internal override async Task<EvaluateTimelimitsTaskContext> GetQueueItemAsync(Decimal id)
        {
            var batchTask = new EvaluateTimelimitsTaskContext
            {
                Timeline = new Timeline(this.Context),
                MonthToProcess = this.Context.Date,
                PinNumber = id,
                ExternalJobId = id.ToString()
            };
            var monthRange = new DateTimeRange(this.Context.Date.StartOf(DateTimeUnit.Month), this.Context.Date.EndOf(DateTimeUnit.Month));

            try
            {
                await this.GetQueueItemAsyncThrottle.WaitAsync(this._token).ConfigureAwait(false);
                await this.CreateJobAsync(batchTask).ConfigureAwait(false);
                await this.UpdateJobStatusAsync(batchTask, JobStatus.CreatingJobForProcessing).ConfigureAwait(false);
                this._logger.Information($"Creating queue Item with id: {id}");
                //try to generate participant Timeline
                using (var timelimitService = this.Container.Resolve<ITimelimitService>())
                {

                    try
                    {
                        var allPlacements = new List<Placement>();
                        //var primaryPlacements = await timelimitService.GetPlacementsAsync(id, this._token).ConfigureAwait(false);
                        var primaryPlacements = await timelimitService.GetPlacementsAsync(id).ConfigureAwait(false);
                        allPlacements.AddRange(primaryPlacements);

                        //var assistancGroup = await timelimitService.GetOtherAGMembersAsync(id, monthRange.Start, monthRange.End, this._token);
                        var assistancGroup = await timelimitService.GetOtherAGMembersAsync(id, monthRange.Start, monthRange.End).ConfigureAwait(false);

                        foreach (var agMember in assistancGroup)
                        {
                            if (!agMember.IsChild() && agMember.ELIGIBILITY_PART_STATUS_CODE != "XA")
                            {
                                //var agMemberPlacements = await timelimitService.GetPlacements(agMember.PinNumber.GetValueOrDefault(), this._token).ConfigureAwait(false);
                                var agMemberPlacements = agMember.Timeline.Placements.SelectMany(x => x.Value);
                                allPlacements.AddRange(agMemberPlacements);
                            }
                        }


                        //var placementsTasks = new List<Task<IEnumerable<Placement>>>();

                        //placementsTasks.Add(this._timelimitService.GetPlacementsAsync(id, this._token));

                        //var assistancGroup = await this._timelimitService.GetOtherAGMembersAsync(id, monthRange.Start, monthRange.End, this._token);

                        //foreach (var agMember in assistancGroup)
                        //{
                        //    if (!agMember.IsChild() && agMember.ELIGIBILITY_PART_STATUS_CODE != "XA")
                        //    {
                        //        placementsTasks.Add(this._timelimitService.GetPlacementsAsync(agMember.PinNumber.GetValueOrDefault(),this._token));
                        //    }
                        //}

                        //await Task.WhenAll(placementsTasks).ConfigureAwait(false);

                        // check all the placements, and whoever had the last one is the "primary" who we are gonna evaluate
                        // var allPlacements = placementsTasks.SelectMany(x => x.Result);

                        var monthlyPlacements = allPlacements.Where(c => c.PlacementType.GetValueOrDefault().HasAnyFlags(ClockTypes.PlacementTypes) && c.DateRange.Overlaps(monthRange));
                        var lastPlacement = monthlyPlacements.GetMax(x => x.DateRange.End);


                        if (lastPlacement == null)
                        {
                            // No one was in paid placement this month
                            return batchTask;
                        }

                        if (lastPlacement.PinNumber != id)
                        {
                            this._logger.Information($"Switching queue Item id to: {lastPlacement.PinNumber}. id: {id} is not last placed in month: {this.Context.Date:MM/yyyy}");
                            id = lastPlacement.PinNumber;
                            batchTask.PinNumber = lastPlacement.PinNumber;
                            //assistancGroup = await timelimitService.GetOtherAGMembersAsync(lastPlacement.PinNumber, monthRange.Start, monthRange.End, this._token);
                            assistancGroup = timelimitService.GetOtherAGMembers(lastPlacement.PinNumber, monthRange.Start, monthRange.End);
                        }

                        // Filter out placements that start after the month we are trying to run the batch for
                        List<Placement> placements = allPlacements.Where(x => x.PinNumber == lastPlacement.PinNumber && x.DateRange.Start.IsSameOrBefore(this.Context.Date, DateTimeUnit.Month)).ToList();


                        //if (!placements.Any(c => c.DateRange.Overlaps(new DateTimeRange(this.Context.Date.StartOf(DateTimeUnit.Month), this.Context.Date.EndOf(DateTimeUnit.Month)))))
                        //{
                        //    // if this person ia an other parent try the same thing but with the other parent if there is one in case they switched
                        //    var parents = assistancGroupTask.Result.ToList();
                        //    var pins = parents.Select(x=>x.SourcePinNumber.GetValueOrDefault()).Distinct().ToList().Remove(0);
                        //    placementsTasks
                        //    foreach (var possiblePrimaryParent in assistancGroupTask.Result)
                        //    {

                        //        var parentPlacements = await this._timelimitService.GetPlacementsAsync(possiblePrimaryParent.SourcePinNumber.GetValueOrDefault(), this._token).ConfigureAwait(false);
                        //        var lastPlacement = parentPlacements.GetMax(x => x.DateRange.End);
                        //        if (lastPlacement != null && lastPlacement.DateRange.End.IsAfter())
                        //    }
                        //    return batchTask;
                        //}

                        //var participant = await timelimitService.GetParticipantAsync(id, this._token).ConfigureAwait(false);
                        //var timelineMonths = await timelimitService.GetTimelimitMonthsAsync(id, this._token).ConfigureAwait(false);
                        //var extensionSequences = await timelimitService.GetTimelineExtensionSequencesAsync(id, this._token).ConfigureAwait(false);
                        //var alientStatus = await timelimitService.GetParticipantAlienStatusAsync(id, this._token).ConfigureAwait(false);

                        var participant = await timelimitService.GetParticipantAsync(id, this._token).ConfigureAwait(false);
                        var timelineMonths = await timelimitService.GetTimelineMonthsAsync(id, this._token).ConfigureAwait(false);
                        var extensionSequences = await timelimitService.GetTimelineExtensionSequencesAsync(id, this._token).ConfigureAwait(false);
                        var alientStatus = await timelimitService.GetParticipantAlienStatusAsync(id, this._token).ConfigureAwait(false);

                        //once Participant is loaded, then fire off paymentTask

                        //if (participant == null || participant.CaseNumber.GetValueOrDefault() == 0)   //TODO: remove this line after testing, during clean-up
                        if (participant == null ||  participant.PinNumber.HasValue && participant.PinNumber.Value == 0)
                        {
                            throw new GetQueueItemException($"Error Get Queue item for participant {participant?.PinNumber}. No pin or CaseNumber found");
                        }

                        //var payments = await timelimitService.GetPaymentInfoAsync(participant.CaseNumber.Value, this._token).ConfigureAwait(false);
                        var payments = await timelimitService.GetPaymentInfoAsync(participant.PinNumber.Value, _token).ConfigureAwait(false);

                        // add Placements
                        batchTask.Timeline.AddPlacements(placements);
                        // add Extensions Sequences
                        batchTask.Timeline.AddExtensionSequences(extensionSequences);
                        // add Timeline Months (Timelmits rows)
                        batchTask.Timeline.AddTimelineMonths(timelineMonths);
                        //// add Auxillary Payments that have not been imported yet
                        //batchTask.AuxillaryPayments = auxillaryPaymentsTask.Result;
                        // add the Participant
                        batchTask.Participant = participant;
                        batchTask.PinNumber = participant.PinNumber.GetValueOrDefault();
                        // add Any Alien statuses
                        batchTask.AlienStatus = alientStatus;
                        // add payments
                        batchTask.Payments = payments;
                        //Add AG members
                        batchTask.AssitanceGroupMembers = assistancGroup;

                        batchTask.Status = JobStatus.ReadyForJobProcessing;

                        await this.UpdateJobStatusAsync(batchTask, batchTask.Status.GetValueOrDefault());

                    }
                    catch (Exception e)
                    {
                        this._logger.Error(e, "Error creating Batch Task:");
                        batchTask.Status = JobStatus.JobProcessingFailure;
                        batchTask.Result.Errors.Add(e);
                        this.FailedProccessingJobsSinceStart++;
                        //throw;
                    }
                }
            }
            finally

            {
                this.GetQueueItemAsyncThrottle.Release();
            }

            return batchTask;
        }
    }
}
