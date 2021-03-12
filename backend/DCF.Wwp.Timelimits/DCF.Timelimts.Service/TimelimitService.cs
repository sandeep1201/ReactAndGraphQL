using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Dcf.Wwp.Api.Library.Services;
using Dcf.Wwp.Data.Sql.Model;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Repository;
using DCF.Common.Dates;
using DCF.Common.Extensions;
using DCF.Common.Logging;
using DCF.Common.Tasks;
using DCF.Core;
using DCF.Timelimits.Core.Tasks;
using DCF.Timelimits.Rules.Domain;
using EnumsNET;
using SpAlienStatusResult = Dcf.Wwp.Data.Sql.Model.SpAlienStatusResult;
using Dcf.Wwp.Model.Interface.Constants;
using Dcf.Wwp.Api.Library;

namespace DCF.Timelimts.Service
{
    public class TimelimitService : ITimelimitService
    {
        private readonly WwpEntities _dbContext;
        //private Func<WwpEntities> _dbContextFactory;

        private ILog _logger = LogProvider.GetLogger(typeof(TimelimitService));

        public TimelimitService(WwpEntities _dbContext)
        {
            this._dbContext = _dbContext;
            //this._dbContextFactory = () => _dbContext;
        }

        //public TimelimitService(SingleInstanceFactory singleInstanceFactory, Func<WwpEntities> func)
        //{
        //    this._dbContextFactory = ()=> (WwpEntities)singleInstanceFactory(typeof(WwpEntities));
        //}

        //public TimelimitService(Func<WwpEntities> func)
        //{
        //    this._dbContextFactory =func;
        //}

        // TODO: Get dynamically from store
        public Task<Int32?> GetClockMaxAsync(Decimal pinNumber, ClockTypes flags, CancellationToken token = default(CancellationToken))
        {
            switch (flags)
            {
                case ClockTypes.State:
                    return Task.FromResult((Int32?) 60);
                case ClockTypes.CSJ:
                case ClockTypes.W2T:
                case ClockTypes.TEMP:
                    return Task.FromResult((Int32?) 24);
                    ;
                case ClockTypes.None:
                case ClockTypes.Federal:
                case ClockTypes.TNP:
                case ClockTypes.TMP:
                case ClockTypes.CMC:
                case ClockTypes.OPC:
                case ClockTypes.OTF:
                case ClockTypes.TRIBAL:
                case ClockTypes.TJB:
                case ClockTypes.JOBS:
                case ClockTypes.NoPlacementLimit:
                case ClockTypes.Other:
                case ClockTypes.PlacementLimit:
                case ClockTypes.ExtensableTypes:
                case ClockTypes.CreateableTypes:
                default:
                    return Task.FromResult((Int32?) null);
            }
        }

        public Task<List<Decimal>> GetTimelimitPinsToProcessAsync(DateTime currentDate, Int32 partition, CancellationToken token = default(CancellationToken))
        {
            var partitionRange = PartitionRangeRanges[partition];
            return this._dbContext.Participants.Where(x => x.TimeLimitStatus == true && x.PinNumber.HasValue && x.PinNumber.Value >= partitionRange.Item1 && x.PinNumber.Value <= partitionRange.Item2).Select(x => x.PinNumber.Value).ToListAsync(token);
        }

        public Task<List<Decimal>> GetExtensionPinsToProcessAsync(DateTime currentDate, Int32 partition, CancellationToken token = default(CancellationToken))
        {
            currentDate = currentDate.StartOf(DateTimeUnit.Month);
            var partitionRange = PartitionRangeRanges[partition];
            return this._dbContext.TimeLimitExtensions.Where(x => x.EndMonth == currentDate && x.Participant != null && x.Participant.PinNumber.HasValue && x.Participant.PinNumber.Value >= partitionRange.Item1 && x.Participant.PinNumber.Value <= partitionRange.Item2).Select(x => x.Participant.PinNumber.Value).ToListAsync(token);
        }

        public async Task<List<Decimal>> GetBatchEvaluatedPins(DateTime currentDate, Int32 partition, CancellationToken token = default(CancellationToken))
        {
            var partitionRange = PartitionRangeRanges[partition];

            var effectiveMonth = currentDate.StartOf(DateTimeUnit.Month);
            var allPins        = new List<Decimal>();
            var timelimitPins  = await this.GetTimelimitPinsToProcessAsync(currentDate, partition, token).ConfigureAwait(false);
            var opcPins        = await this._dbContext.TimeLimits.Where(x => x.TimeLimitTypeId == (Int32) ClockTypes.OPC && x.EffectiveMonth == effectiveMonth && x.Participant.PinNumber.HasValue && x.Participant.PinNumber.Value >= partitionRange.Item1 && x.Participant.PinNumber.Value <= partitionRange.Item2).Select(x => x.Participant.PinNumber.Value).ToListAsync(token).ConfigureAwait(false);

            allPins.AddRange(timelimitPins);
            allPins.AddRange(opcPins);
            return allPins.Distinct().ToList();
        }

        public async Task<IEnumerable<Placement>> GetPlacementsAsync(Decimal pinNumber, CancellationToken token = default(CancellationToken))
        {
            var placementData = await this._dbContext.SpTimelimitPlacementSummaryAsync(pinNumber.ToString(), token).ConfigureAwait(false);
            var results       = placementData.Select(x => new Placement(x.PLACEMENT_TYPE, x.PLACEMENT_BEGIN_DATE, x.PLACEMENT_END_MONTH) { PinNumber = x.PARTICIPANT });
            return results;
        }

        public IEnumerable<Placement> GetPlacements(Decimal pinNumber)
        {
            var placementData = this._dbContext.SpTimelimitPlacementSummary(pinNumber.ToString());
            var results       = placementData.Select(x => new Placement(x.PLACEMENT_TYPE, x.PLACEMENT_BEGIN_DATE, x.PLACEMENT_END_MONTH) { PinNumber = x.PARTICIPANT });
            return results;
        }

        public async Task<List<TimelineMonth>> GetTimelineMonthsAsync(Decimal pinNumber, CancellationToken token = new CancellationToken())
        {
            var timelineMonths = await this._dbContext.TimeLimits.Where(x => x.Participant.PinNumber == pinNumber && !x.IsDeleted && x.TimeLimitTypeId.HasValue && x.TimeLimitTypeId.Value != (Int32) ClockTypes.None).ToListAsync(token).ConfigureAwait(false);
            return timelineMonths.Select(TimelimitService.MapTimelimitToTimelineMonth).ToList();
        }

        public List<TimelineMonth> GetTimelineMonths(Decimal pinNumber)
        {
            var timelineMonths = this._dbContext.TimeLimits.Where(x => x.Participant.PinNumber == pinNumber && !x.IsDeleted && x.TimeLimitTypeId.HasValue && x.TimeLimitTypeId.Value != (Int32) ClockTypes.None).ToList();
            return timelineMonths.Select(TimelimitService.MapTimelimitToTimelineMonth).ToList();
        }

