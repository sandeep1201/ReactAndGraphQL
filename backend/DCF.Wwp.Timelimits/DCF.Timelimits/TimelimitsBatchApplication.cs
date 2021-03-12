using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Autofac;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Services;
using DCF.Common.Dates;
using DCF.Common.Extensions;
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
using Nito.AsyncEx;
using NRules;
using NRules.Fluent;
using Serilog;

namespace DCF.Timelimits
{
    public class TimelimitsBatchApplication : BatchApplication<Decimal, EvaluateTimelimitsTaskContext, EvaluateTimelimitsTaskResult>
    {
        public IJobQueue JobQueue { get; set; }
        public IJobQueueService JobQueueService { get; private set; }

        internal AsyncCollection<Decimal> PinsToProcess = new AsyncCollection<decimal>();

        internal ITaskMediator mediator;

        public TimelimitsBatchApplication(ApplicationContext context) : base(context)
        {

            this.OnInitialized = this.DoInitialize;
            this.OnContainerInitialized = this.ContainerInitialized;
        }

        private void ContainerInitialized(ContainerBuilder containerBuilder)
        {
            NRules.ISessionFactory ruleFactory = this.CompileTimelimitRuleNetwork();
            containerBuilder.RegisterInstance(ruleFactory).As<ISessionFactory>().SingleInstance();
        }

        internal ISessionFactory CompileTimelimitRuleNetwork()
        {
            return RulesEngine.CompileTimelimitRuleNetwork();
        }

        public async Task DoInitialize(ContainerBuilder containerBuilder)
        {
            this.mediator = this.Container.Resolve<ITaskMediator>();

            this._logger.Information("Initilization Application - Getting JobQueue.");

            this.JobQueueService = this.Container.Resolve<IJobQueueService>();
            this.JobQueue = await this.JobQueueService.GetJobQueueAsync(this.Context.JobQueueName, this.Context.JobQueueType, this.Context.JobQueuePartion).ConfigureAwait(false);

            this._logger.Information("Initilization Application - Getting JobQueue Complete!");

            await this.RefreshPartipicantsFromLegacySystem().ConfigureAwait(false);
            await this.GetPinsToProcess();

        }

        private Task RefreshPartipicantsFromLegacySystem()
        {
            using (var timelimitService = this.Container.Resolve<ITimelimitService>())
            {
                return timelimitService.RefreshParticipantsAsync(this.Context.Date, this._token);
            }
        }

