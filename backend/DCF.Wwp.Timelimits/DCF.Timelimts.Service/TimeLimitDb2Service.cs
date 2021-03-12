using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Dcf.Wwp.Data.Sql.Model;
using Dcf.Wwp.Model;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Repository;
using DCF.Common.Dates;
using DCF.Common.Extensions;
using DCF.Timelimits.Rules.Domain;
using DCF.Timelimts.Service;
using EnumsNET;
using FileHelpers;
using ExtensionDecision = DCF.Timelimits.Rules.Domain.ExtensionDecision;


namespace Dcf.Wwp.Api.Library.Services
{
    public class Db2TimelimitService : IDb2TimelimitService
    {
        public static DateTime HighDate = new DateTime(9999, 12, 31);

        // Name of application making changes to DB2 table. In our case it's WPASS.
        public static String TransCode { get; set; } = "WPASS";
        public const  Int16  FedMaxMonthLimit = 60;

        public virtual IRepository _repo { get; set; }

        public virtual ITimelimitService _timelimitService { get; set; }

        public virtual Lazy<List<IApprovalReason>> approvalReasons { get; set; }
        public virtual Lazy<List<IDenialReason>>   denialReasons   { get; set; }
        public virtual Lazy<List<IChangeReason>>   changeReasons   { get; set; }
        public         Boolean                     IsSimulated     { get; set; } = false;


        private FileHelperAsyncEngine<ExtensionNoticeRecord> _fixedLengthFileEngine = new FileHelperAsyncEngine<ExtensionNoticeRecord>(Encoding.UTF8);

        public Db2TimelimitService(IRepository repo, ITimelimitService timelimitService)
        {
            this._repo             = repo;
            this._timelimitService = timelimitService;

            this.approvalReasons = new Lazy<List<IApprovalReason>>(() => { return this._repo.GetExtensionApprovalReasons().ToList(); }, LazyThreadSafetyMode.ExecutionAndPublication);
            this.denialReasons   = new Lazy<List<IDenialReason>>(() => { return this._repo.GetExtensionDenialReasons().ToList(); }, LazyThreadSafetyMode.ExecutionAndPublication);
            this.changeReasons   = new Lazy<List<IChangeReason>>(() => { return this._repo.ChangeReasons().ToList(); }, LazyThreadSafetyMode.ExecutionAndPublication);


            ConverterBase.DefaultDateTimeFormat = "yyyy-MM-dd";
        }


        #region Timelimit T0459 methods

        public IT0459_IN_W2_LIMITS Upsert(ITimeLimit timeLimitModel, IParticipantInfo particpant, String wamsId, DateTime? updatedDate = null)
        {
            var mainFrameUserId = wamsId.IsNullOrEmpty() ? null : this._repo.WorkerByWamsId(wamsId)?.MFUserId;
            //if (mainFrameUserId == null)
            //    throw new Exception("User not found.");

            Decimal effectiveMonth;
            if (!Decimal.TryParse(timeLimitModel.EffectiveMonth.ToStringMonthYearComposite(), out effectiveMonth))
                throw new Exception("Effective month in wrong format.");


            var upsertedTick = this.TickRecords(timeLimitModel, effectiveMonth, particpant, mainFrameUserId, updatedDate);

            // Save to SQL SERVER.
            this.Save();

            return upsertedTick;
        }