        public static Extension MapTimelimitExtensionToExtension(ITimeLimitExtension extensioModel)
        {
            return
                new Extension((ClockTypes) extensioModel.TimeLimitTypeId, extensioModel.DecisionDate.GetValueOrDefault(), extensioModel.BeginMonth, extensioModel.EndMonth);
        }

        public static TimelineMonth MapTimelimitToTimelineMonth(ITimeLimit x)
        {
            return new TimelineMonth(x.EffectiveMonth.Value, (ClockTypes) x.TimeLimitTypeId.GetValueOrDefault(), x.FederalTimeLimit.GetValueOrDefault(), x.StateTimelimit.GetValueOrDefault(), x.TwentyFourMonthLimit.GetValueOrDefault());
        }

        public static ITimeLimit MapTimelineMonthToTimelimit(TimelineMonth x)
        {
            return new TimeLimit()
                   {
                       EffectiveMonth       = x.Date,
                       TimeLimitTypeId      = (Int32) x.ClockTypes.CommonFlags(ClockTypes.PlacementTypes),
                       FederalTimeLimit     = x.ClockTypes.HasAnyFlags(ClockTypes.Federal),
                       StateTimelimit       = x.ClockTypes.HasAnyFlags(ClockTypes.State),
                       TwentyFourMonthLimit = x.ClockTypes.HasAnyFlags(ClockTypes.PlacementLimit)
                   };
        }


        public async Task<List<ExtensionSequence>> GetTimelineExtensionSequencesAsync(Decimal pinNumber, CancellationToken token = new CancellationToken())
        {
            var timeLimitExtensions = (await this._dbContext.TimeLimitExtensions
                                                 .Where(x => x.Participant.PinNumber == pinNumber)
                                                 .Where(x => !x.IsDeleted)
                                                 .GroupBy(x => new { x.ExtensionSequence, x.TimeLimitTypeId })
                                                 .ToListAsync(token).ConfigureAwait(false))
                .Select(x => new Tuple<Int32, IEnumerable<TimeLimitExtension>>(x.Key.ExtensionSequence.GetValueOrDefault(), x.AsEnumerable()));

            return this._extractExtensionsFromSequenceQuery(timeLimitExtensions).ToList();
            ;
        }


        public List<ExtensionSequence> GetTimelineExtensionSequences(Decimal pinNumber)
        {
            var timeLimitExtensions = this._dbContext.TimeLimitExtensions
                                          .Where(x => x.Participant.PinNumber == pinNumber)
                                          .Where(x => !x.IsDeleted)
                                          .GroupBy(x => new { x.ExtensionSequence, x.TimeLimitTypeId })
                                          .ToList()
                                          .Select(x => new Tuple<Int32, IEnumerable<TimeLimitExtension>>(x.Key.ExtensionSequence.GetValueOrDefault(), x.AsEnumerable()));

            return this._extractExtensionsFromSequenceQuery(timeLimitExtensions).ToList();
        }

        private IQueryable<Tuple<Int32, IEnumerable<TimeLimitExtension>>> DoGetTimelineExtensions(Expression<Func<TimeLimitExtension, Boolean>> participantFilter, WwpEntities context)
        {
            Debug.Assert(participantFilter != null, nameof(participantFilter) + " != null");
            return context.TimeLimitExtensions
                          .Where(participantFilter)
                          .Where(x => !x.IsDeleted)
                          .GroupBy(x => new { x.ExtensionSequence, x.TimeLimitTypeId })
                          .Select(x => new Tuple<Int32, IEnumerable<TimeLimitExtension>>(x.Key.ExtensionSequence.GetValueOrDefault(), x.AsEnumerable()));
        }


        private IEnumerable<ExtensionSequence> _extractExtensionsFromSequenceQuery(IEnumerable<Tuple<Int32, IEnumerable<TimeLimitExtension>>> sequenceGroups)
        {
            return sequenceGroups?.ToList().Select(x => new ExtensionSequence(x.Item1, x.Item2.Select(TimelimitService.MapTimelimitExtensionToExtension))) ?? new ExtensionSequence[0];
        }


        public async Task RefreshAuxPaymentsAsync(decimal pinNumber, CancellationToken token = default(CancellationToken))
        {
            await this._dbContext.SpAuxiliaryPaymentAsync(pinNumber.ToString(), token).ConfigureAwait(false);
        }

        public void RefreshAuxPayments(decimal pinNumber)
        {
            this._dbContext.SpAuxiliaryPaymentAsync(pinNumber.ToString());
        }

        public async Task RefreshParticipantsAsync(DateTime month, CancellationToken token = default(CancellationToken))
        {
            await this._dbContext.SpBatchParticipantAsync(month, token).ConfigureAwait(false);
        }

        public void RefreshParticipants(DateTime month)
        {
            this._dbContext.SpBatchParticipant(month);
        }

        public Task<Participant> GetParticipantAsync(Decimal pinNumber, CancellationToken token = default(CancellationToken))
        {
            return this._dbContext.SpRefreshParticipantAsync(pinNumber.ToString(), token);
        }

        public Task<Participant> GetParticipantByIdAsync(Int32 id, CancellationToken token = default(CancellationToken))
        {
            return this._dbContext.Participants.FirstOrDefaultAsync(x => x.Id == id, token);
        }

        public Participant GetParticipant(Decimal pinNumber)
        {
            return this._dbContext.SpRefreshParticipant(pinNumber.ToString());
        }

        public async Task<List<AlienStatus>> GetParticipantAlienStatusAsync(Decimal pinNumber, CancellationToken token = default(CancellationToken))
        {
            List<SpAlienStatusResult> results = await this._dbContext.SpAlienStatusAsync(pinNumber.ToString(), token).ConfigureAwait(false);
            return this.Map_SpAlienStatusResults_To_SpAlienStatusResult(results);
        }

        public List<AlienStatus> GetParticipantAlienStatus(Decimal pinNumber)
        {
            List<SpAlienStatusResult> results = this._dbContext.SpAlienStatus(pinNumber.ToString());
            return this.Map_SpAlienStatusResults_To_SpAlienStatusResult(results);
        }

        public List<AlienStatus> Map_SpAlienStatusResults_To_SpAlienStatusResult(IEnumerable<SpAlienStatusResult> data)
        {
            return data.Select(x =>
                               {
                                   var      start = DateTime.ParseExact(x.EffectiveBeginMonth.ToString(), "yyyyMM", CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal);
                                   DateTime end;
                                   if (x.EffectiveEndMonth?.ToString().Length != 6 || !DateTime.TryParseExact(x.EffectiveEndMonth?.ToString(), "yyyyMM", CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out end))
                                   {
                                       //Treat any thing else as an OPEN end date for alien status
                                       end = DateTime.MaxValue;
                                   }

                                   var alientStatus = new AlienStatus(start, end == default(DateTime) ? null : new DateTime?(end)) { AlienStatusCode = x.ALIEN_STS_CD, AlienStatusCodeDescriptionText = x.AlienStatusCodeDescriptionText };
                                   return alientStatus;
                               }).ToList();
        }