        public virtual async Task GetPinsToProcess()
        {
            if (!this.Context.inputPins.Any())
            {
                using (var timelimitService = this.Container.Resolve<ITimelimitService>())
                {
                    this._logger.Information("No Pins provided to process. Searching Database for active participants:");
                    var pinsToProcess = await timelimitService.GetPinsToProcessAsync(this.Context.Date, this._token).ConfigureAwait(false);
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
                this.PinsToProcess.Add(pin, this._token);
            }
            this.PinsToProcess.CompleteAdding();

        }

        protected override async Task<IEnumerable<T>> ProcessQueuedTaskAsync<T>(IBatchTask<T> queuedItem)
        {
            var results = await this.mediator.SendToMany(queuedItem).ConfigureAwait(false);
            return results;
        }

        public override void CreateProcessingQueue()
        {
            this.DoCreateProcessingQueue();
        }

        protected override Task UpdateJobStatus(EvaluateTimelimitsTaskContext Job, JobStatus status)
        {
            return this.JobQueueService.UpdateJobStatusAsync(Job, status, token: this._token);
        }

        protected override Task CreateJobAsync(IBatchTask task)
        {
            return this.JobQueueService.CreateJobAsync(task, this.JobQueue, this.Context.ApplicationInstanceName, this._token);
        }



        private void DoCreateProcessingQueue()
        {
            this._queue = new ActionBlock<EvaluateTimelimitsTaskContext>(async queuedItem =>
            {
                await this.ProcessItem(queuedItem);

            }, new ExecutionDataflowBlockOptions { CancellationToken = this._token, BoundedCapacity = 5, MaxDegreeOfParallelism = this.Context.MaxDegreeOfParallelism, SingleProducerConstrained = true });
        }

        protected async Task<IEnumerable<EvaluateTimelimitsTaskResult>> ProcessItem(EvaluateTimelimitsTaskContext queuedItem)
        {
            Boolean success = false;
            var jobResults = new List<EvaluateTimelimitsTaskResult>();
            using (this._logger.BeginTimedOperation("Job Proccesing", queuedItem.GetItemIdentifier()))
            {
                var stopWatch = new Stopwatch();

                try
                {
                    stopWatch.Start();
                    await this.UpdateJobStatus(queuedItem, JobStatus.JobIsProcessing);
                    jobResults = (await this.ProcessQueuedTaskAsync(queuedItem).ConfigureAwait(false)).ToList();
                    stopWatch.Stop();
                    success = true;
                    this.LogJobAsProccessed(queuedItem, jobResults, stopWatch.Elapsed);
                    var jobProcessingStatus = jobResults.Any(x => x.Status != JobStatus.JobProcessingSuccess) ? JobStatus.JobProcessingSuccess : JobStatus.JobProcessingFailure;
                    await this.UpdateJobStatus(queuedItem, jobProcessingStatus);
                }
                catch (Exception e)
                {
                    this._logger.Error(e, $"Error processing job: {queuedItem.JobId}");
                }
                finally
                {
                    stopWatch.Stop();
                }
                return jobResults;
            }
        }

        internal override async Task StartProducerQueue()
        {
            var tasks = new List<Task<EvaluateTimelimitsTaskContext>>();
            this._logger.Information("Starting Producer queue: Generating items to process...");

            while (await this.PinsToProcess.OutputAvailableAsync().ConfigureAwait(false))
            {
                var pinNumberResult = await this.PinsToProcess.TryTakeAsync(this._token).ConfigureAwait(false);
                var pinNumber = pinNumberResult.Success ? pinNumberResult.Item : 0;

                if (pinNumber == 0)
                {
                    this._logger.Debug("Error getting another pin out of the collection queue, waiting a few seconds and trying again if available.");
                    await this.SleepAsync(token: this._token).ConfigureAwait(false); // wait a little bit and try again in case the AsyncCollection completed
                    continue;
                }

                var queueItemTask = this.GetQueueItemAsync(pinNumber).ContinueWith(async (x) =>
                {
                    if (x.Result.Status == JobStatus.ReadyForJobProcessing)
                    {
                        this.UpdateJobStatus(x.Result, JobStatus.QueuedForJobProcessing);

                        await this._queue.SendAsync(x.Result); //send item to the queue, will wait to items fall out of the queue(this.Context.MaxDegreeOfParallelism)
                        this._logger.Information($"Adding item to processing queue with ID: {x.Result.GetItemIdentifier()}");
                    }
                    else
                    {
                        this.UpdateJobStatus(x.Result, JobStatus.JobProcessingSuccess);
                        this._logger.Information($"Skipped processing item with ID (marked as complete). Nothing to do: {x.Result.GetItemIdentifier()}");
                    }
                    return x.Result;
                }, TaskContinuationOptions.OnlyOnRanToCompletion).Unwrap();

                tasks.Add(queueItemTask);
            }

            await Task.WhenAll(tasks).ConfigureAwait(false);
            this._logger.Information($"Stopping Producer queue: All processing Items generated. Generated [{tasks.Count}] items to process. ");
            this._queue.Complete();
        }

        internal override async Task<EvaluateTimelimitsTaskContext> GetQueueItemAsync(Decimal id)
        {
            var batchTask = new EvaluateTimelimitsTaskContext
            {
                Timeline = new Timeline(this.Context),
                MonthToProcess = this.Context.Date,
                PinNumber = id,
            };
            var monthRange = new DateTimeRange(this.Context.Date.StartOf(DateTimeUnit.Month), this.Context.Date.EndOf(DateTimeUnit.Month));

            await this.CreateJobAsync(batchTask).ConfigureAwait(false);
            await this.UpdateJobStatus(batchTask, JobStatus.CreatingJobForProcessing).ConfigureAwait(false);
            this._logger.Information($"Creating queue Item with id: {id}");
            //try to generate participant Timeline
            using (var timelimitService = this.Container.Resolve<ITimelimitService>())
            {

                try
                {
                    var allPlacements = new List<Placement>();
                    //var primaryPlacements = await timelimitService.GetPlacementsAsync(id, this._token).ConfigureAwait(false);
                    var primaryPlacements = timelimitService.GetPlacements(id);
                    allPlacements.AddRange(primaryPlacements);

                    //var assistancGroup = await timelimitService.GetOtherAGMembersAsync(id, monthRange.Start, monthRange.End, this._token);
                    var assistancGroup = timelimitService.GetOtherAGMembers(id, monthRange.Start, monthRange.End);

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

                    var participant = timelimitService.GetParticipant(id);
                    var timelineMonths = timelimitService.GetTimelineMonths(id);
                    var extensionSequences = timelimitService.GetTimelineExtensionSequences(id);
                    var alientStatus = timelimitService.GetParticipantAlienStatus(id);

                    //once Participant is loaded, then fire off paymentTask

                    if (participant == null || participant.CaseNumber.GetValueOrDefault() == 0)
                    {
                        throw new GetQueueItemException($"Error Get Queue item for participant {participant?.PinNumber}. No pin or CaseNumber found");
                    }

                    var payments = timelimitService.GetPaymentInfo(participant.CaseNumber.Value);

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
                    // add Any Alien statuses
                    batchTask.AlienStatus = alientStatus;
                    // add payments
                    batchTask.Payments = payments;
                    //Add AG members
                    batchTask.AssitanceGroupMembers = assistancGroup;

                    batchTask.Status = JobStatus.ReadyForJobProcessing;

                    await this.UpdateJobStatus(batchTask, batchTask.Status.GetValueOrDefault());

                }
                catch (Exception e)
                {
                    this._logger.Error(e, "Error creating Batch Task:");
                    batchTask.Result.Errors.Add(e);
                    //throw;
                }

                return batchTask;
            }
        }
    }
}