        public virtual IT0459_IN_W2_LIMITS TickRecords(ITimeLimit timeLimitModel, Decimal effectiveMonth, IParticipantInfo participant, String mainFrameUserId, DateTime? updatedDate = null)
        {
            var timeline      = this._timelimitService.GetTimeline(participant.PinNumber.GetValueOrDefault());
            var timelineMonth = TimelimitService.MapTimelimitToTimelineMonth(timeLimitModel);
            if (timeLimitModel.IsDeleted || timeLimitModel.TimeLimitTypeId == (Int32) ClockTypes.None)
            {
                TimelineMonth m = null;
                timeline.Months.TryRemove(timeLimitModel.EffectiveMonth.GetValueOrDefault(), out m);
            }
            else
            {
                timeline.AddTimelineMonth(timelineMonth);
            }

            var pinNum = participant.PinNumber.GetValueOrDefault();

            var originalTick = this._repo.GetW2LimitByMonth(effectiveMonth, pinNum);
            var tick         = this.InsertTick0459(timeLimitModel, originalTick, participant, mainFrameUserId, timeline, updatedDate);
            this.Save(); // so that GetLatestW2LimitsMonthsForEachClockType will return this tick if it's the latest or the previous one if it's subtracted

            if (tick != null)
            {
                List<IT0459_IN_W2_LIMITS> ticksToUpdate = new List<IT0459_IN_W2_LIMITS>();

                var latestW2Limits = this._repo.GetLatestW2LimitsMonthsForEachClockType(pinNum);
                //var subsequentW2Limits = this._repo.GetSubsequentW2Limits(pinNum, timelineMonth.Date);

                // Remove the inserted Tick from the latest latestW2Limits list, it is already updated
                if (latestW2Limits.Any(x => x.BENEFIT_MM == tick.BENEFIT_MM))
                {
                    var latestMatch = latestW2Limits.First(x => x.BENEFIT_MM == tick.BENEFIT_MM);
                    latestW2Limits.Remove(latestMatch); //So we don't update the same tick if it is the original one
                    //this.Dettach(latestMatch); // so we dont' get stupid error.
                }

                //// Remove any latestW2Limits from the subsequentS2Limits list
                //foreach (var latestTick in latestW2Limits)
                //{
                //    var subTick = subsequentW2Limits.FirstOrDefault(x => x.BENEFIT_MM == latestTick.BENEFIT_MM);
                //    if (subTick != null)
                //    {
                //        subsequentW2Limits.Remove(subTick);
                //        //this.Dettach(subTick);
                //    }
                //}

                ticksToUpdate.AddRange(latestW2Limits);
                //ticksToUpdate.AddRange(subsequentW2Limits);

                this.UpdateTicks0459(ticksToUpdate, timeline, updatedDate);
            }

            return tick;
        }

        //public virtual Timeline LoadTimeline(Int32 participantId)
        //{
        //    var timeline = new Timeline();
        //    var timelimitMonths = this._timelimitService.GetTimelineMonths(participantId);
        //    var extensions = this._timelimitService.GetTimelineExtensionSequences(participantId);
        //    timeline.AddExtensionSequences(extensions);
        //    timeline.AddTimelineMonths(timelimitMonths);
        //    return timeline;
        //}