        public async Task<List<AuxiliaryPayment>> GetAuxillaryPaymentsAsync(Decimal pinNumber, CancellationToken token = default(CancellationToken))
        {
            return await this._dbContext.SpAuxiliaryPaymentAsync(pinNumber.ToString(), token).ConfigureAwait(false);
        }

        public List<AuxiliaryPayment> GetAuxillaryPayments(Decimal pinNumber)
        {
            return this._dbContext.SpAuxiliaryPayment(pinNumber.ToString());
        }

        public List<Payment> GetPaymentInfo(decimal pinNumber)
        {
            var results = this._dbContext.SpW2PaymentInfo(pinNumber.ToString());
            return results.Select(x => new Payment() { AdjustedNetAmount = x.AdjustedNetAmount, CaseNumber = x.CaseNumber, EffectivePaymentMonth = x.EffectivePaymentMonthDateTime, OriginalPaymentAmount = x.OriginalPaymentAmount, OrignalCheckAmount = x.OrignalCheckAmount, VendorPayment = x.VendorPayment, PayPeriodBeginDate = x.PayPeriodBeginDate, PayPeriodEndDate = x.PayPeriodEndDate }).ToList();
        }

        public async Task<List<Payment>> GetPaymentInfoAsync(decimal pinNumber, CancellationToken token = default(CancellationToken))
        {
            var results = await this._dbContext.SpW2PaymentInfoAsync(pinNumber.ToString(), token).ConfigureAwait(false);
            return results.Select(x => new Payment() { AdjustedNetAmount = x.AdjustedNetAmount, CaseNumber = x.CaseNumber, EffectivePaymentMonth = x.EffectivePaymentMonthDateTime, OriginalPaymentAmount = x.OriginalPaymentAmount, OrignalCheckAmount = x.OrignalCheckAmount, VendorPayment = x.VendorPayment, PayPeriodBeginDate = x.PayPeriodBeginDate, PayPeriodEndDate = x.PayPeriodEndDate }).ToList();
        }

        //public  Task CreateJobAsync(IBatchTask task, Int32 queueId)
        //{
        //    using (var _dbContext = this._dbContextFactory())
        //    {
        //        var jobQueueItem = await _dbContext.JobQueues.FirstOrDefault(x=>x.Id == queueId);
        //    }
        //    return CreateJobAsync()
        //}


        public List<AssistanceGroupMember> GetOtherAGMembers(Decimal pinNumber, DateTime beginDate, DateTime endDate)
        {
            var data    = this._dbContext.SpOtherParticipant(pinNumber.ToString(), beginDate, endDate);
            var results = data.Select(x => new AssistanceGroupMember { AGE = x.AGE, BIRTH_DATE = x.BIRTH_DATE, DEATH_DATE = x.DEATH_DATE, ELIGIBILITY_PART_STATUS_CODE = x.ELIGIBILITY_PART_STATUS_CODE, FIRST_NAME = x.FIRST_NAME, GENDER = x.GENDER, ISINPLACEMENTPLACED = x.ISINPLACEMENTPLACED, LAST_NAME = x.LAST_NAME, MIDDLE_INITIAL_NAME = x.MIDDLE_INITIAL_NAME, PinNumber = x.OTHER_PARTICIPANT, SourcePinNumber = x.PARTICIPANT, RELATIONSHIP = x.RELATIONSHIP }).ToList();

            foreach (var member in results)
            {
                this.GetAdditionDataForAgMember(member);
            }

            //add children
            var childData = this._dbContext.SPParticpantsChildrenFromCARES(pinNumber.ToString());
            foreach (var child in childData)
            {
                var agChild = new AssistanceGroupMember()
                              {
                                  AGE                 = child.AGE.GetValueOrDefault().ToString(),
                                  RELATIONSHIP        = "Child",
                                  BIRTH_DATE          = child.DOB_DT,
                                  FIRST_NAME          = child.FIRST_NAM,
                                  LAST_NAME           = child.LAST_NAM,
                                  MIDDLE_INITIAL_NAME = child.MIDDLE_INITIAL_NAM,
                                  GENDER              = child.GENDER,
                                  SourcePinNumber     = pinNumber,
                                  DEATH_DATE          = child.DEATH_DT
                              };
                results.Add(agChild);
            }

            return results;
        }

        private void GetAdditionDataForAgMember(AssistanceGroupMember member)
        {
            if (member.IsChild())
            {
                return;
            }

            Decimal pinNum             = member.PinNumber.GetValueOrDefault();
            var     AlienStatuses      = this.GetParticipantAlienStatus(pinNum);
            var     extensionSequences = this.GetTimelineExtensionSequences(pinNum);
            var     timelineMonths     = this.GetTimelineMonths(pinNum);
            var     placements         = this.GetPlacements(pinNum);

            //await Task.WhenAll(alienStatusTask, extensionSequencesTask, timelineMonthsTask).ConfigureAwait(false);
            member.AlienStatuses = AlienStatuses;
            member.Timeline.AddExtensionSequences(extensionSequences);
            member.Timeline.AddTimelineMonths(timelineMonths);
            member.Timeline.AddPlacements(placements);
        }

        public async Task<List<AssistanceGroupMember>> GetOtherAGMembersAsync(Decimal pinNumber, DateTime beginDate, DateTime endDate, CancellationToken token = default(CancellationToken))
        {
            var data    = await this._dbContext.SpOtherParticipantAsync(pinNumber.ToString(), beginDate, endDate, token).ConfigureAwait(false);
            var results = data.Select(x => new AssistanceGroupMember { AGE = x.AGE, BIRTH_DATE = x.BIRTH_DATE, DEATH_DATE = x.DEATH_DATE, ELIGIBILITY_PART_STATUS_CODE = x.ELIGIBILITY_PART_STATUS_CODE, FIRST_NAME = x.FIRST_NAME, GENDER = x.GENDER, ISINPLACEMENTPLACED = x.ISINPLACEMENTPLACED, LAST_NAME = x.LAST_NAME, MIDDLE_INITIAL_NAME = x.MIDDLE_INITIAL_NAME, PinNumber = x.OTHER_PARTICIPANT, SourcePinNumber = x.PARTICIPANT, RELATIONSHIP = x.RELATIONSHIP }).ToList();

            foreach (var member in results)
            {
                await this.GetAdditionDataForAgMemberAsync(member, token).ConfigureAwait(false);
            }

            //add children
            var childData = await this._dbContext.SPParticpantsChildrenFromCARESAsync(pinNumber.ToString(), token).ConfigureAwait(false);
            foreach (var child in childData)
            {
                var agChild = new AssistanceGroupMember()
                              {
                                  AGE                 = child.AGE.GetValueOrDefault().ToString(),
                                  RELATIONSHIP        = "Child",
                                  BIRTH_DATE          = child.DOB_DT,
                                  FIRST_NAME          = child.FIRST_NAM,
                                  LAST_NAME           = child.LAST_NAM,
                                  MIDDLE_INITIAL_NAME = child.MIDDLE_INITIAL_NAM,
                                  GENDER              = child.GENDER,
                                  SourcePinNumber     = pinNumber,
                                  DEATH_DATE          = child.DEATH_DT
                              };
                results.Add(agChild);
            }

            return results;
        }

