using Dcf.Wwp.Api.Library.Contracts.Timelimits;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Core;
using Dcf.Wwp.Model.Interface.Repository;
using DCF.Common.Dates;
using DCF.Common.Exceptions;
using DCF.Common.Extensions;
using DCF.Timelimits.Rules.Domain;
using DCF.Timelimts.Service;
using EnumsNET;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Dcf.Wwp.Data.Sql.Model;
using DCF.Common.Dates;
using DCF.Common.Extensions;
using DCF.Common.Logging;
using Dcf.Wwp.Api.Library.Extensions;

namespace Dcf.Wwp.Api.Library.ViewModels
{
    public class TimeLineViewModel : BaseViewModel
    {
        private readonly ITimelimitService _timelimitService;

        public TimeLineViewModel(IRepository repository, IAuthUser authUser, ITimelimitService timelimitService) : base(repository, authUser)
        {
            this._timelimitService = timelimitService;
        }

        public TimelineContract GetTimelineByPin(String pin)
        {
            var particpant = Repo.GetParticipant(pin);
            ImportAnyAuxTicks(pin, particpant);

            var timeline = new TimelineContract();
            var months   = this.Repo.TimeLimitsByPin(pin);
            var extensionGroups = this.Repo.GetExtensionsByPin(pin).GroupBy(x => new Tuple<Int32, Int32>(x.TimeLimitTypeId.GetValueOrDefault(), x.ExtensionSequence.GetValueOrDefault()))
                                      .Select(x => x);

            timeline.TimelineMonths.AddRange(
                                             months.Select(TimelineMonthContract.Create));
            timeline.ExtensionSequences.AddRange(extensionGroups.Select(
                                                                        x => ExtensionSequenceContract.Create(x.Key.Item1, x.Key.Item2, x.ToList())));


            return timeline;
        }

        public TimelineSummaryContract GetTimelineSnapshot(String pin, Boolean save)
        {
            var     participant = this.Repo.GetParticipant(pin);
            Decimal pinDecimal;
            if (participant == null || !Decimal.TryParse(pin, out pinDecimal))
            {
                throw new DCFApplicationException($"Error Creating TimelineSnapshot. Invalid pin number: {pin}");
            }

            var timelineSummary = this._timelimitService.GetTimelimitSummary(participant.PinNumber.GetValueOrDefault());
            if (save)
            {
                timelineSummary.ModifiedBy = this.AuthUser?.Username ?? "unknown";
                this.Repo.Save();
            }

            var contract = TimelineSummaryContract.Create(timelineSummary);
            return contract;
        }