        public IT0459_IN_W2_LIMITS InsertTick0459(ITimeLimit timeLimitModel, IT0459_IN_W2_LIMITS originalTick, IParticipantInfo participant, String mainFrameUser, ITimeline timeline, DateTime? updatedDate = null)
        {
            var benefitMmModel = Decimal.Parse(timeLimitModel.EffectiveMonth.ToStringMonthYearComposite());

            IT0459_IN_W2_LIMITS clockTick = null;
            // if we aren't updating a tick to none, but creating a new one, we'll skipp the DB2 update
            if (originalTick == null && timeLimitModel.TimeLimitTypeId.GetValueOrDefault() == (Int32) ClockTypes.None)
            {
                return null;
            }
            else
                if (originalTick != null && timeLimitModel.TimeLimitTypeId.GetValueOrDefault() == (Int32) ClockTypes.None || timeLimitModel.IsDeleted)
                {
                    // We are changing a tick to None, don't write it back as it, use the original and mark it as deleted with updated counts
                    clockTick = this.NewT0459_IN_W2_LIMITS(true);
                    originalTick.Copy(clockTick);
                    clockTick.Id = 0; // Make sure we INSERT (not UPDATE)
                }
                else
                {
                    var clockType = ((ClockTypes) timeLimitModel.TimeLimitTypeId.GetValueOrDefault());
                    clockTick               = this.NewT0459_IN_W2_LIMITS(true);
                    clockTick.CLOCK_TYPE_CD = (clockType == ClockTypes.TRIBAL ? ClockTypes.OTF : clockType).ToString();
                    clockTick.FED_CLOCK_IND = timeLimitModel.FederalTimeLimit.ToYn();
                }

            clockTick.BENEFIT_MM = benefitMmModel;
            clockTick.PIN_NUM    = participant.PinNumber.GetValueOrDefault();
            clockTick.OVERRIDE_REASON_CD = (ClockTypes) timeLimitModel.TimeLimitTypeId.GetValueOrDefault() == ClockTypes.CMC
                                           && timeLimitModel.StateTimelimit                                == false
                                           && timeLimitModel.FederalTimeLimit                              == true
                                               ? "FED"
                                               : (changeReasons.Value.Where(x => x.Id == timeLimitModel.ChangeReasonId).Select(x => x.Code).FirstOrDefault() ?? " ");
            clockTick.CRE_TRAN_CD     = Db2TimelimitService.TransCode;
            clockTick.HISTORY_SEQ_NUM = (Int16) (originalTick?.HISTORY_SEQ_NUM + 1).GetValueOrDefault(1);
            clockTick.HISTORY_CD      = 0;
            clockTick.COMMENT_TXT     = $"{clockTick.CLOCK_TYPE_CD} created by WPASS".ToUpper();
            clockTick.UPDATED_DT      = updatedDate              ?? DateTime.Now;
            clockTick.USER_ID         = mainFrameUser?.ToUpper() ?? " "; // We use mainframe ID instead of wams.

            this.UpdateT0459ModelCounts(clockTick, timeline, originalTick);

            // Before we add to list mark old record deleted.

            if (originalTick != null)
            {
                originalTick.HISTORY_CD = 9;
            }

            return clockTick;
        }


        public virtual List<IT0459_IN_W2_LIMITS> UpdateTicks0459(IEnumerable<IT0459_IN_W2_LIMITS> monthsToUpdate, ITimeline timeline, DateTime? updatedDate = null, Boolean retainDate = false, String commentText = null)
        {
            var newTicks = new List<IT0459_IN_W2_LIMITS>();

            var t0459InW2Limitss = monthsToUpdate?.ToList();
            if (t0459InW2Limitss == null)
            {
                return newTicks;
            }

            foreach (var tock in t0459InW2Limitss)
            {
                var newTick = this._doUpdateTicks0459(tock, timeline, retainDate, commentText, updatedDate);
                if (newTick != null)
                {
                    newTicks.Add(newTick);
                }
            }

            return newTicks;
        }

        private IT0459_IN_W2_LIMITS _doUpdateTicks0459(IT0459_IN_W2_LIMITS tock, ITimeline timeline, Boolean retainDate, String commentText, DateTime? updatedDate)
        {
            IT0459_IN_W2_LIMITS newTick = null;
            var                 tick    = (IT0459_IN_W2_LIMITS) tock.Clone();

            this.UpdateT0459ModelCounts(tick, timeline, tock);

            if (!tick.AreSemanticallyEqual(tock))
            {
                tick.Id = default(Int32);

                // attach the clone after we change the key of the original
                this.AttachAs(tick, EntityState.Added);


                tock.HISTORY_CD = 9;

                this.AttachAs(tock, EntityState.Modified);

                tick.CRE_TRAN_CD     = Db2TimelimitService.TransCode;
                tick.UPDATED_DT      = retainDate ? tock.UPDATED_DT : updatedDate ?? DateTime.Now;
                tick.COMMENT_TXT     = commentText.IsNullOrEmpty() ? tick.COMMENT_TXT + " " : (tick.COMMENT_TXT + commentText).Replace("  ", " ");
                tick.COMMENT_TXT     = tick.COMMENT_TXT.Substring(0, Math.Min(tick.COMMENT_TXT.Length, 75));
                tick.HISTORY_SEQ_NUM = (Int16) (tock.HISTORY_SEQ_NUM + 1);
                newTick              = tick;
            }
            else
            {
                this.Dettach(tock); //don't save anything
            }

            return newTick;
        }