        private async Task GetAdditionDataForAgMemberAsync(AssistanceGroupMember member, CancellationToken token = default(CancellationToken))
        {
            if (member.IsChild())
            {
                return;
            }

            Decimal pinNum             = member.PinNumber.GetValueOrDefault();
            var     AlienStatuses      = await this.GetParticipantAlienStatusAsync(pinNum, token).ConfigureAwait(false);
            var     extensionSequences = await this.GetTimelineExtensionSequencesAsync(pinNum, token).ConfigureAwait(false);
            var     timelineMonths     = await this.GetTimelineMonthsAsync(pinNum, token).ConfigureAwait(false);
            var     placements         = !member.IsChild() && member.ELIGIBILITY_PART_STATUS_CODE != "XA" ? await this.GetPlacementsAsync(pinNum, token).ConfigureAwait(false) : new List<Placement>();
            //await Task.WhenAll(alienStatusTask, extensionSequencesTask, timelineMonthsTask).ConfigureAwait(false);
            member.AlienStatuses = AlienStatuses;
            member.Timeline.AddExtensionSequences(extensionSequences);
            member.Timeline.AddTimelineMonths(timelineMonths);
            member.Timeline.AddPlacements(placements);
        }

        public async Task<List<TimeLimit>> TimeLimitsByPinAsync(Decimal pin, CancellationToken token = default(CancellationToken))
        {
            return await this._dbContext.TimeLimits.Where(x => x.Participant.PinNumber == pin && !x.IsDeleted).ToListAsync(token).ConfigureAwait(false);
        }

        public async Task<TimeLimit> TimeLimitByIdAsync(Int32 id, CancellationToken token = default(CancellationToken))
        {
            return await this._dbContext.TimeLimits.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, token).ConfigureAwait(false);
        }

        public async Task<TimeLimit> TimeLimitByDateAsync(Decimal pin, DateTime date, CancellationToken token = default(CancellationToken))
        {
            return await this._dbContext.TimeLimits.FirstOrDefaultAsync(x => x.Participant.PinNumber == pin && !x.IsDeleted && x.EffectiveMonth.HasValue && DbFunctions.DiffMonths(x.EffectiveMonth.Value, date) == 0, token).ConfigureAwait(false);
        }

        public List<TimeLimit> TimeLimitsByPin(Decimal pin)
        {
            return this._dbContext.TimeLimits.Where(x => x.Participant.PinNumber == pin && !x.IsDeleted).ToList();
        }

        public TimeLimit TimeLimitById(Int32 id)
        {
            return this._dbContext.TimeLimits.FirstOrDefault(x => x.Id == id && !x.IsDeleted);
        }

        public TimeLimit TimeLimitByDate(Decimal pin, DateTime date)
        {
            return this._dbContext.TimeLimits.FirstOrDefault(x => x.Participant.PinNumber == pin && !x.IsDeleted && x.EffectiveMonth.HasValue && DbFunctions.DiffMonths(x.EffectiveMonth.Value, date) == 0);
        }

        public TimeLimit NewTimeLimit()
        {
            var timelimit = this._dbContext.TimeLimits.Create<TimeLimit>();
            timelimit.IsDeleted                    = false;
            timelimit.CreatedDate                  = DateTime.Now;
            this._dbContext.Entry(timelimit).State = EntityState.Added;
            return timelimit;
        }

        public async Task<List<TimeLimitState>> TimeLimitStatesAsync(Boolean excludeWisconsin = true, CancellationToken token = default(CancellationToken))
        {
            IQueryable<TimeLimitState> query = this._dbContext.TimeLimitStates;
            if (excludeWisconsin)
            {
                query = query.Where(x => x.Code != "WI");
            }

            return await query.ToListAsync(token).ConfigureAwait(false);
        }


        public async Task<List<ChangeReason>> ChangeReasons(CancellationToken token = default(CancellationToken))
        {
            return await this._dbContext.ChangeReasons.ToListAsync(token).ConfigureAwait(false);
        }

        public async Task<Int32> SaveEntityAsync<T>(T entity, CancellationToken token = default(CancellationToken)) where T : class, ICommonDelCreatedModel
        {
            var dbEntry = this._dbContext.Entry(entity);
            if (entity.Id == default(Int32))
            {
                dbEntry.State = EntityState.Added;
            }
            else
            {
                //entity.ModifiedDate = DateTime.Now;
                dbEntry.State = EntityState.Modified;
            }

            return await this._dbContext.SaveChangesAsync(token).ConfigureAwait(false);
        }


        public Int32 SaveEntity<T>(T entity) where T : class, ICommonDelCreatedModel
        {
            var dbEntry = _dbContext.Entry(entity);
            if (entity.Id == default(Int32))
            {
                dbEntry.State = EntityState.Added;
            }
            else
            {
                //entity.ModifiedDate = DateTime.Now;
                dbEntry.State = EntityState.Modified;
            }

            return _dbContext.SaveChanges();
        }

        /// <summary>
        /// Updateds a db2record.
        /// Current sproc impelementation will only change history code 0 record to history code 9 records and ignore all other fields! After "updating", insert a new record with the update values
        /// and incremented history sequence number
        /// </summary>
        /// <param name="db2Record"></param>
        public void SPDB2T0459Update(IT0459_IN_W2_LIMITS db2Record)
        {
            throw new NotImplementedException();
            //_dbContext.DB2_T0459_Update(db2Record.PIN_NUM, db2Record.BENEFIT_MM, db2Record.HISTORY_SEQ_NUM, db2Record.CLOCK_TYPE_CD, db2Record.CRE_TRAN_CD, db2Record.FED_CLOCK_IND, db2Record.FED_CMP_MTH_NUM, db2Record.FED_MAX_MTH_NUM,
            //    db2Record.HISTORY_CD, db2Record.OT_CMP_MTH_NUM, db2Record.OVERRIDE_REASON_CD, db2Record.TOT_CMP_MTH_NUM, db2Record.TOT_MAX_MTH_NUM, db2Record.UPDATED_DT, db2Record.USER_ID, db2Record.WW_CMP_MTH_NUM, db2Record.WW_MAX_MTH_NUM, db2Record.COMMENT_TXT);
        }


