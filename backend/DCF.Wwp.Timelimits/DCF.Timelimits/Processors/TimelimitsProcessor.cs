using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DCF.Common.Dates;
using DCF.Common.Extensions;
using DCF.Common.Tasks;
using DCF.Timelimits.Core.Processors;
using DCF.Timelimits.Rules.Definitions;
using DCF.Timelimits.Rules.Domain;
using DCF.Timelimits.Rules.Scripting;
using DCF.Timelimits.Tasks;
using DCF.Timelimts.Service;
using EnumsNET;
using NRules;

namespace DCF.Timelimits.Processors
{
    [BatchTaskProcess(Priority = 1)]
    public class TimelimitsProcessor : BatchTaskProcessBase<EvaluateTimelimitsTaskContext, EvaluateTimelimitsTaskResult>
    {
        private readonly ITimelimitService _timelimitService;
        //private readonly IDb2TimelimitService _db2TimelimitService;
        private ISessionFactory _ruleSessionFactory;
        private readonly ApplicationContext _applicationContext;

        public TimelimitsProcessor(ITimelimitService timelimitService, ISessionFactory ruleSessionFactory, ApplicationContext applicationContext)
        {
            this._timelimitService = timelimitService;
            //this._db2TimelimitService = db2TimelimitService;
            this._ruleSessionFactory = ruleSessionFactory;
            this._applicationContext = applicationContext;
        }

        public override async Task<EvaluateTimelimitsTaskResult> Handle(EvaluateTimelimitsTaskContext context, CancellationToken token)
        {
            var ruleSession = this._ruleSessionFactory.CreateSession();
            var rulesContext = new RuleContext();
            rulesContext.EvaluationMonth = context.MonthToProcess;

            ruleSession.Insert(context.Timeline ?? new Timeline());
            ruleSession.Insert(rulesContext);
            ruleSession.InsertAll(context.Payments);
            ruleSession.InsertAll(context.AlienStatus);
            ruleSession.Insert(context.Participant);
            ruleSession.InsertAll(context.AssitanceGroupMembers);

            // Error handling events
            RulesEngine.LogSessionEvents(ruleSession, this._logger);

            ruleSession.Fire();
            rulesContext = ruleSession.Query<RuleContext>().First();
            var otherParentTicks = ruleSession.Query<OpcTick>().ToList();

            var result = new EvaluateTimelimitsTaskResult();
            if (rulesContext.TimelimitType.HasValue)
            {
                result.EvaluatedData = new TimelineMonth(rulesContext.EvaluationMonth, rulesContext.TimelimitType.GetValueOrDefault(), rulesContext.TimelimitType.GetValueOrDefault().HasFlag(ClockTypes.Federal), rulesContext.TimelimitType.GetValueOrDefault().HasFlag(ClockTypes.State), rulesContext.TimelimitType.GetValueOrDefault().HasAnyFlags(ClockTypes.PlacementLimit) && !rulesContext.TimelimitType.GetValueOrDefault().HasFlag(ClockTypes.NoPlacementLimit));
            }
            else
            {
                result.EvaluatedData = new TimelineMonth(rulesContext.EvaluationMonth, ClockTypes.None, false, false, false);
            }


            result.MonthProcessed = rulesContext.EvaluationMonth;
            result.RuleContext = rulesContext;
            result.OtherParentData = otherParentTicks;
            result.PinNumber = context.PinNumber;
            await this.SaveTimelimitRecordsAsync(context, result, token).ConfigureAwait(false);

            result.Status = JobStatus.JobProcessingSuccess;
            return result;
        }

        private async Task SaveTimelimitRecordsAsync(EvaluateTimelimitsTaskContext context, EvaluateTimelimitsTaskResult result, CancellationToken token = default(CancellationToken))
        {

            //this._db2TimelimitService.IsSimulated = this._applicationContext.IsSimulation;

            var month = await this._timelimitService.TimeLimitByDateAsync(result.PinNumber, result.MonthProcessed, token).ConfigureAwait(false) ?? this._timelimitService.NewTimeLimit();

            month.FederalTimeLimit = result.EvaluatedData.ClockTypes.HasAnyFlags(ClockTypes.Federal);
            month.StateTimelimit = result.EvaluatedData.ClockTypes.HasAnyFlags(ClockTypes.State);
            month.TwentyFourMonthLimit = result.EvaluatedData.ClockTypes.HasAnyFlags(ClockTypes.PlacementLimit);
            month.TimeLimitTypeId = (Int32)result.EvaluatedData.ClockTypes.CommonFlags(ClockTypes.CreateableTypes);
            month.EffectiveMonth = result.MonthProcessed.StartOf(DateTimeUnit.Month);
            month.ModifiedBy = "WWP Batch";
            month.ParticipantID = context.Participant.Id;

            if (!this._applicationContext.IsSimulation)
            {
                await this._timelimitService.SaveEntityAsync(month, token).ConfigureAwait(false);
            }

            foreach (var opcTick in result.OtherParentData)
            {
                var opcParticipant = await this._timelimitService.GetParticipantAsync(opcTick.parent.PinNumber.GetValueOrDefault(), token).ConfigureAwait(false);
                var opcMonth = await this._timelimitService.TimeLimitByDateAsync(opcParticipant.PinNumber.GetValueOrDefault(), result.MonthProcessed, token).ConfigureAwait(false) ?? this._timelimitService.NewTimeLimit();

                opcMonth.ParticipantID = opcParticipant.Id;
                opcMonth.EffectiveMonth = result.MonthProcessed.StartOf(DateTimeUnit.Month);
                opcMonth.TimeLimitTypeId = (Int32)opcTick.TimelimitType.CommonFlags(ClockTypes.CreateableTypes);
                opcMonth.TwentyFourMonthLimit = opcTick.TimelimitType.HasAnyFlags(ClockTypes.PlacementLimit);
                opcMonth.StateTimelimit = opcTick.TimelimitType.HasAnyFlags(ClockTypes.State);
                opcMonth.FederalTimeLimit = opcTick.TimelimitType.HasAnyFlags(ClockTypes.Federal);
                opcMonth.ModifiedBy = "WWP Batch";

                if (!this._applicationContext.IsSimulation)
                {
                    await this._timelimitService.SaveEntityAsync(opcMonth, token).ConfigureAwait(false);
                }

                await this.CreateSnapshotAsync(opcTick.parent.Timeline, opcParticipant.Id, token).ConfigureAwait(false);

            }
        }

        private Task CreateSnapshotAsync(ITimeline timeline, Int32 participantId, CancellationToken token = default(CancellationToken))
        {
            var tls = this._timelimitService.CreateTimelimitSummary(timeline, participantId);
            if (!this._applicationContext.IsSimulation)
            {
                return _timelimitService.SaveEntityAsync(tls, token);
            }

            return Task.CompletedTask;
        }
    }
}