        private void AttachAs(object obj, EntityState state)
        {
            if (!this.IsSimulated)
                this._repo.GetEntityEntry(obj).State = state;
        }

        private void Dettach(object obj)
        {
            if (!this.IsSimulated)
                this._repo.Dettach(obj);
        }


        // TODO: Unit test the crap outta this, move isLastestTick to Timeline class
        public virtual void UpdateT0459ModelCounts(IT0459_IN_W2_LIMITS clockTick, ITimeline timeline, IT0459_IN_W2_LIMITS originalTick)
        {
            ClockTypes clockType;
            if (!Enum.TryParse(clockTick.CLOCK_TYPE_CD, true, out clockType) || !clockType.HasAnyFlags(ClockTypes.CreateableTypes))
            {
                return;
            }

            // Normalize clockType for OTF/TEMP
            clockType = clockType.HasAnyFlags(ClockTypes.OTF) ? clockType.CombineFlags(ClockTypes.TRIBAL) : clockType;
            clockType = clockType.HasAnyFlags(ClockTypes.TEMP) ? clockType.CombineFlags(ClockTypes.TEMP) : clockType;

            var benefitDate           = DateTime.ParseExact(clockTick.BENEFIT_MM.ToString(CultureInfo.InvariantCulture), "yyyyMM", CultureInfo.InvariantCulture);
            var monthsWithClockType   = timeline.Months.Where(x => x.Value.ClockTypes.HasAnyFlags(clockType)).Select(x => x.Key);
            var lastMonthForClockType = (DateTime?) monthsWithClockType.GetMax(x => x.Date);
            var isLatestTick          = (Boolean) lastMonthForClockType?.IsSame(benefitDate, DateTimeUnit.Month);

            if (isLatestTick || originalTick == null)
            {
                clockType = clockType.CommonFlags(ClockTypes.CreateableTypes);
                var timelineDate = timeline.TimelineDate;

                // store the maxes/used for current date
                var currentStateMax = (Int16) timeline.GetMaxMonths(ClockTypes.State).GetValueOrDefault(0);

                // switch date to current month, we will only use currentStateMax on the latest tick. The rest of the counts are time/context aware
                timeline.TimelineDate = benefitDate;

                clockTick.FED_MAX_MTH_NUM = Db2TimelimitService.FedMaxMonthLimit;
                clockTick.FED_CMP_MTH_NUM = (Int16) timeline.GetUsedMonths(ClockTypes.Federal).GetValueOrDefault(0);

                clockTick.TOT_CMP_MTH_NUM = (Int16) timeline.GetUsedMonths(ClockTypes.State).GetValueOrDefault(0);
                clockTick.TOT_MAX_MTH_NUM = isLatestTick ? currentStateMax : (Int16) timeline.GetMaxMonths(ClockTypes.State).GetValueOrDefault(0);

                if (clockType.HasAnyFlags(ClockTypes.PlacementLimit | ClockTypes.TJB | ClockTypes.JOBS))
                {
                    clockTick.WW_CMP_MTH_NUM = (Int16) timeline.GetUsedMonths(clockType).GetValueOrDefault(0);
                    clockTick.WW_MAX_MTH_NUM = (Int16) timeline.GetMaxMonths(clockType).GetValueOrDefault(24);
                    clockTick.OT_CMP_MTH_NUM = 0;
                }
                else
                {
                    clockTick.WW_CMP_MTH_NUM = 0;
                    clockTick.WW_MAX_MTH_NUM = 0;
                    clockTick.OT_CMP_MTH_NUM = (Int16) timeline.GetUsedMonths(clockType).GetValueOrDefault(0);
                }

                timeline.TimelineDate = timelineDate;
            }
        }