        public void SPDB2T0459Insert(IT0459_IN_W2_LIMITS db2Record)
        {
            throw new NotImplementedException("SPDB2T0459Insert is not implemented exception.");
            //using (var _dbContext = this._dbContextFactory())
            //{

            //}
        }

        public ITimeline GetTimeline(Decimal pinNumber)
        {
            var timeline        = new Timeline();
            var timelimitMonths = this.GetTimelineMonths(pinNumber);
            var extensions      = this.GetTimelineExtensionSequences(pinNumber);
            timeline.AddExtensionSequences(extensions);
            timeline.AddTimelineMonths(timelimitMonths);
            return timeline;
        }

        public async Task<ITimeline> GetTimelineAsync(Decimal pinNumber, CancellationToken token = default(CancellationToken))
        {
            var timeline        = new Timeline();
            var timelimitMonths = await this.GetTimelineMonthsAsync(pinNumber, token).ConfigureAwait(false);
            var extensions      = await this.GetTimelineExtensionSequencesAsync(pinNumber, token).ConfigureAwait(false);

            timeline.AddExtensionSequences(extensions);
            timeline.AddTimelineMonths(timelimitMonths);
            return timeline;
        }

        public ITimeLimitSummary GetTimelimitSummary(Decimal pinNumber)
        {
            var timeLimitSummary = new TimeLimitSummary();
            var timeline         = this.GetTimeline(pinNumber);
            var participantId    = this._dbContext.Participants.Where(x => x.PinNumber == pinNumber).Select(x => x.Id).FirstOrDefault();
            this.CreateTimelimitSummary(timeline, participantId);

            return timeLimitSummary;
        }

        public async Task<ITimeLimitSummary> GetTimelimitSummaryAsync(Decimal pinNumber, CancellationToken token = default(CancellationToken))
        {
            var timeline         = await this.GetTimelineAsync(pinNumber, token).ConfigureAwait(false);
            var participantId    = await this._dbContext.Participants.Where(x => x.PinNumber == pinNumber).Select(x => x.Id).FirstOrDefaultAsync(token).ConfigureAwait(false);
            var timeLimitSummary = this.CreateTimelimitSummary(timeline, participantId);

            return timeLimitSummary;
        }

        public async Task<ITimeLimitSummary> CreateTimelimitSummaryAsync(Decimal pinNumber, CancellationToken token = default(CancellationToken))
        {
            var summary = await this.GetTimelimitSummaryAsync(pinNumber, token).ConfigureAwait(false);
            this.SaveEntityAsync(summary, token).ConfigureAwait(false);
            return summary;
        }

        public ITimeLimitSummary CreateTimelimitSummary(ITimeline timeline, Int32 participantId)
        {
            var summary = this._dbContext.TimeLimitSummaries.Create();
            summary.ParticipantId = participantId;
            summary.FederalUsed   = timeline.GetUsedMonths(ClockTypes.Federal);
            summary.FederalMax    = timeline.GetMaxMonths(ClockTypes.Federal);

            summary.StateUsed = timeline.GetUsedMonths(ClockTypes.State);
            summary.StateMax  = timeline.GetMaxMonths(ClockTypes.State);

            summary.CSJUsed = timeline.GetUsedMonths(ClockTypes.CSJ);
            summary.CSJMax  = timeline.GetMaxMonths(ClockTypes.CSJ);

            summary.W2TUsed = timeline.GetUsedMonths(ClockTypes.W2T);
            summary.W2TMax  = timeline.GetMaxMonths(ClockTypes.W2T);

            summary.TMPUsed = timeline.GetUsedMonths(ClockTypes.TMP);
            summary.TNPUsed = timeline.GetUsedMonths(ClockTypes.TNP);

            summary.TempUsed = timeline.GetUsedMonths(ClockTypes.TEMP);
            summary.TempMax  = timeline.GetMaxMonths(ClockTypes.TEMP);

            summary.CMCUsed = timeline.GetUsedMonths(ClockTypes.CMC);
            summary.CMCMax  = timeline.GetMaxMonths(ClockTypes.CMC);

            summary.OPCUsed = timeline.GetUsedMonths(ClockTypes.OPC);
            summary.OPCMax  = timeline.GetMaxMonths(ClockTypes.OPC);

            summary.OtherUsed = timeline.GetUsedMonths(ClockTypes.Other);
            summary.OtherMax  = timeline.GetMaxMonths(ClockTypes.Other);

            summary.OTF    = timeline.GetUsedMonths(ClockTypes.OTF);
            summary.Tribal = timeline.GetUsedMonths(ClockTypes.TRIBAL);

            summary.TJB  = timeline.GetUsedMonths(ClockTypes.TJB);
            summary.JOBS = timeline.GetUsedMonths(ClockTypes.JOBS);

            summary.NO24 = timeline.GetUsedMonths(ClockTypes.NoPlacementLimit);

            summary.IsDeleted    = false;
            summary.CreatedDate  = DateTime.Now;
            summary.ModifiedDate = DateTime.Now;
            summary.ModifiedBy   = "WWP";

            ClockStates csjClockState   = ClockStates.None;
            ClockStates w2tClockState   = ClockStates.None;
            ClockStates tempClockState  = ClockStates.None;
            ClockStates stateClockState = ClockStates.None;

            timeline.ClockStatesMap.TryGetValue(ClockTypes.CSJ,   out csjClockState);
            timeline.ClockStatesMap.TryGetValue(ClockTypes.W2T,   out w2tClockState);
            timeline.ClockStatesMap.TryGetValue(ClockTypes.TEMP,  out tempClockState);
            timeline.ClockStatesMap.TryGetValue(ClockTypes.State, out stateClockState);

            summary.CSJExtensionDue   = csjClockState.HasAnyFlags(ClockStates.Warn   | ClockStates.Danger);
            summary.W2TExtensionDue   = w2tClockState.HasAnyFlags(ClockStates.Warn   | ClockStates.Danger);
            summary.TempExtensionDue  = tempClockState.HasAnyFlags(ClockStates.Warn  | ClockStates.Danger);
            summary.StateExtensionDue = stateClockState.HasAnyFlags(ClockStates.Warn | ClockStates.Danger);

            return summary;
        }