        public ITimeLimitSummary CreateTimelimitSummary(ITimeline timeline, Int32 participantId)
        {
            var summary = this.Repo.NewTimeLimitSummary();
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
            summary.ModifiedBy   = this.AuthUser?.Username ?? "unknown";
            summary.ModifiedDate = DateTime.Now;

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

        public ITimeLimitSummary GetT0459Counts(String pin, Int32 id)
        {
            var     participant = this.Repo.GetParticipant(pin);
            Decimal pinDecimal;
            if (participant == null || !Decimal.TryParse(pin, out pinDecimal))
            {
                throw new DCFApplicationException($"Error Creating TimelineSnapshot. Invalid pin number: {pin}");
            }

            var timeline = this._timelimitService.GetTimeline(participant.PinNumber.GetValueOrDefault());


            var clockTick       = this.Repo.GetW2LimitsByPin(Decimal.Parse(pin)).FirstOrDefault(x => x.Id == id);
            var benefitDate     = DateTime.ParseExact(clockTick.BENEFIT_MM.ToString(CultureInfo.InvariantCulture), "yyyyMM", CultureInfo.InvariantCulture);
            var currentStateMax = timeline.GetMaxMonths(ClockTypes.State);
            timeline.TimelineDate = benefitDate;
            //var timelineSummary = this.CreateTimelimitSummary(timeline, participant.Id);
            var timelineSummary = this._timelimitService.CreateTimelimitSummary(timeline, participant.Id);

            var     clockType             = (ClockTypes) Enum.Parse(typeof(ClockTypes), clockTick.CLOCK_TYPE_CD);
            var     lastMonthForClockType = timeline.GetTimelineMonths(clockType).GetMax(x => x.Date)?.Date;
            Boolean isLatestTick          = lastMonthForClockType?.IsSame(benefitDate, DateTimeUnit.Month) ?? false;
            if (isLatestTick)
            {
                timelineSummary.StateMax = currentStateMax;
            }

            return timelineSummary;
        }

        #region  Aux Imports

        public void ImportAnyAuxTicks(string pin, IParticipantInfo participant)
        {
            var no24DateRange = new DateTimeRange(new DateTime(2009, 11, 01), new DateTime(2011, 12, 31));
            try
            {
                var decimalPin     = Decimal.Parse(pin);
                var missingRecords = _timelimitService.GetAuxillaryPayments(decimalPin);
                var timelimits     = Repo.TimeLimitsByPin(pin).ToDictionary(x => x.EffectiveMonth);
                foreach (var tick in missingRecords)
                {
                    var        month = DateTime.ParseExact(tick.BENEFIT_MM.Value.ToString(CultureInfo.InvariantCulture), "yyyyMM", CultureInfo.InvariantCulture);
                    ITimeLimit timelimit;
                    timelimits.TryGetValue(month, out timelimit);
                    if (timelimit == null)
                    {
                        timelimit = Repo.NewTimeLimit();
                    }
                    else
                        if (timelimit.TimeLimitTypeId == (Int32) ClockTypes.None || timelimit.TimeLimitTypeId == (Int32) ClockTypes.OTF)
                        {
                            this.Logger.Info($"Edited \"{(ClockTypes) timelimit.TimeLimitTypeId.Value}\" Timelimit record found. Overwriting! ");
                        }
                        else
                            if (timelimit.ModifiedDate.HasValue)
                            {
                                this.Logger.Info($"Edited Timelimit record found with ClockType: \"{(ClockTypes) timelimit.TimeLimitTypeId.Value}\". skipping! ");
                                continue;
                            }
                            else
                            {
                                this.Logger.Info($"Batch Timelimit record found with ClockType: \"{(ClockTypes) timelimit.TimeLimitTypeId.Value}\". Skipping! ");
                                continue;
                            }

                    ClockTypes clockType;
                    if (!Enum.TryParse(tick.CLOCK_TYPE_CD, out clockType))
                    {
                        this.Logger.Info($" Unable to parse clocktype with CLOCK_TYPE_CD: \"{tick.CLOCK_TYPE_CD}\". Skipping! ");
                    }

                    timelimit.ParticipantID        = participant.Id;
                    timelimit.EffectiveMonth       = month.StartOf(DateTimeUnit.Month);
                    timelimit.TimeLimitTypeId      = (Int32) clockType;
                    timelimit.TwentyFourMonthLimit = clockType.HasAnyFlags(ClockTypes.PlacementTypes) && !no24DateRange.Contains(timelimit.EffectiveMonth.Value);
                    timelimit.StateTimelimit       = true;
                    timelimit.FederalTimeLimit     = tick.FED_CLOCK_IND == "Y";
                    timelimit.CreatedDate          = tick.UPDATED_DT;
                    timelimit.ModifiedBy           = "WWP Batch";
                    timelimit.Notes                = $"Imported missing month created by:{tick.CRE_TRAN_CD}, Comments from old WP application: {tick.COMMENT_TXT}";
                    timelimit.IsDeleted            = tick.OVERRIDE_REASON_CD?.ToUpper().StartsWith("S") ?? false;
                    tick.PinNumber                 = participant.PinNumber;
                    tick.EffectiveMonth            = timelimit.EffectiveMonth;
                    tick.TimeLimitTypeId           = timelimit.TimeLimitTypeId;
                    tick.StateTimelimit            = timelimit.StateTimelimit;
                    tick.FederalTimeLimit          = timelimit.FederalTimeLimit;
                    tick.TwentyFourMonthLimit      = timelimit.TwentyFourMonthLimit;
                    tick.CreatedDateFromCARES      = tick.UPDATED_DT;
                    tick.ModifiedBy                = timelimit.ModifiedBy;
                    tick.ModifiedDate              = timelimit.ModifiedDate;
                    Repo.Save();
                    this._timelimitService.SaveEntity(tick);
                    var timeline = _timelimitService.GetTimeline(decimalPin);
                    var tls      = _timelimitService.CreateTimelimitSummary(timeline, participant.Id);
                    _timelimitService.SaveEntity(tls);
                }
            }
            catch (Exception ex)
            {
                this.Logger.ErrorException("Failed to import Auxilliary payment for participant {pinNumber}", ex, pin);
            }
        }

        private void MapAuxTickToT049(IT0459_IN_W2_LIMITS t049AuxTick, AuxiliaryPayment auxTick)
        {
            t049AuxTick.PIN_NUM            = auxTick.PIN_NUM.GetValueOrDefault();
            t049AuxTick.BENEFIT_MM         = auxTick.BENEFIT_MM.GetValueOrDefault();
            t049AuxTick.HISTORY_SEQ_NUM    = auxTick.HISTORY_SEQ_NUM.GetValueOrDefault();
            t049AuxTick.CLOCK_TYPE_CD      = auxTick.CLOCK_TYPE_CD;
            t049AuxTick.CRE_TRAN_CD        = auxTick.CRE_TRAN_CD;
            t049AuxTick.FED_CLOCK_IND      = auxTick.FED_CLOCK_IND;
            t049AuxTick.FED_CMP_MTH_NUM    = auxTick.FED_CMP_MTH_NUM.GetValueOrDefault();
            t049AuxTick.FED_MAX_MTH_NUM    = auxTick.FED_MAX_MTH_NUM.GetValueOrDefault();
            t049AuxTick.HISTORY_CD         = auxTick.HISTORY_CD.GetValueOrDefault();
            t049AuxTick.OT_CMP_MTH_NUM     = auxTick.OT_CMP_MTH_NUM.GetValueOrDefault();
            t049AuxTick.OVERRIDE_REASON_CD = auxTick.OVERRIDE_REASON_CD;
            t049AuxTick.TOT_CMP_MTH_NUM    = auxTick.TOT_CMP_MTH_NUM.GetValueOrDefault();
            t049AuxTick.TOT_MAX_MTH_NUM    = auxTick.TOT_MAX_MTH_NUM.GetValueOrDefault();
            t049AuxTick.UPDATED_DT         = auxTick.UPDATED_DT.GetValueOrDefault();
            t049AuxTick.USER_ID            = auxTick.USER_ID;
            t049AuxTick.WW_CMP_MTH_NUM     = auxTick.WW_CMP_MTH_NUM.GetValueOrDefault();
            t049AuxTick.WW_MAX_MTH_NUM     = auxTick.WW_MAX_MTH_NUM.GetValueOrDefault();
            t049AuxTick.COMMENT_TXT        = auxTick.COMMENT_TXT;
        }

        #endregion

        #region TimeLimits WebService

        public TimeLimitWebService GetTimelineByPins(string pins)
        {
            var timeLimitWebService = new TimeLimitWebService
                                      {
                                          Participants = new List<TimeLimitWSSummary>()
                                      };
            var pinList = pins.Split(',');

            foreach (var pin in pinList)
            {
                var participant = Repo.GetParticipant(pin);

                if (participant != null)
                {
                    ImportAnyAuxTicks(pin, participant);
                    var twentyFourFrom2009To2011 = Repo.GetTimeLimit(participant.Id);
                    var summary                  = _timelimitService.CreateTimeLimitWebServiceSummary(_timelimitService.GetTimeline(decimal.Parse(pin)), pin, twentyFourFrom2009To2011);

                    timeLimitWebService.Participants.Add(summary);
                }
                else
                {
                    var summary = new TimeLimitWSSummary
                                  {
                                      IsDataFound      = false,
                                      PinNumber        = pin,
                                      TimeLimitSummary = new List<TimeLimitTicks>()
                                  };
                    timeLimitWebService.Participants.Add(summary);
                }
            }

            return timeLimitWebService;
        }

        #endregion
    }
}