        #endregion

        #region Extensions T0460 methods

        public IT0460_IN_W2_EXT Upsert(ITimeLimitExtension extensionModel, IParticipantInfo participant, String wamsId)
        {
            var mainFrameUserId = this._repo.WorkerByWamsId(wamsId)?.MFUserId;
            if (mainFrameUserId == null)
                throw new Exception("User not found.");

            var timeline  = this._timelimitService.GetTimeline(participant.PinNumber.GetValueOrDefault());
            var extension = TimelimitService.MapTimelimitExtensionToExtension(extensionModel);
            if (extensionModel.IsDeleted)
            {
                timeline.RemoveExtension(extensionModel.ExtensionSequence.GetValueOrDefault(), extension);
            }
            else
            {
                timeline.AddExtension(extensionModel.ExtensionSequence.GetValueOrDefault(1), extension);
            }

            var upsertedExt = this.InsertExtension(extensionModel, participant, mainFrameUserId, timeline);
            this.UpdateCountsFromExtensionUpsert(extensionModel, participant, timeline);
            this.Save();

            // Generate a notice
            this.InsertExtensionNotice(extensionModel, participant, timeline);

            return upsertedExt;
        }

        public virtual IT0460_IN_W2_EXT InsertExtension(ITimeLimitExtension extensionModel, IParticipantInfo participant, String mainFrameUser, ITimeline timeline)
        {
            // In order to insert correctly, we need the most current extension.

            var currentExtension = this._repo.GetW2ExtensionByClockType(participant.PinNumber.Value, (ClockTypes) extensionModel.TimeLimitTypeId.GetValueOrDefault(), extensionModel.ExtensionSequence.GetValueOrDefault(1));
            if (currentExtension != null)
            {
                currentExtension.HISTORY_CD = 9;
                currentExtension.UPDATED_DT = DateTime.Now;
            }


            var lastTimelimitMonth = timeline.Months[timeline.Months.Keys.Max()];

            var              clockType = ((ClockTypes) extensionModel.TimeLimitTypeId.GetValueOrDefault());
            IT0460_IN_W2_EXT extension = this.NewT0460InW2Ext(true);
            extension.PIN_NUM         = participant.PinNumber.Value;
            extension.CLOCK_TYPE_CD   = clockType == ClockTypes.State ? "60MO" : clockType.ToString().ToUpper();
            extension.AGY_DCSN_DT     = extensionModel.DecisionDate.GetValueOrDefault(HighDate);
            extension.EXT_REQ_PRC_DT  = DateTime.Now;
            extension.UPDATED_DT      = DateTime.Now;
            extension.BENEFIT_MM      = Convert.ToDecimal(lastTimelimitMonth.Date.ToStringMonthYearComposite());
            extension.EXT_SEQ_NUM     = (Int16) extensionModel.ExtensionSequence.GetValueOrDefault();
            extension.HISTORY_CD      = 0;
            extension.HISTORY_SEQ_NUM = currentExtension?.HISTORY_SEQ_NUM > 0 ? (Int16) (currentExtension.HISTORY_SEQ_NUM + 1) : (Int16) 1;
            extension.USER_ID         = mainFrameUser?.ToUpper() ?? " ";

            // Approval.
            if (extensionModel.ApprovalReasonId.HasValue)
            {
                var approvalReason     = this.approvalReasons.Value.FirstOrDefault(x => x.Id == extensionModel.ApprovalReasonId);
                var extensionDateRange = new DateTimeRange(extensionModel.BeginMonth.Value, extensionModel.EndMonth.Value);
                extension.AGY_DCSN_CD = "ERA";
                extension.EXT_BEG_MM  = Convert.ToDecimal(extensionModel.BeginMonth.ToStringMonthYearComposite());
                extension.EXT_END_MM  = Convert.ToDecimal(extensionModel.EndMonth.ToStringMonthYearComposite());
                extension.STA_DCSN_CD = (approvalReason.Code.Trim() + extensionDateRange.By(DateTimeUnits.Months).Count()).ToUpper();
            }
            else
                if (extensionModel.DenialReasonId.HasValue)
                {
                    var denialReason = this.denialReasons.Value.FirstOrDefault(x => x.Id == extensionModel.DenialReasonId);
                    extension.AGY_DCSN_CD = denialReason.Code.Trim().ToUpper();
                    extension.EXT_BEG_MM  = 999912m;
                    extension.EXT_END_MM  = 999912m;
                    extension.STA_DCSN_CD = " ";
                }

            if (extension.EXT_BEG_MM > extension.EXT_END_MM)
            {
                extension.DELETE_REASON_CD = "DE";
            }
            else
                if (extensionModel.IsDeleted)
                {
                    extension.DELETE_REASON_CD = "AE";
                }
                else
                {
                    extension.DELETE_REASON_CD = " ";
                }

            return extension;
        }