        public TimeLimitWSSummary CreateTimeLimitWebServiceSummary(ITimeline timeLine, string pin, IQueryable<ITimeLimit> twentyFourFrom2009To2011)
        {
            var summary = new TimeLimitWSSummary
                          {
                              PinNumber        = pin,
                              TimeLimitSummary = new List<TimeLimitTicks>()
                          };

            var stateUsed     = timeLine.GetUsedMonthsCount(ClockTypes.State);
            var stateClockMax = timeLine.GetClockMax(ClockTypes.State);
            var stateMax      = timeLine.GetMaxMonthsByClockType(timeLine, ClockTypes.State);
            var state = new TimeLimitTicks
                        {
                            TimeLimitType            = TimeLimitTypeNames.State,
                            Max                      = timeLine.GetMaxMonthsCount(int.Parse(stateUsed), stateClockMax, int.Parse(stateUsed), stateMax, ClockTypes.State),
                            Used                     = stateUsed,
                            Remaining                = timeLine.GetRemainingMonthsCount(int.Parse(stateUsed), int.Parse(stateUsed), stateMax, stateClockMax, ClockTypes.State),
                            TwentyFourFrom2009To2011 = "-"
                        };

            var csjUsed = timeLine.GetUsedMonthsCount(ClockTypes.CSJ);
            var csjMax  = timeLine.GetMaxMonthsByClockType(timeLine, ClockTypes.CSJ);
            var csj24   = TwentyFourFrom2009To2011(twentyFourFrom2009To2011, (int) ClockTypes.CSJ);
            var csj = new TimeLimitTicks
                      {
                          TimeLimitType            = TimeLimitTypeNames.CSJ,
                          Max                      = timeLine.GetMaxMonthsCount(int.Parse(stateUsed), stateClockMax, int.Parse(csjUsed), csjMax, ClockTypes.CSJ),
                          Used                     = csjUsed,
                          Remaining                = timeLine.GetRemainingMonthsCount(int.Parse(csjUsed), int.Parse(stateUsed), csjMax, stateClockMax, ClockTypes.CSJ),
                          TwentyFourFrom2009To2011 = csj24
                      };

            var w2tUsed = timeLine.GetUsedMonthsCount(ClockTypes.W2T);
            var w2tMax  = timeLine.GetMaxMonthsByClockType(timeLine, ClockTypes.W2T);
            var w2t24   = TwentyFourFrom2009To2011(twentyFourFrom2009To2011, (int) ClockTypes.W2T);
            var w2t = new TimeLimitTicks
                      {
                          TimeLimitType            = TimeLimitTypeNames.W2T,
                          Max                      = timeLine.GetMaxMonthsCount(int.Parse(stateUsed), stateClockMax, int.Parse(w2tUsed), w2tMax, ClockTypes.W2T),
                          Used                     = w2tUsed,
                          Remaining                = timeLine.GetRemainingMonthsCount(int.Parse(w2tUsed), int.Parse(stateUsed), w2tMax, stateClockMax, ClockTypes.W2T),
                          TwentyFourFrom2009To2011 = w2t24
                      };

            var cmcUsed = timeLine.GetUsedMonthsCount(ClockTypes.CMC);
            var cmcMax  = timeLine.GetMaxMonthsByClockType(timeLine, ClockTypes.CMC);
            var cmc = new TimeLimitTicks
                      {
                          TimeLimitType            = TimeLimitTypeNames.CMC,
                          Max                      = timeLine.GetMaxMonthsCount(int.Parse(stateUsed), stateClockMax, int.Parse(cmcUsed), cmcMax, ClockTypes.CMC),
                          Used                     = cmcUsed,
                          Remaining                = timeLine.GetRemainingMonthsCount(int.Parse(cmcUsed), int.Parse(stateUsed), cmcMax, stateClockMax, ClockTypes.CMC),
                          TwentyFourFrom2009To2011 = "-"
                      };

            var tmpUsed = timeLine.GetUsedMonthsCount(ClockTypes.TMP);
            var tmpMax  = timeLine.GetMaxMonthsByClockType(timeLine, ClockTypes.TMP);
            var tmMax   = timeLine.GetMaxMonthsCount(int.Parse(stateUsed), stateClockMax, int.Parse(tmpUsed), tmpMax, ClockTypes.TMP);
            var tmp = new TimeLimitTicks
                      {
                          TimeLimitType            = TimeLimitTypeNames.TMP,
                          Max                      = "-",
                          Used                     = tmpUsed,
                          Remaining                = "-",
                          TwentyFourFrom2009To2011 = "-"
                      };

            var tnpUsed = timeLine.GetUsedMonthsCount(ClockTypes.TNP);
            var tnpMax  = timeLine.GetMaxMonthsByClockType(timeLine, ClockTypes.TNP);
            var tnMax   = timeLine.GetMaxMonthsCount(int.Parse(stateUsed), stateClockMax, int.Parse(tnpUsed), tnpMax, ClockTypes.TNP);
            var tnp = new TimeLimitTicks
                      {
                          TimeLimitType            = TimeLimitTypeNames.TNP,
                          Max                      = "-",
                          Used                     = tnpUsed,
                          Remaining                = "-",
                          TwentyFourFrom2009To2011 = "-"
                      };

            var max = Math.Max(int.Parse(tmMax == "" ? "0" : tmMax), int.Parse(tnMax == "" ? "0" : tnMax)).ToString();
            var rem = (int.Parse(max) - (int.Parse(tmpUsed) + int.Parse(tnpUsed))).ToString();
            var temp = new TimeLimitTicks
                       {
                           TimeLimitType            = TimeLimitTypeNames.Temp,
                           Max                      = max == "0" ? "-" : max,
                           Used                     = (int.Parse(tmp.Used) + int.Parse(tnp.Used)).ToString(),
                           Remaining                = rem == "0" ? "-" : rem,
                           TwentyFourFrom2009To2011 = "-"
                       };

            var tjbUsed = timeLine.GetUsedMonthsCount(ClockTypes.TJB);
            var tjbMax  = timeLine.GetMaxMonthsByClockType(timeLine, ClockTypes.TJB);
            var tjb24   = TwentyFourFrom2009To2011(twentyFourFrom2009To2011, (int) ClockTypes.TJB);
            var tjb = new TimeLimitTicks
                      {
                          TimeLimitType            = TimeLimitTypeNames.TJB,
                          Max                      = timeLine.GetMaxMonthsCount(int.Parse(stateUsed), stateClockMax, int.Parse(tjbUsed), tjbMax, ClockTypes.TJB),
                          Used                     = tjbUsed,
                          Remaining                = timeLine.GetRemainingMonthsCount(int.Parse(tjbUsed), int.Parse(stateUsed), tjbMax, stateClockMax, ClockTypes.TJB),
                          TwentyFourFrom2009To2011 = tjb24
                      };

            var opcUsed = timeLine.GetUsedMonthsCount(ClockTypes.OPC);
            var opcMax  = timeLine.GetMaxMonthsByClockType(timeLine, ClockTypes.OPC);
            var opc = new TimeLimitTicks
                      {
                          TimeLimitType            = TimeLimitTypeNames.OPC,
                          Max                      = timeLine.GetMaxMonthsCount(int.Parse(stateUsed), stateClockMax, int.Parse(opcUsed), opcMax, ClockTypes.OPC),
                          Used                     = opcUsed,
                          Remaining                = timeLine.GetRemainingMonthsCount(int.Parse(opcUsed), int.Parse(stateUsed), opcMax, stateClockMax, ClockTypes.OPC),
                          TwentyFourFrom2009To2011 = "-"
                      };

            var otfUsed = timeLine.GetUsedMonthsCount(ClockTypes.OTF);
            var otfMax  = timeLine.GetMaxMonthsByClockType(timeLine, ClockTypes.OTF);
            var otf = new TimeLimitTicks
                      {
                          TimeLimitType            = TimeLimitTypeNames.OTF,
                          Max                      = timeLine.GetMaxMonthsCount(int.Parse(stateUsed), stateClockMax, int.Parse(otfUsed), otfMax, ClockTypes.OTF),
                          Used                     = otfUsed,
                          Remaining                = timeLine.GetRemainingMonthsCount(int.Parse(otfUsed), int.Parse(stateUsed), otfMax, stateClockMax, ClockTypes.OTF),
                          TwentyFourFrom2009To2011 = "-"
                      };

            var jobsUsed = timeLine.GetUsedMonthsCount(ClockTypes.JOBS);
            var jobsMax  = timeLine.GetMaxMonthsByClockType(timeLine, ClockTypes.JOBS);
            var jobs = new TimeLimitTicks
                       {
                           TimeLimitType            = TimeLimitTypeNames.Jobs,
                           Max                      = timeLine.GetMaxMonthsCount(int.Parse(stateUsed), stateClockMax, int.Parse(jobsUsed), jobsMax, ClockTypes.JOBS),
                           Used                     = jobsUsed,
                           Remaining                = timeLine.GetRemainingMonthsCount(int.Parse(jobsUsed), int.Parse(stateUsed), jobsMax, stateClockMax, ClockTypes.JOBS),
                           TwentyFourFrom2009To2011 = "-"
                       };

            var fedUsed = timeLine.GetUsedMonthsCount(ClockTypes.Federal);
            var fedMax  = timeLine.GetMaxMonthsByClockType(timeLine, ClockTypes.Federal);
            var federal = new TimeLimitTicks
                          {
                              TimeLimitType            = TimeLimitTypeNames.Federal,
                              Max                      = timeLine.GetMaxMonthsCount(int.Parse(stateUsed), stateClockMax, int.Parse(fedUsed), fedMax, ClockTypes.Federal),
                              Used                     = fedUsed,
                              Remaining                = timeLine.GetRemainingMonthsCount(int.Parse(fedUsed), int.Parse(stateUsed), fedMax, stateClockMax, ClockTypes.Federal),
                              TwentyFourFrom2009To2011 = "-"
                          };

            var tribalUsed = timeLine.GetUsedMonthsCount(ClockTypes.TRIBAL);
            var tribalMax  = timeLine.GetMaxMonthsByClockType(timeLine, ClockTypes.TRIBAL);
            var tribal = new TimeLimitTicks
                         {
                             TimeLimitType            = TimeLimitTypeNames.Tribal,
                             Max                      = timeLine.GetMaxMonthsCount(int.Parse(stateUsed), stateClockMax, int.Parse(tribalUsed), tribalMax, ClockTypes.TRIBAL),
                             Used                     = tribalUsed,
                             Remaining                = timeLine.GetRemainingMonthsCount(int.Parse(tribalUsed), int.Parse(stateUsed), tribalMax, stateClockMax, ClockTypes.TRIBAL),
                             TwentyFourFrom2009To2011 = "-"
                         };

            var no24Used = timeLine.GetUsedMonthsCount(ClockTypes.NoPlacementLimit);
            var no24Max  = timeLine.GetMaxMonthsByClockType(timeLine, ClockTypes.NoPlacementLimit);
            var no24 = new TimeLimitTicks
                       {
                           TimeLimitType            = TimeLimitTypeNames.No24,
                           Max                      = timeLine.GetMaxMonthsCount(int.Parse(stateUsed), stateClockMax, int.Parse(no24Used), no24Max, ClockTypes.NoPlacementLimit),
                           Used                     = no24Used,
                           Remaining                = timeLine.GetRemainingMonthsCount(int.Parse(no24Used), int.Parse(stateUsed), no24Max, stateClockMax, ClockTypes.NoPlacementLimit),
                           TwentyFourFrom2009To2011 = "-"
                       };

            if (isZeroOrEmpty(stateUsed)  && isZeroOrEmpty(csjUsed) && isZeroOrEmpty(w2tUsed)  && isZeroOrEmpty(cmcUsed) && isZeroOrEmpty(temp.Used)  && isZeroOrEmpty(tjbUsed)
                && isZeroOrEmpty(opcUsed) && isZeroOrEmpty(otfUsed) && isZeroOrEmpty(jobsUsed) && isZeroOrEmpty(fedUsed) && isZeroOrEmpty(tribalUsed) && isZeroOrEmpty(no24Used))
            {
                summary.IsDataFound = false;
            }
            else
            {
                summary.IsDataFound = true;
                summary.TimeLimitSummary.Add(state);
                summary.TimeLimitSummary.Add(csj);
                summary.TimeLimitSummary.Add(w2t);
                summary.TimeLimitSummary.Add(cmc);
                summary.TimeLimitSummary.Add(temp);
                summary.TimeLimitSummary.Add(tmp);
                summary.TimeLimitSummary.Add(tnp);
                summary.TimeLimitSummary.Add(tjb);
                summary.TimeLimitSummary.Add(opc);
                summary.TimeLimitSummary.Add(otf);
                summary.TimeLimitSummary.Add(jobs);
                summary.TimeLimitSummary.Add(federal);
                summary.TimeLimitSummary.Add(tribal);
                summary.TimeLimitSummary.Add(no24);
            }

            return summary;
        }

        private bool isZeroOrEmpty(string used)
        {
            return used == "" || used == "0";
        }

        private string TwentyFourFrom2009To2011(IQueryable<ITimeLimit> twentyFourFrom2009To2011, int flag)
        {
            var beginMonth = DateTime.Parse("2009-11-01");
            var endMonth   = DateTime.Parse("2011-12-31");
            var twentyFourUsed = twentyFourFrom2009To2011.Count(i => i.TimeLimitTypeId == flag && i.EffectiveMonth >= beginMonth
                                                                                               && i.EffectiveMonth <= endMonth && i.TwentyFourMonthLimit == false
                                                                                               && i.Notes.Contains("No 24 months for time period between 2009-11-01 and 2011-12-31"))
                                                         .ToString();

            return twentyFourUsed;
        }

        public Task SpTimeLimitPlacementClosureAsync(Decimal caseNumber, DateTime databaseDate, string inputUserId, DateTime existingEpisodeBeginDate, Decimal pinNumber, string existingFepId, DateTime existingEpisodeEndDate, string existingPlacementCode, DateTime existingPlacementBeginDate, string newFepIdNumber, DateTime newEpisodeEndDate, string newPlacementCode, CancellationToken token = default(CancellationToken))
        {
            return this._dbContext.SpTimeLimitPlacementClosureAsync(caseNumber,
                                                                    databaseDate,
                                                                    inputUserId,
                                                                    existingEpisodeBeginDate,
                                                                    pinNumber,
                                                                    existingFepId,
                                                                    existingEpisodeEndDate,
                                                                    existingPlacementCode,
                                                                    existingPlacementBeginDate,
                                                                    newFepIdNumber,
                                                                    newEpisodeEndDate,
                                                                    newPlacementCode, token);
        }