        public virtual void UpdateCountsFromExtensionUpsert(ITimeLimitExtension extensionModel, IParticipantInfo participant, ITimeline timeline)
        {
            List<IT0459_IN_W2_LIMITS> recordsToUpdate = new List<IT0459_IN_W2_LIMITS>();
            var                       clockType       = (ClockTypes) extensionModel.TimeLimitTypeId.GetValueOrDefault();
            if (clockType.HasFlag(ClockTypes.State))
            {
                recordsToUpdate = this._repo.GetLatestW2LimitsMonthsForEachClockType(participant.PinNumber.GetValueOrDefault());
            }
            else
            {
                var latestTickRecord = this._repo.GetLatestW2LimitsByClockType(participant.PinNumber.GetValueOrDefault(), clockType);
                if (latestTickRecord != null)
                {
                    recordsToUpdate.Add(latestTickRecord);
                }
            }

            this.UpdateTicks0459(recordsToUpdate, timeline);
        }

        public IT0754_LTR_RQST InsertExtensionNotice(ITimeLimitExtension extension, IParticipantInfo participant, ITimeline timeline)
        {
            var model = this.CreateExtensionNotice(extension, participant, timeline);

            this._repo.SpDB2_T0754_Insert(model);

            return model;
        }

        public IT0754_LTR_RQST CreateExtensionNotice(ITimeLimitExtension extension, IParticipantInfo participant, ITimeline timeline)
        {
            var wpGeoArea = this._repo.WpGeoAreaByPin(participant.PinNumber.GetValueOrDefault());
            var clockType = ((ClockTypes) extension?.TimeLimitTypeId.GetValueOrDefault());

            var extensionNotice = this.GetExtensionNotice(extension, participant, timeline, clockType);

            var noticeRecord = extensionNotice.CreateRecord();

            IT0754_LTR_RQST model = this.GetExtensionTriggerLetterRequest(timeline, participant, noticeRecord, wpGeoArea);

            return model;
        }

        public IT0754_LTR_RQST GetExtensionTriggerLetterRequest(ITimeline timeline, IParticipantInfo participant, ExtensionNoticeRecord noticeRecord, GeoArea wpGeoArea)
        {
            IT0754_LTR_RQST model;
            var             stringBuilder = new StringBuilder();
            var             tw            = new StringWriter(stringBuilder);

            foreach (var field in this._fixedLengthFileEngine.Options.Fields)
            {
                var fixedLengthField = field as FixedLengthField;
                if (fixedLengthField == null)
                {
                    continue;
                }
            }

            using (this._fixedLengthFileEngine.BeginWriteStream(tw))
            {
                this._fixedLengthFileEngine.WriteNext(noticeRecord);
            }

            var benefitMM = (timeline.GetTimelineMonths().GetMax(y => y.Date) ?? new TimelineMonth(HighDate)).Date;
            //var benefitMM = DateTime.ParseExact(upsertedExt.BENEFIT_MM.ToString(), "yyyyMM", CultureInfo.InvariantCulture);

            var caseNumberResult = this._repo.GetCARESCaseNumber(participant.PinNumber.ToString());

            model = new T0754_LTR_RQST(benefitMM, wpGeoArea, participant, caseNumberResult)
                    {
                        LTR_TXT = stringBuilder.ToString(),
                    };
            //TODO:trim the weird carriage return at the end, Dunno where that is from
            model.LTR_TXT.TrimEnd(Environment.NewLine.ToCharArray());
            return model;
        }