        public void SpTimeLimitPlacementClosure(Decimal caseNumber, DateTime databaseDate, string inputUserId, DateTime existingEpisodeBeginDate, Decimal pinNumber, string existingFepId, DateTime existingEpisodeEndDate, string existingPlacementCode, DateTime existingPlacementBeginDate, string newFepIdNumber, DateTime newEpisodeEndDate, string newPlacementCode)
        {
            this._dbContext.SpTimeLimitPlacementClosure(caseNumber,
                                                        databaseDate,
                                                        inputUserId,
                                                        existingEpisodeBeginDate,
                                                        pinNumber,
                                                        existingFepId,
                                                        existingEpisodeEndDate,
                                                        existingPlacementCode,
                                                        existingPlacementBeginDate,
                                                        newFepIdNumber,
                                                        newEpisodeEndDate,
                                                        newPlacementCode);
        }

        public async Task<List<IT0459_IN_W2_LIMITS>> GetLatestW2LimitsMonthsForEachClockTypeAsync(Decimal pinNum, CancellationToken token = default(CancellationToken))
        {
            var results = await this._dbContext.T0459_IN_W2_LIMITS.Where(x => x.PIN_NUM == pinNum && x.HISTORY_CD == 0 && x.OVERRIDE_REASON_CD.StartsWith("S") == false).GroupBy(x => x.CLOCK_TYPE_CD.Trim())
                                    .Select(y => y.FirstOrDefault(z => z.BENEFIT_MM == y.Max(d => d.BENEFIT_MM))).ToListAsync(token).ConfigureAwait(false);

            return new List<IT0459_IN_W2_LIMITS>(results);
        }

        public async Task<IT0459_IN_W2_LIMITS> GetLatestW2LimitsByClockTypeAsync(Decimal pinNum, ClockTypes clockType, CancellationToken token = default(CancellationToken))
        {
            var clockTypeCode = clockType.ToString();
            var query         = await this.GetLatestW2LimitsMonthsForEachClockTypeAsync(pinNum, token).ConfigureAwait(false);
            return query.FirstOrDefault(x => x.CLOCK_TYPE_CD.Trim() == clockTypeCode);
        }

        //public void DisableT0459_IN_W2_LIMITS_Triggers()
        //{
        //    using (var _dbContext = this._dbContextFactory())
        //    {
        //        var script = @"DISABLE TRIGGER [wwp].[T0459_AFTER_INSERT] ON  [wwp].[T0459_IN_W2_LIMITS];
        //                       DISABLE TRIGGER [wwp].[T0459_AFTER_UPDATE] ON  [wwp].[T0459_IN_W2_LIMITS];";
        //        _dbContext.Database.ExecuteSqlCommand(script);
        //    } 
        //}

        //public void EnableT0459_IN_W2_LIMITS_Triggers()
        //{
        //    using (var _dbContext = this._dbContextFactory())
        //    {
        //        var script = @"ENABLE TRIGGER [wwp].[T0459_AFTER_INSERT] ON  [wwp].[T0459_IN_W2_LIMITS];
        //                       ENABLE TRIGGER [wwp].[T0459_AFTER_UPDATE] ON  [wwp].[T0459_IN_W2_LIMITS];";
        //        _dbContext.Database.ExecuteSqlCommand(script);
        //    }
        //}

        //public void DisableT0460_IN_W2_EXT_Triggers()
        //{
        //    using (var _dbContext = this._dbContextFactory())
        //    {
        //        var script = @"DISABLE TRIGGER [wwp].[T0460_AFTER_INSERT] ON  [wwp].[T0460_IN_W2_EXT];
        //                       DISABLE TRIGGER [wwp].[T0460_AFTER_UPDATE] ON  [wwp].[T0460_IN_W2_EXT];";
        //        _dbContext.Database.ExecuteSqlCommand(script);
        //    }
        //}

        //public void EnableT0460_IN_W2_EXT_Triggers()
        //{
        //    using (var _dbContext = this._dbContextFactory())
        //    {
        //        var script = @"ENABLE TRIGGER [wwp].[T0460_AFTER_INSERT] ON  [wwp].[T0460_IN_W2_EXT];
        //                       ENABLE TRIGGER [wwp].[T0460_AFTER_UPDATE] ON  [wwp].[T0460_IN_W2_EXT];";
        //        _dbContext.Database.ExecuteSqlCommand(script);
        //    }
        //}

        public void Dispose()
        {
            try
            {
                this._dbContext?.Dispose();
                //if (this._dbContext?.Database?.Connection?.State != ConnectionState.Closed)
                //{
                //    this._dbContext?.Database?.Connection?.Close();

                //}
            }
            catch (Exception)
            {
            }
        }


        //TODO: Move to common pin tools
        public static Dictionary<Int32, Tuple<Decimal, Decimal>> PartitionRangeRanges => new Dictionary<Int32, Tuple<Decimal, Decimal>>
                                                                                         {
                                                                                             { 0, new Tuple<Decimal, Decimal>(0000000000, 0999999999) },
                                                                                             { 1, new Tuple<Decimal, Decimal>(1000000000, 1999999999) },
                                                                                             { 2, new Tuple<Decimal, Decimal>(2000000000, 2999999999) },
                                                                                             { 3, new Tuple<Decimal, Decimal>(3000000000, 3999999999) },
                                                                                             { 4, new Tuple<Decimal, Decimal>(4000000000, 4999999999) },
                                                                                             { 5, new Tuple<Decimal, Decimal>(5000000000, 5999999999) },
                                                                                             { 6, new Tuple<Decimal, Decimal>(6000000000, 6999999999) },
                                                                                             { 7, new Tuple<Decimal, Decimal>(7000000000, 7999999999) },
                                                                                             { 8, new Tuple<Decimal, Decimal>(8000000000, 8999999999) },
                                                                                             { 9, new Tuple<Decimal, Decimal>(9000000000, 9999999999) }
                                                                                         };
    }
}