        public ExtensionNotice GetExtensionNotice(ITimeLimitExtension extension, IParticipantInfo participant, ITimeline timeline, ClockTypes clockType)
        {
            ExtensionNotice extensionNotice = null;

            if (extension.ExtensionDecisionId == (Int32) ExtensionDecision.Approve)
            {
                if (clockType == ClockTypes.State)
                {
                    extensionNotice = new StateApprovalExtensionNotice(participant.PinNumber?.ToString(), extension);
                }
                else
                {
                    extensionNotice = new PlacementApprovalExtensionNotice(participant.PinNumber?.ToString(), extension);
                }
            }
            else
            {
                var denialReason = this.denialReasons.Value.FirstOrDefault(x => x.Id == extension.DenialReasonId.GetValueOrDefault());

                var remainingMonths      = timeline.GetRemainingMonths(clockType).GetValueOrDefault();
                var isCurrentMonthTicked = timeline.GetTimelineMonths(clockType).OrderByDescending(i => i.Date).FirstOrDefault()?.IsCurrentMonth ?? false;
                var endDate              = DateTime.Now.StartOf(DateTimeUnit.Month).AddMonths(remainingMonths).EndOf(DateTimeUnit.Month);
                endDate = !isCurrentMonthTicked ? endDate.AddMonths(-1) : endDate;


                if (clockType == ClockTypes.State)
                {
                    extensionNotice = new StateDenialExtensionNotice(participant.PinNumber?.ToString(), extension, endDate, denialReason);
                }
                else
                {
                    extensionNotice = new PlacementDenialExtensionNotice(participant.PinNumber?.ToString(), extension, endDate, denialReason);
                }
            }

            return extensionNotice;
        }

        //public virtual Boolean IsT0460Unique(IT0460_IN_W2_EXT t0460)
        //{
        //    var ut0460 =
        //        _allExtensions.FirstOrDefault(
        //            x =>
        //                x.CLOCK_TYPE_CD == t0460.CLOCK_TYPE_CD && x.BENEFIT_MM == t0460.BENEFIT_MM &&
        //                x.HISTORY_SEQ_NUM == t0460.HISTORY_SEQ_NUM && x.EXT_SEQ_NUM == t0460.EXT_SEQ_NUM);

        //    if (ut0460 == null)
        //        return true;
        //    else
        //        return false;
        //}

        #endregion

        public IT0459_IN_W2_LIMITS NewT0459_IN_W2_LIMITS(Boolean attach)
        {
            return this._repo.NewT0459_IN_W2_LIMITS(attach && !IsSimulated);
        }

        public IT0460_IN_W2_EXT NewT0460InW2Ext(Boolean attach)
        {
            return this._repo.NewT0460InW2Ext(attach && !IsSimulated);
        }

        public void Save()
        {
            if (!this.IsSimulated)
                this._repo.Save();
            this._repo.ResetContext();
        }

        public Task SaveAsync(CancellationToken token = default (CancellationToken))
        {
            if (!this.IsSimulated)
                return this._repo.SaveAsync(token);
            this._repo.ResetContext();
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            ((IDisposable) this._fixedLengthFileEngine)?.Dispose();
            this._repo?.Dispose();
        }
    }
}
