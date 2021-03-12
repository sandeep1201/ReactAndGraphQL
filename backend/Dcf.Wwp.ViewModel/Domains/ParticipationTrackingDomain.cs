using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using DCF.Common;
using DCF.Common.Dates;
using DCF.Common.Exceptions;
using DCF.Common.Extensions;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.Api.Library.Extensions;
using Dcf.Wwp.Api.Library.Interfaces;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.Model.Interface.Core;
using Microsoft.Extensions.Caching.Memory;
using Dcf.Wwp.DataAccess.Models;
using Dcf.Wwp.Model.Interface.Constants;
using Dcf.Wwp.Model.Interface.Model;
using SmartFormat;
using AuxiliaryReason = Dcf.Wwp.Model.Interface.Constants.AuxiliaryReason;
using AuxiliaryStatusType = Dcf.Wwp.Model.Interface.Constants.AuxiliaryStatusType;
using EnrolledProgram = Dcf.Wwp.Model.Interface.Constants.EnrolledProgram;
using WorkerTaskStatus = Dcf.Wwp.Model.Interface.Constants.WorkerTaskStatus;

namespace Dcf.Wwp.Api.Library.Domains
{
    public class ParticipationTrackingDomain : IParticipationTrackingDomain
    {
        #region Properties

        private readonly IParticipantRepository               _participantRepository;
        private readonly IParticipationEntryRepository        _participationEntryRepository;
        private readonly ICFParticipationEntryRepository      _cfParticipationEntryRepository;
        private readonly IParticipationMakeUpEntryRepository  _makeUpEntryRepository;
        private readonly IPlacementTypeRepository             _placementTypeRepository;
        private readonly IParticipantPlacementRepository      _participantPlacementRepository;
        private readonly IPullDownDateRepository              _pullDownDateRepository;
        private readonly IOverPaymentRepository               _overPaymentRepository;
        private readonly IWorkerTaskListRepository            _workerTaskListRepository;
        private readonly IWorkerTaskCategoryRepository        _workerTaskCategoryRepository;
        private readonly IParticipantPaymentHistoryRepository _participantPaymentHistoryRepository;
        private readonly IOverUnderPaymentEmail               _overUnderPaymentEmail;
        private readonly IWorkerRepository                    _workerRepository;
        private readonly IParticipationEntryHistoryRepository _participationEntryHistoryRepository;
        private readonly IWorkerTaskStatusRepository          _workerTaskStatusRepository;
        private readonly IAuxiliaryRepository                 _auxiliaryRepository;
        private readonly IAuxiliaryStatusRepository           _auxiliaryStatusRepository;
        private readonly IAuxiliaryStatusTypeRepository       _auxiliaryStatusTypeRepository;
        private readonly IOfficeRepository                    _officeRepository;
        private readonly IAuxiliaryReasonRepository           _auxiliaryReasonRepository;
        private readonly IParticipationPeriodLookUpRepository _participationPeriodLookUpRepository;
        private readonly IDatabaseConfiguration               _dbConfig;
        private readonly IUnitOfWork                          _unitOfWork;
        private readonly IAuthUser                            _authUser;
        private readonly Func<string, string>                 _convertWIUIdToName;
        private readonly IMemoryCache                         _memoryCache;
        private readonly ITransactionDomain                   _transactionDomain;
        private          int                                  Cached                   { get; set; }
        private          List<OverUnderPaymentResult>         _overUnderPaymentResults { get; set; } = new List<OverUnderPaymentResult>();
        private          NewPayment                           NewPayment               { get; set; }
        private          List<OverUnderPayment>               _overUnderPayment        { get; set; } = new List<OverUnderPayment>();

        #endregion

        #region Methods

        public ParticipationTrackingDomain(IParticipantRepository               participantRepository,
                                           IParticipationEntryRepository        participationEntryRepository,
                                           ICFParticipationEntryRepository      cfParticipationEntryRepository,
                                           IParticipationMakeUpEntryRepository  makeUpEntryRepository,
                                           IPlacementTypeRepository             placementTypeRepository,
                                           IParticipantPlacementRepository      participantPlacementRepository,
                                           IPullDownDateRepository              pullDownDateRepository,
                                           IOverPaymentRepository               overPaymentRepository,
                                           IWorkerTaskListRepository            workerTaskListRepository,
                                           IWorkerTaskCategoryRepository        workerTaskCategoryRepository,
                                           IParticipantPaymentHistoryRepository participantPaymentHistoryRepository,
                                           IOverUnderPaymentEmail               overUnderPaymentEmail,
                                           IWorkerRepository                    workerRepository,
                                           IParticipationEntryHistoryRepository participationEntryHistoryRepository,
                                           IWorkerTaskStatusRepository          workerTaskStatusRepository,
                                           IAuxiliaryRepository                 auxiliaryRepository,
                                           IAuxiliaryStatusRepository           auxiliaryStatusRepository,
                                           IAuxiliaryStatusTypeRepository       auxiliaryStatusTypeRepository,
                                           IOfficeRepository                    officeRepository,
                                           IAuxiliaryReasonRepository           auxiliaryReasonRepository,
                                           IParticipationPeriodLookUpRepository participationPeriodLookUpRepository,
                                           IDatabaseConfiguration               dbConfig,
                                           IUnitOfWork                          unitOfWork,
                                           IAuthUser                            authUser,
                                           IMemoryCache                         memoryCache,
                                           ITransactionDomain                   transactionDomain)
        {
            _unitOfWork                          = unitOfWork;
            _authUser                            = authUser;
            _participantRepository               = participantRepository;
            _participationEntryRepository        = participationEntryRepository;
            _cfParticipationEntryRepository      = cfParticipationEntryRepository;
            _makeUpEntryRepository               = makeUpEntryRepository;
            _placementTypeRepository             = placementTypeRepository;
            _participantPlacementRepository      = participantPlacementRepository;
            _pullDownDateRepository              = pullDownDateRepository;
            _overPaymentRepository               = overPaymentRepository;
            _workerTaskListRepository            = workerTaskListRepository;
            _workerTaskCategoryRepository        = workerTaskCategoryRepository;
            _participantPaymentHistoryRepository = participantPaymentHistoryRepository;
            _overUnderPaymentEmail               = overUnderPaymentEmail;
            _workerRepository                    = workerRepository;
            _participationEntryHistoryRepository = participationEntryHistoryRepository;
            _workerTaskStatusRepository          = workerTaskStatusRepository;
            _auxiliaryRepository                 = auxiliaryRepository;
            _officeRepository                    = officeRepository;
            _auxiliaryReasonRepository           = auxiliaryReasonRepository;
            _auxiliaryStatusRepository           = auxiliaryStatusRepository;
            _auxiliaryStatusTypeRepository       = auxiliaryStatusTypeRepository;
            _participationPeriodLookUpRepository = participationPeriodLookUpRepository;
            _dbConfig                            = dbConfig;
            _memoryCache                         = memoryCache;
            _transactionDomain                   = transactionDomain;
            Cached                               = 0;

            _convertWIUIdToName = (wiuId) =>
                                  {
                                      string wn;
                                      if (wiuId == "WWP Batch") wn = "WWP Batch";
                                      else
                                      {
                                          var wo = workerRepository.GetAsQueryable()
                                                                   .Where(i => i.WIUId == wiuId)
                                                                   .Select(i => new { i.FirstName, i.MiddleInitial, i.LastName })
                                                                   .FirstOrDefault();

                                          wn = $"{wo?.FirstName} {wo?.MiddleInitial}. {wo?.LastName}".Replace(" . ", " ");
                                      }

                                      return (wn);
                                  };
        }

        public List<ParticipationTrackingContract> GetParticipationTrackingDetails(int participantId, string startDate, string endDate, bool isFromDetails = false, string programCode = null)
        {
            var startDateTime  = startDate.ToMonthDateYearTime();
            var endDateTime    = endDate.ToMonthDateYearTime();
            var trackingDates  = startDate.ToMonthDateYearTime().GetDates(endDateTime).ToList();
            var participantKey = $"participant-{participantId}";

            if (!_memoryCache.TryGetValue(participantKey, out List<ParticipantEnrolledProgram> peps))
            {
                peps = _participantRepository.GetAsQueryable()
                                             .Where(i => i.Id == participantId)
                                             .SelectMany(i => i.ParticipantEnrolledPrograms)
                                             .ToList();
                _memoryCache.Set(participantKey, peps, TimeSpan.FromMinutes(5));
            }
            else
            {
                Cached++;
            }

            var recentPep = peps?.OrderByDescending(i => i.ReferralDate)
                                .FirstOrDefault(i => i.EnrolledProgram.ProgramCode.Trim().ToLower() == (programCode ?? EnrolledProgram.W2ProgramCode));
            var isWorkerInRecentPepOrg = recentPep?.Office.ContractArea.Organization.EntsecAgencyCode == _authUser.AgencyCode;
            var pepDates = programCode?.ToLower() != EnrolledProgram.CFProgramCode.ToLower() && _authUser.AgencyCode != AgencyCode.DCF
                               ? peps?.Where(i => i.Office?.ContractArea?.Organization?.EntsecAgencyCode == _authUser.AgencyCode)
                                     .Select(i => new { i.EnrollmentDate, i.DisenrollmentDate }).ToList()
                               : peps?.Select(i => new { i.EnrollmentDate, i.DisenrollmentDate }).ToList();
            var enrollmentPeriod = new List<DateTime>();

            pepDates?.ForEach(i =>
                              {
                                  if (i.EnrollmentDate == null) return;
                                  enrollmentPeriod.AddRange(i.EnrollmentDate.GetDates(i.DisenrollmentDate
                                                                                      ?? (endDateTime > i.EnrollmentDate ? endDateTime : i.EnrollmentDate)));
                              });

            var isInPeriod       = trackingDates.Intersect(enrollmentPeriod).Any();
            var trackingContract = new List<ParticipationTrackingContract>();

            if (isWorkerInRecentPepOrg || isInPeriod)
                trackingContract = PrepareContract(programCode, isFromDetails, participantId, startDateTime, endDateTime, enrollmentPeriod);

            return trackingContract;
        }

        public ParticipationTrackingContract UpsertParticipationTrackingDetails(ParticipationTrackingContract participationTrackingContract, string programCode)
        {
            if (participationTrackingContract == null)
            {
                throw new ArgumentNullException(nameof(participationTrackingContract));
            }

            var modifiedBy   = _authUser.WIUID;
            var modifiedDate = DateTime.Now;

            if (!programCode.IsNullOrWhiteSpace() && programCode.ToLower() == "cf")
            {
                var cfParticipationEntry = participationTrackingContract.Id != 0
                                               ? _cfParticipationEntryRepository.Get(c => c.Id == participationTrackingContract.Id && !c.IsDeleted)
                                               : _cfParticipationEntryRepository.New();

                cfParticipationEntry.ActivityId                    = participationTrackingContract.ActivityId;
                cfParticipationEntry.ParticipationDate             = participationTrackingContract.ParticipationDate;
                cfParticipationEntry.ScheduledHours                = Convert.ToDecimal(participationTrackingContract.ScheduledHours);
                cfParticipationEntry.DidParticipate                = participationTrackingContract.DidParticipate;
                cfParticipationEntry.NonParticipationReasonId      = participationTrackingContract.NonParticipationReasonId;
                cfParticipationEntry.NonParticipationReasonDetails = participationTrackingContract.NonParticipationReasonDetails;
                cfParticipationEntry.GoodCauseGranted              = participationTrackingContract.GoodCauseGranted;
                cfParticipationEntry.GoodCauseDeniedReasonId       = participationTrackingContract.GoodCauseDeniedReasonId;
                cfParticipationEntry.GoodCauseReasonDetails        = participationTrackingContract.GoodCauseReasonDetails;
                cfParticipationEntry.GoodCauseGrantedReasonId      = participationTrackingContract.GoodCauseGrantedReasonId;
                cfParticipationEntry.ModifiedDate                  = modifiedDate;
                cfParticipationEntry.ModifiedBy                    = modifiedBy;

                if (participationTrackingContract.Id == 0)
                    _cfParticipationEntryRepository.Add(cfParticipationEntry);

                _unitOfWork.Commit();
            }
            else
            {
                var participationEntry = participationTrackingContract.Id != 0
                                             ? _participationEntryRepository.Get(c => c.Id == participationTrackingContract.Id && !c.IsDeleted)
                                             : _participationEntryRepository.New();

                participationEntry.ActivityId                    = participationTrackingContract.ActivityId;
                participationEntry.ParticipationDate             = participationTrackingContract.ParticipationDate;
                participationEntry.ScheduledHours                = Convert.ToDecimal(participationTrackingContract.ScheduledHours);
                participationEntry.NonParticipationReasonId      = participationTrackingContract.NonParticipationReasonId;
                participationEntry.NonParticipationReasonDetails = participationTrackingContract.NonParticipationReasonDetails;
                participationEntry.GoodCauseGranted              = participationTrackingContract.GoodCauseGranted;
                participationEntry.GoodCauseDeniedReasonId       = participationTrackingContract.GoodCauseDeniedReasonId;
                participationEntry.GoodCauseReasonDetails        = participationTrackingContract.GoodCauseReasonDetails;
                participationEntry.GoodCauseGrantedReasonId      = participationTrackingContract.GoodCauseGrantedReasonId;
                participationEntry.GoodCausedHours               = participationTrackingContract.GoodCausedHours.ToDecimalNull();
                participationEntry.ReportedHours                 = participationTrackingContract.ReportedHours.ToDecimalNull();
                participationEntry.TotalMakeupHours              = participationTrackingContract.TotalMakeupHours.ToDecimalNull();
                participationEntry.ParticipatedHours             = participationTrackingContract.ParticipatedHours.ToDecimalNull();
                participationEntry.NonParticipatedHours          = participationTrackingContract.NonParticipatedHours.ToDecimalNull();
                participationEntry.IsProcessed                   = false;
                participationEntry.ModifiedDate                  = modifiedDate;
                participationEntry.ModifiedBy                    = modifiedBy;

                var dbMakeUpEntryIds       = _makeUpEntryRepository.GetMany(i => i.ParticipationEntryId == participationEntry.Id).Select(i => i.Id).ToList();
                var contractMakeUpEntryIds = participationTrackingContract.MakeUpEntries?.Select(i => i.Id).ToList();
                var deletedIds             = contractMakeUpEntryIds != null ? dbMakeUpEntryIds.Except(contractMakeUpEntryIds).ToList() : dbMakeUpEntryIds.ToList();

                deletedIds.ForEach(i => _makeUpEntryRepository.Delete(makeUpEntry => makeUpEntry.Id == i));

                participationTrackingContract.MakeUpEntries.AsNotNull().ForEach(makeupEntry =>
                                                                                {
                                                                                    var entry = _makeUpEntryRepository.Get(i => i.Id == makeupEntry.Id && i.ParticipationEntryId == participationTrackingContract.Id) ?? _makeUpEntryRepository.New();

                                                                                    entry.Id                   = makeupEntry.Id;
                                                                                    entry.ParticipationEntryId = participationTrackingContract.Id;
                                                                                    entry.MakeupDate           = makeupEntry.MakeupDate;
                                                                                    entry.MakeupHours          = Convert.ToDecimal(makeupEntry.MakeupHours);
                                                                                    entry.ModifiedBy           = modifiedBy;
                                                                                    entry.ModifiedDate         = modifiedDate;

                                                                                    if (makeupEntry.Id == 0)
                                                                                        participationEntry.ParticipationMakeUpEntries.Add(entry);
                                                                                });

                if (participationTrackingContract.Id == 0)
                    _participationEntryRepository.Add(participationEntry);
                var participationList = new List<ParticipationEntry>
                                        {
                                            participationEntry
                                        };

                PTTransactionalSave(modifiedBy, modifiedDate, participationList);
            }

            return participationTrackingContract;
        }

        public bool DeleteParticipationTrackingDetails(int id)
        {
            var participationEntry = _participationEntryRepository.Get(c => c.Id == id && !c.IsDeleted);

            if (participationEntry == null)
            {
                throw new InvalidOperationException("Participation Entry is null.");
            }

            if (participationEntry.IsDeleted) return false;

            var modifiedBy   = _authUser.WIUID;
            var modifiedDate = DateTime.Now;

            participationEntry.ReportedHours                 = null;
            participationEntry.TotalMakeupHours              = null;
            participationEntry.ParticipatedHours             = null;
            participationEntry.NonParticipatedHours          = null;
            participationEntry.NonParticipationReasonId      = null;
            participationEntry.NonParticipationReasonDetails = null;
            participationEntry.GoodCauseGranted              = null;
            participationEntry.GoodCauseDeniedReasonId       = null;
            participationEntry.GoodCauseReasonDetails        = null;
            participationEntry.GoodCauseGrantedReasonId      = null;
            participationEntry.GoodCausedHours               = null;
            participationEntry.ModifiedDate                  = modifiedDate;
            participationEntry.ModifiedBy                    = modifiedBy;

            _makeUpEntryRepository.DeleteRange(participationEntry.ParticipationMakeUpEntries);

            var participationList = new List<ParticipationEntry>
                                    {
                                        participationEntry
                                    };

            PTTransactionalSave(modifiedBy, modifiedDate, participationList);
            return true;
        }

        public List<ParticipationTrackingContract> MakeFullOrNoParticipation(int participationId, string makeFullOrNoParticipation, string startDate, string endDate, List<ParticipationTrackingContract> participationTrackingContracts, string programCode = null)
        {
            var modifiedBy   = _authUser.WIUID;
            var modifiedDate = DateTime.Now;

            var peIds = participationTrackingContracts.Select(i => i.Id);

            if (programCode?.Trim().ToLower() != "cf")
            {
                var pes = _participationEntryRepository.GetMany(i => peIds.Contains(i.Id) && !i.IsDeleted).ToList();

                pes.ForEach(i =>
                            {
                                var pe = participationTrackingContracts.FirstOrDefault(j => j.Id == i.Id);

                                if (makeFullOrNoParticipation == "fullparticipation")
                                {
                                    i.ReportedHours        = pe?.ScheduledHours.ToDecimalNull();
                                    i.ParticipatedHours    = pe?.ScheduledHours.ToDecimalNull();
                                    i.NonParticipatedHours = "0.0".ToDecimal();
                                }
                                else
                                {
                                    i.ReportedHours        = "0.0".ToDecimal();
                                    i.ParticipatedHours    = "0.0".ToDecimal();
                                    i.NonParticipatedHours = pe?.ScheduledHours.ToDecimalNull();
                                }

                                i.ModifiedBy   = modifiedBy;
                                i.ModifiedDate = modifiedDate;
                            });

                PTTransactionalSave(modifiedBy, modifiedDate, pes);
            }
            else
            {
                var cfPes = _cfParticipationEntryRepository.GetMany(i => peIds.Contains(i.Id) && !i.IsDeleted);

                cfPes.ForEach(i =>
                              {
                                  i.DidParticipate = makeFullOrNoParticipation == "fullparticipation";
                                  i.ModifiedBy     = modifiedBy;
                                  i.ModifiedDate   = modifiedDate;
                              });

                _unitOfWork.Commit();
            }

            return GetParticipationTrackingDetails(participationId, startDate, endDate, false, programCode);
        }

        private List<ParticipationTrackingContract> PrepareContract(string programCode, bool isFromDetails, int participantId, DateTime? startDateTime, DateTime? endDateTime, List<DateTime> enrollmentPeriod)
        {
            return programCode?.ToLower() != "cf"
                       ? PrepareTrackingContract(isFromDetails, participantId, startDateTime, endDateTime, enrollmentPeriod)
                       : PrepareCFTrackingContract(isFromDetails, participantId, startDateTime, endDateTime);
        }

        private List<ParticipationTrackingContract> PrepareTrackingContract(bool isFromDetails, int participantId, DateTime? startDateTime, DateTime? endDateTime, IReadOnlyCollection<DateTime> enrollmentPeriod)
        {
            var today            = _authUser.CDODate ?? DateTime.Today;
            var trackingContract = new List<ParticipationTrackingContract>();

            var trackingEntries = isFromDetails
                                      ? _participationEntryRepository.GetAsQueryable()
                                                                     .Where(i => i.ParticipantId        == participantId
                                                                                 && i.ParticipationDate >= startDateTime && i.ParticipationDate    <= endDateTime
                                                                                 && !i.IsDeleted                         && i.NonParticipatedHours > 0)
                                                                     .ToList()
                                      : _participationEntryRepository.GetAsQueryable()
                                                                     .Where(i => i.ParticipantId        == participantId
                                                                                 && i.ParticipationDate >= startDateTime && i.ParticipationDate <= endDateTime
                                                                                 && !i.IsDeleted)
                                                                     .ToList();

            trackingEntries.ForEach(i =>
                                    {
                                        var lastNonBatchModified = _participationEntryHistoryRepository.GetMany(j => j.Id == i.Id && j.ModifiedBy != "WWP Batch")
                                                                                                       ?.OrderByDescending(j => j.ModifiedDate)
                                                                                                       .FirstOrDefault();
                                        var contract = new ParticipationTrackingContract
                                                       {
                                                           Id                            = i.Id,
                                                           EPId                          = i.EPId,
                                                           ActivityId                    = i.ActivityId,
                                                           ActivityCd                    = i.Activity.ActivityType?.Code,
                                                           ActivityName                  = i.Activity.ActivityType?.Name,
                                                           ParticipationDate             = i.ParticipationDate,
                                                           ScheduledHours                = i.ScheduledHours.ToString(CultureInfo.InvariantCulture),
                                                           ReportedHours                 = i.ReportedHours?.ToString(),
                                                           TotalMakeupHours              = i.TotalMakeupHours?.ToString(),
                                                           ParticipatedHours             = i.ParticipatedHours?.ToString(),
                                                           NonParticipatedHours          = i.NonParticipatedHours?.ToString(),
                                                           GoodCausedHours               = i.GoodCausedHours?.ToString(),
                                                           NonParticipationReasonId      = i.NonParticipationReasonId,
                                                           NonParticipationReasonName    = i.NonParticipationReason?.Name,
                                                           NonParticipationReasonCd      = i.NonParticipationReason?.Code,
                                                           NonParticipationReasonDetails = i.NonParticipationReasonDetails,
                                                           GoodCauseGranted              = i.GoodCauseGranted,
                                                           GoodCauseGrantedReasonId      = i.GoodCauseGrantedReasonId,
                                                           GoodCauseGrantedReasonName    = i.GoodCauseGrantedReason?.Name,
                                                           GoodCauseDeniedReasonId       = i.GoodCauseDeniedReasonId,
                                                           GoodCauseDeniedReasonName     = i.GoodCauseDeniedReason?.Name,
                                                           GoodCauseReasonCd             = i.GoodCauseGrantedReason?.Code ?? i.GoodCauseDeniedReason?.Code,
                                                           GoodCauseReasonDetails        = i.GoodCauseReasonDetails,
                                                           PlacementTypeId               = i.PlacementTypeId,
                                                           PlacementTypeName             = i.PlacementType?.Name,
                                                           PlacementTypeCd               = i.PlacementType?.Code,
                                                           FormalAssessmentExists        = i.FormalAssessmentExists,
                                                           HoursSanctionable             = i.HoursSanctionable,
                                                           MakeUpEntries = i.ParticipationMakeUpEntries.Where(j => !j.IsDeleted).Select(j => new ParticipationMakeUpEntryContract
                                                                                                                                             {
                                                                                                                                                 Id                   = j.Id,
                                                                                                                                                 ParticipationEntryId = j.ParticipationEntryId,
                                                                                                                                                 MakeupDate           = j.MakeupDate,
                                                                                                                                                 MakeupHours          = j.MakeupHours.ToString(CultureInfo.InvariantCulture)
                                                                                                                                             }).ToList(),
                                                           ModifiedBy         = _convertWIUIdToName(lastNonBatchModified?.ModifiedBy ?? "WWP Batch"),
                                                           ModifiedDate       = lastNonBatchModified?.ModifiedDate ?? i.ModifiedDate,
                                                           CanEditBasedOnDate = i.ParticipationDate <= today,
                                                           IsProcessed        = i.IsProcessed,
                                                           ProcessedDate      = i.ProcessedDate?.ToString("MM/dd/yyyy"),
                                                           CanEditBasedOnOrg  = enrollmentPeriod.Any(j => j == i.ParticipationDate)
                                                       };

                                        trackingContract.Add(contract);
                                    });

            return trackingContract;
        }

        private List<ParticipationTrackingContract> PrepareCFTrackingContract(bool isFromDetails, int participantId, DateTime? startDateTime, DateTime? endDateTime)
        {
            var trackingContract = new List<ParticipationTrackingContract>();

            var cfTrackingEntries = isFromDetails
                                        ? _cfParticipationEntryRepository.GetAsQueryable()
                                                                         .Where(i => i.ParticipantId        == participantId
                                                                                     && i.ParticipationDate >= startDateTime && i.ParticipationDate <= endDateTime
                                                                                     && !i.IsDeleted                         && i.DidParticipate    == false)
                                                                         .ToList()
                                        : _cfParticipationEntryRepository.GetAsQueryable()
                                                                         .Where(i => i.ParticipantId        == participantId
                                                                                     && i.ParticipationDate >= startDateTime && i.ParticipationDate <= endDateTime
                                                                                     && !i.IsDeleted)
                                                                         .ToList();

            cfTrackingEntries.ForEach(i =>
                                      {
                                          var contract = new ParticipationTrackingContract
                                                         {
                                                             Id                            = i.Id,
                                                             EPId                          = i.EPId,
                                                             ActivityId                    = i.ActivityId,
                                                             ActivityCd                    = i.Activity.ActivityType?.Code,
                                                             ActivityName                  = i.Activity.ActivityType?.Name,
                                                             ParticipationDate             = i.ParticipationDate,
                                                             ScheduledHours                = i.ScheduledHours.ToString(CultureInfo.InvariantCulture),
                                                             DidParticipate                = i.DidParticipate,
                                                             NonParticipationReasonId      = i.NonParticipationReasonId,
                                                             NonParticipationReasonName    = i.NonParticipationReason?.Name,
                                                             NonParticipationReasonDetails = i.NonParticipationReasonDetails,
                                                             GoodCauseGranted              = i.GoodCauseGranted,
                                                             GoodCauseGrantedReasonId      = i.GoodCauseGrantedReasonId,
                                                             GoodCauseGrantedReasonName    = i.GoodCauseGrantedReason?.Name,
                                                             GoodCauseDeniedReasonId       = i.GoodCauseDeniedReasonId,
                                                             GoodCauseDeniedReasonName     = i.GoodCauseDeniedReason?.Name,
                                                             GoodCauseReasonDetails        = i.GoodCauseReasonDetails,
                                                             ModifiedBy                    = _convertWIUIdToName(i.ModifiedBy),
                                                             ModifiedDate                  = i.ModifiedDate,
                                                             CanEditBasedOnDate            = i.ParticipationDate <= DateTime.Today
                                                         };

                                          trackingContract.Add(contract);
                                      });

            return trackingContract;
        }

        private CommitStatus PTTransactionalSave(string modifiedBy, DateTime modifiedDate, IEnumerable<ParticipationEntry> participationEntries, bool returnStatus = false)
        {
            var commitStatus = new CommitStatus();

            using (var tx = _participationEntryRepository.GetDataBase().BeginTransaction())
            {
                try
                {
                    participationEntries = participationEntries.ToList();

                    var xml = new XElement("ParticipationEntries", participationEntries.Select(i => new XElement("ParticipationEntry", new XElement("ParticipantId", i.ParticipantId),
                                                                                                                 new XElement("ParticipationEntryId",                i.Id),
                                                                                                                 new XElement("ParticipationDate",                   i.ParticipationDate),
                                                                                                                 new XElement("EPId",                                i.EPId),
                                                                                                                 new XElement("ActivityId",                          i.ActivityId),
                                                                                                                 new XElement("PlacementTypeId",                     i.PlacementTypeId),
                                                                                                                 new XElement("NonParticipatedHours",                i.NonParticipatedHours - (i.GoodCausedHours ?? 0) ?? 0)))).ToString();


                    var parms = new Dictionary<string, object>
                                {
                                    ["ParticipationEntryIdXML"] = xml,
                                    ["ModifiedBy"]              = modifiedBy,
                                    ["ModifiedDate"]            = modifiedDate,
                                    ["Debug"]                   = true
                                };

                    var rs = _participationEntryRepository.ExecStoredProc<SanctionableSPResult>("USP_IsNonParticipationSanctionable", parms);

                    participationEntries.ForEach(i =>
                                                 {
                                                     var result = rs.FirstOrDefault(j => j.ParticipationEntryId == i.Id);

                                                     i.FormalAssessmentExists = result?.FormalAssessmentExists;
                                                     i.HoursSanctionable      = result?.HoursSanctionable;
                                                 });

                    if (returnStatus)
                        commitStatus = _unitOfWork.CommitWithStatus();
                    else
                        _unitOfWork.Commit();

                    var sprocRs          = false;
                    var hasPHSprocCalled = false;

                    if (_overUnderPayment.Count > 0)
                    {
                        CallIssueOverPaymentOrAuxiliary(modifiedBy, modifiedDate);

                        commitStatus = _unitOfWork.CommitWithStatus();

                        if (NewPayment != null)
                        {
                            hasPHSprocCalled = true;
                            sprocRs          = WriteNewPaymentToDB2();
                        }
                    }

                    if (hasPHSprocCalled && sprocRs)
                        throw new DCFApplicationException("Failed due to SProc issue. Please try again.");

                    tx.Commit();
                }
                catch (Exception ex)
                {
                    tx.Dispose();
                    throw new DCFApplicationException("Failed due to SProc issue. Please try again.", ex);
                }

                tx.Dispose();
            }

            return returnStatus ? commitStatus : null;
        }

        public CommitStatus UpdatePlacement(List<UpdatePlacement> updatePlacements, string modifiedBy)
        {
            updatePlacements.ForEach(i =>
                                     {
                                         i.CaseNumber = decimal.Truncate(i.CaseNumber);
                                         i.PinNumber  = decimal.Truncate(i.PinNumber);
                                     });

            var modifiedDate             = DateTime.Now;
            var pins                     = updatePlacements.Select(j => (decimal?) j.PinNumber).ToList();
            var participationEntries     = _participationEntryRepository.GetMany(i => pins.Contains(i.Participant.PinNumber) && !i.IsDeleted).ToList();
            var participationEntriesList = new List<ParticipationEntry>();
            var placementContract        = updatePlacements.Select(i => i.PlacementType);
            var placementTypes           = _placementTypeRepository.GetMany(i => placementContract.Contains(i.DB2Code)).ToList();

            ChangesDueToBackDatedPlacement(updatePlacements, placementTypes, modifiedBy, modifiedDate);

            participationEntries.ForEach(i =>
                                         {
                                             var updatePlacement = updatePlacements.FirstOrDefault(j => j.PinNumber == i.Participant.PinNumber);

                                             if (updatePlacement?.PlacementEndDate != null)
                                             {
                                                 if (i.ParticipationDate <= updatePlacement.PlacementEndDate) return;
                                                 {
                                                     UpdateEntry(i, updatePlacement, placementTypes, modifiedBy, modifiedDate);
                                                     participationEntriesList.Add(i);
                                                 }
                                             }
                                             else
                                             {
                                                 if (i.ParticipationDate < updatePlacement?.PlacementStartDate) return;
                                                 {
                                                     UpdateEntry(i, updatePlacement, placementTypes, modifiedBy, modifiedDate);
                                                     participationEntriesList.Add(i);
                                                 }
                                             }
                                         });

            return PTTransactionalSave(modifiedBy, modifiedDate, participationEntriesList, true);
        }

        private void ChangesDueToBackDatedPlacement(List<UpdatePlacement> updatePlacements, IEnumerable<PlacementType> placementTypes, string modifiedBy, DateTime modifiedDate)
        {
            var today = _authUser.CDODate ?? DateTime.Today;

            updatePlacements.ForEach(i =>
                                     {
                                         var placementEndDate = i.PlacementEndDate == DateTime.MaxValue.Date ? null : i.PlacementEndDate;
                                         var placementDate    = placementEndDate ?? i.PlacementStartDate;
                                         var delayedCycleDt = _pullDownDateRepository.Get(j => j.BenefitMonth == placementDate.Month && j.BenefitYear == placementDate.Year)
                                                                                     .DelayedCycleDate;
                                         var participant          = _participantRepository.Get(j => j.PinNumber == i.PinNumber) ?? CreateParticipant(i.PinNumber);
                                         var currentPlacementType = placementTypes.First(j => j.DB2Code   == i.PlacementType);
                                         var workerId             = _workerRepository.Get(j => j.MFUserId == i.MFFEPId)?.Id;
                                         var lastParticipantPlacement = _participantPlacementRepository.GetMany(j => j.ParticipantId == participant.Id && !j.IsDeleted)
                                                                                                       .OrderByDescending(j => j.PlacementStartDate)
                                                                                                       .FirstOrDefault();
                                         var participationBeginDate = today.SpecificDayOfMonth(16, "previous").Date;
                                         var participationEndDate   = today.SpecificDayOfMonth(15).Date;
                                         var delayedPayment = _participantPaymentHistoryRepository.Get(j => j.CaseNumber           == i.CaseNumber         && j.ParticipationBeginDate   >= participationBeginDate &&
                                                                                                            j.ParticipationEndDate <= participationEndDate && j.ParticipationEndDate.Day != 15);

                                         if (lastParticipantPlacement != null)
                                         {
                                             if (lastParticipantPlacement.PlacementStartDate != i.PlacementStartDate)
                                             {
                                                 UpsertParticipantPlacement(participant, i, currentPlacementType.Id, modifiedBy, modifiedDate, workerId);

                                                 if (lastParticipantPlacement.PlacementEndDate == null)
                                                     UpdateLastParticipantPlacement(lastParticipantPlacement, placementEndDate ?? i.PlacementStartDate.AddDays(-1), modifiedBy, modifiedDate);
                                             }
                                             else
                                             {
                                                 UpsertParticipantPlacement(participant, i, currentPlacementType.Id, modifiedBy, modifiedDate, workerId, lastParticipantPlacement);
                                                 UpdateLastParticipantPlacement(lastParticipantPlacement, placementEndDate, modifiedBy, modifiedDate);
                                             }

                                             if (today.IsAfter(delayedCycleDt) && placementDate.IsBefore(today.FirstDayOfMonth()) && delayedPayment != null)
                                                 _overUnderPayment.Add(new OverUnderPayment
                                                                       {
                                                                           CaseNumber             = i.CaseNumber,
                                                                           Today                  = today,
                                                                           ParticipationBeginDate = participationBeginDate,
                                                                           ParticipationEndDate   = participationEndDate,
                                                                           DelayedPayment         = delayedPayment,
                                                                           PlacementEntry         = i,
                                                                           LastPlacementEntry     = lastParticipantPlacement,
                                                                           Participant            = participant,
                                                                           WorkerId               = workerId
                                                                       });
                                         }
                                         else
                                             UpsertParticipantPlacement(participant, i, currentPlacementType.Id, modifiedBy, modifiedDate, workerId);
                                     });
        }

        private Participant CreateParticipant(decimal pin)
        {
            var parms = new Dictionary<string, object>
                        {
                            ["PinNumber"] = pin.ToString(CultureInfo.InvariantCulture),
                            ["Debug"]     = true
                        };

            return _participantRepository.ExecStoredProc<Participant>("USP_RefreshParticipant", parms).FirstOrDefault();
        }

        private void UpsertParticipantPlacement(Participant participant, UpdatePlacement placementContract, int placementTypeId, string modifiedBy, DateTime modifiedDate, int? workerId, ParticipantPlacement participantPlacement = null)
        {
            participantPlacement = participantPlacement ?? _participantPlacementRepository.New();

            participantPlacement.ParticipantId      = participant.Id;
            participantPlacement.CaseNumber         = placementContract.CaseNumber;
            participantPlacement.PlacementTypeId    = placementTypeId;
            participantPlacement.PlacementStartDate = placementContract.PlacementStartDate;
            participantPlacement.CreatedDate        = modifiedDate;
            participantPlacement.ModifiedBy         = modifiedBy;
            participantPlacement.ModifiedDate       = modifiedDate;

            if (participantPlacement.Id == 0)
                _participantPlacementRepository.Add(participantPlacement);

            InsertTransaction(participant, workerId, placementContract, modifiedBy, modifiedDate);
            InsertWorkerTask(participant, placementContract, modifiedBy, modifiedDate, WorkerTaskCategoryCodes.PlacementChangeIACode);
        }

        private void UpdateLastParticipantPlacement(ParticipantPlacement lastParticipantPlacement, DateTime? placementEndDate, string modifiedBy, DateTime modifiedDate)
        {
            lastParticipantPlacement.PlacementEndDate = placementEndDate;
            lastParticipantPlacement.ModifiedBy       = modifiedBy;
            lastParticipantPlacement.ModifiedDate     = modifiedDate;
        }

        public TransactionContract InsertTransaction(Participant participant, int? workerId, UpdatePlacement placementContract, string modifiedBy, DateTime modifiedDate)
        {
            var transactionContract = new TransactionContract
                                      {
                                          ParticipantId       = participant.Id,
                                          WorkerId            = workerId,
                                          EffectiveDate       = placementContract.PlacementEndDate ?? placementContract.PlacementStartDate,
                                          CreatedDate         = modifiedDate,
                                          TransactionTypeCode = TransactionTypes.Placement,
                                          ModifiedBy          = modifiedBy
                                      };
            var pep = participant.ParticipantEnrolledPrograms;

            if (pep != null && pep.Count > 0)
            {
                transactionContract.OfficeId = pep.OrderByDescending(i => i.ReferralDate).First().OfficeId.GetValueOrDefault();

                _transactionDomain.InsertTransaction(transactionContract);
            }

            return transactionContract;
        }

        public void InsertWorkerTask(Participant participant, UpdatePlacement placementContract, string modifiedBy, DateTime modifiedDate, string code, decimal? revisedPaymentAmount = null, DateTime? participationBeginDate = null, DateTime? participationEndDate = null)
        {
            var pep = participant.ParticipantEnrolledPrograms?.FirstOrDefault(i => i.IsW2 && i.IsEnrolled);

            if ((pep != null && participant.InformalAssessments != null && pep.WorkerId.HasValue &&
                 !participant.InformalAssessments.Any(i => i.EndDate.HasValue && i.EndDate.GetValueOrDefault().DateDiff(placementContract.PlacementStartDate) < 10)) ||
                (pep != null && code == WorkerTaskCategoryCodes.BackDatedPlacementDelayedCycleCode))
            {
                var workerTaskCategory = _workerTaskCategoryRepository.Get(i => i.Code == code);
                var workerTaskList = new WorkerTaskList
                                     {
                                         WorkerTaskStatusId = _workerTaskStatusRepository.Get(i => i.Code == WorkerTaskStatus.Open).Id,
                                         CategoryId         = workerTaskCategory.Id,
                                         TaskDetails = revisedPaymentAmount == null
                                                           ? workerTaskCategory.Description
                                                           : Smart.Format(workerTaskCategory.Description,
                                                                          $"{(revisedPaymentAmount < 0 ? "auxiliary" : "overpayment")}",
                                                                          $"{participationBeginDate?.ToMonthName()} {participationBeginDate?.Day}",
                                                                          $"{participationEndDate?.ToMonthName()} {participationEndDate?.Day}",
                                                                          $"{participationEndDate?.Year}",
                                                                          $"{Math.Abs(revisedPaymentAmount.GetValueOrDefault())}"
                                                                         ),
                                         WorkerId          = pep.WorkerId.GetValueOrDefault(),
                                         ParticipantId     = participant.Id,
                                         TaskDate          = modifiedDate,
                                         StatusDate        = _authUser.CDODate ?? modifiedDate,
                                         IsSystemGenerated = true,
                                         ModifiedBy        = modifiedBy,
                                         ModifiedDate      = modifiedDate
                                     };

                _workerTaskListRepository.Add(workerTaskList);
            }
        }

        private void CallIssueOverPaymentOrAuxiliary(string modifiedBy, DateTime modifiedDate)
        {
            _overUnderPayment.ForEach(i =>
                                      {
                                          var parms = new Dictionary<string, object>
                                                      {
                                                          ["CaseNumber"]             = i.CaseNumber,
                                                          ["ParticipationBeginDate"] = i.ParticipationBeginDate,
                                                          ["ParticipationEndDate"]   = i.ParticipationBeginDate.EndOf(DateTimeUnit.Month)
                                                      };

                                          var paymentChange = _participantPlacementRepository.ExecStoredProc<USP_GetPaymentAmount_ByLastPaidPlacement>("USP_GetPaymentAmount_ByLastPaidPlacement", parms)
                                                                                             .FirstOrDefault();
                                          var newBasePayment = paymentChange == null ? 0.00m : Math.Floor(paymentChange.NewBaseW2Payment);
                                          var dfPenaltyPct   = paymentChange?.DFPenaltyPct ?? 0.0m;

                                          IssueOverPaymentOrAuxiliary(i.DelayedPayment, i.Today, i.PlacementEntry, i.Participant, modifiedBy, modifiedDate, newBasePayment, dfPenaltyPct, i.ParticipationBeginDate, i.ParticipationEndDate, i.WorkerId);
                                      });

            if (_overUnderPaymentResults.Count > 0)
                _overUnderPaymentEmail.SendEmail(_dbConfig.Catalog, _overUnderPaymentResults);
        }

        private bool WriteNewPaymentToDB2()
        {
            var phParms = new Dictionary<string, object>
                          {
                              ["XML"] = new XElement("ParticipationPaymentHistories", new XElement("ParticipationPaymentHistory",
                                                                                                   new XElement("CaseNumber",             NewPayment.CaseNumber),
                                                                                                   new XElement("ParticipationBeginDate", NewPayment.ParticipationBeginDate),
                                                                                                   new XElement("ParticipationEndDate",   NewPayment.ParticipationEndDate))).ToString(),
                              ["ReturnResult"] = true
                          };

            var phRs = _participationEntryRepository.GetStoredProcReturnValue("USP_Update_T0485_PYMT_ADJ_IND", phParms);

            var t3049Parms = new Dictionary<string, object>
                             {
                                 ["XML"] = new XElement("ParticipationPaymentHistories", new XElement("ParticipationPaymentHistory",
                                                                                                      new XElement("CaseNumber",             NewPayment.CaseNumber),
                                                                                                      new XElement("EffectiveMonth",         NewPayment.EffectiveMonth),
                                                                                                      new XElement("ParticipationBeginDate", NewPayment.ParticipationBeginDate),
                                                                                                      new XElement("ParticipationEndDate",   NewPayment.ParticipationEndDate),
                                                                                                      new XElement("RevisedPaymentAmount",   NewPayment.RevisedPaymentAmount))).ToString()
                             };

            var t3049Rs = _participationEntryRepository.GetStoredProcReturnValue("USP_Update_T3049_RCLC_W2_D_PT", t3049Parms);

            return phRs != 0 || t3049Rs != 0;
        }

        private void IssueOverPaymentOrAuxiliary(ParticipantPaymentHistory delayedPayment, DateTime today, UpdatePlacement i, Participant participant, string modifiedBy, DateTime modifiedDate, decimal newBasePayment, decimal dfPenaltyPct, DateTime participationBeginDate, DateTime participationEndDate, int? workerId)
        {
            var revisedPaymentAmount = delayedPayment.BaseW2Payment - newBasePayment;
            var participationEntryHistory = _participationEntryHistoryRepository.GetMany(j => j.ParticipantId == participant.Id && j.ModifiedBy != "WWP Batch" && j.ParticipationDate <= participationEndDate)
                                                                                .OrderByDescending(j => j.ModifiedDate)
                                                                                .FirstOrDefault();
            var officeId = participationEntryHistory?.EmployabilityPlan?.ParticipantEnrolledProgram?.OfficeId;
            var pep      = participant.ParticipantEnrolledPrograms.OrderByDescending(j => j.EnrollmentDate).FirstOrDefault(j => j.EnrollmentDate <= today.SpecificDayOfMonth(15).Date);

            int? pepWorkerId;

            if (participationEntryHistory == null)
            {
                officeId    = pep?.OfficeId;
                pepWorkerId = pep?.WorkerId;
            }
            else
                pepWorkerId = _workerRepository.Get(j => j.WIUId == participationEntryHistory.ModifiedBy)?.Id;

            pepWorkerId = pepWorkerId ?? workerId;

            AddParticipantPaymentHistory(delayedPayment, newBasePayment, dfPenaltyPct, modifiedBy, modifiedDate);

            if (officeId == null || pepWorkerId == null)
            {
                _overUnderPaymentResults.Add(new OverUnderPaymentResult
                                             {
                                                 PinNumber            = i.PinNumber,
                                                 CaseNumber           = i.CaseNumber,
                                                 BeginDate            = participationBeginDate,
                                                 EndDate              = participationEndDate,
                                                 RevisedPaymentAmount = revisedPaymentAmount
                                             });

                return;
            }

            InsertWorkerTask(participant, i, modifiedBy, modifiedDate, WorkerTaskCategoryCodes.BackDatedPlacementDelayedCycleCode, revisedPaymentAmount, participationBeginDate, participationEndDate);

            if (revisedPaymentAmount > 0)
                AddOverPayment(participant, participationBeginDate, participationEndDate, i, revisedPaymentAmount, (int) officeId, today, modifiedBy, modifiedDate);
            else
                AddAuxiliary(participant, participationBeginDate, participationEndDate, i, revisedPaymentAmount, (int) officeId, pep, modifiedBy, modifiedDate);
        }

        private void AddParticipantPaymentHistory(ParticipantPaymentHistory delayedPayment, decimal newBasePayment, decimal dfPenaltyPct, string modifiedBy, DateTime modifiedDate)
        {
            var participantPaymentHistory = _participantPaymentHistoryRepository.New();
            var newDrugFelonPenalty       = Math.Floor(decimal.Multiply(newBasePayment, dfPenaltyPct));

            participantPaymentHistory.CaseNumber                = delayedPayment.CaseNumber;
            participantPaymentHistory.EffectiveMonth            = delayedPayment.EffectiveMonth;
            participantPaymentHistory.ParticipationBeginDate    = delayedPayment.ParticipationBeginDate;
            participantPaymentHistory.ParticipationEndDate      = delayedPayment.ParticipationEndDate;
            participantPaymentHistory.BaseW2Payment             = newBasePayment;
            participantPaymentHistory.DrugFelonPenalty          = newDrugFelonPenalty;
            participantPaymentHistory.Recoupment                = delayedPayment.Recoupment;
            participantPaymentHistory.LearnFarePenalty          = delayedPayment.LearnFarePenalty;
            participantPaymentHistory.NonParticipationReduction = delayedPayment.NonParticipationReduction;
            participantPaymentHistory.VendorPayment             = delayedPayment.VendorPayment;
            participantPaymentHistory.ParticipantPayment = participantPaymentHistory.BaseW2Payment             - participantPaymentHistory.DrugFelonPenalty -
                                                           participantPaymentHistory.Recoupment                - participantPaymentHistory.LearnFarePenalty -
                                                           participantPaymentHistory.NonParticipationReduction - participantPaymentHistory.VendorPayment;
            participantPaymentHistory.IsOriginal   = false;
            participantPaymentHistory.ModifiedBy   = modifiedBy;
            participantPaymentHistory.ModifiedDate = modifiedDate;
            participantPaymentHistory.CreatedDate  = modifiedDate;

            NewPayment = new NewPayment
                         {
                             CaseNumber             = participantPaymentHistory.CaseNumber,
                             EffectiveMonth         = participantPaymentHistory.EffectiveMonth,
                             ParticipationBeginDate = participantPaymentHistory.ParticipationBeginDate,
                             ParticipationEndDate   = participantPaymentHistory.ParticipationEndDate,
                             RevisedPaymentAmount = participantPaymentHistory.BaseW2Payment - participantPaymentHistory.DrugFelonPenalty -
                                                    participantPaymentHistory.Recoupment    - participantPaymentHistory.LearnFarePenalty -
                                                    participantPaymentHistory.NonParticipationReduction
                         };

            _participantPaymentHistoryRepository.Add(participantPaymentHistory);
        }

        private void AddOverPayment(Participant participant, DateTime participationBeginDate, DateTime participationEndDate, UpdatePlacement placementEntry, decimal revisedPaymentAmount, int officeId, DateTime today, string modifiedBy, DateTime modifiedDate)
        {
            var overPayment = _overPaymentRepository.New();

            overPayment.ParticipantId                = participant.Id;
            overPayment.ParticipationPeriodBeginDate = participationBeginDate;
            overPayment.ParticipationPeriodEndDate   = participationEndDate;
            overPayment.CaseNumber                   = placementEntry.CaseNumber;
            overPayment.Amount                       = revisedPaymentAmount;
            overPayment.OfficeId                     = officeId;
            overPayment.Reason                       = "Participation Updated";
            overPayment.CreatedDate                  = today;
            overPayment.ModifiedBy                   = modifiedBy;
            overPayment.ModifiedDate                 = modifiedDate;

            _overPaymentRepository.Add(overPayment);
        }

        private void AddAuxiliary(Participant participant, DateTime participationBeginDate, DateTime participationEndDate, UpdatePlacement placementEntry, decimal revisedPaymentAmount, int officeId, ParticipantEnrolledProgram pep, string modifiedBy, DateTime modifiedDate)
        {
            var aux        = _auxiliaryRepository.New();
            var auxStatus  = _auxiliaryStatusRepository.New();
            var office     = _officeRepository.Get(i => i.Id == officeId);
            var periodName = participationBeginDate.GetParticipationPeriodName(participationEndDate);

            aux.ParticipantId           = participant.Id;
            aux.OrganizationId          = office.ContractArea.OrganizationId.GetValueOrDefault();
            aux.OfficeId                = officeId;
            aux.CountyId                = office.CountyandTribeId;
            aux.AuxiliaryReasonId       = _auxiliaryReasonRepository.Get(i => i.Code           == AuxiliaryReason.ParticipationUpdated).Id;
            aux.ParticipationPeriodId   = _participationPeriodLookUpRepository.Get(i => i.Name == periodName).Id;
            aux.CaseNumber              = placementEntry.CaseNumber;
            aux.EnrolledProgramId       = pep.EnrolledProgramId.GetValueOrDefault();
            aux.AGSequenceNumber        = pep.AGSequenceNumber;
            aux.ParticipationPeriodYear = (short) participationEndDate.Year;
            aux.OriginalPayment         = 0;
            aux.RequestedAmount         = Math.Abs(revisedPaymentAmount);
            aux.IsSystemRequested       = true;
            aux.ModifiedBy              = modifiedBy;
            aux.ModifiedDate            = modifiedDate;

            auxStatus.Auxiliary             = aux;
            auxStatus.AuxiliaryStatusTypeId = _auxiliaryStatusTypeRepository.Get(i => i.Code == AuxiliaryStatusType.SystemGenerated).Id;
            auxStatus.AuxiliaryStatusDate   = modifiedDate;
            auxStatus.Details               = $"Placement Update ({modifiedDate:yyyy-MM-dd})";
            auxStatus.ModifiedBy            = modifiedBy;
            auxStatus.ModifiedDate          = modifiedDate;

            _auxiliaryRepository.Add(aux);
            _auxiliaryStatusRepository.Add(auxStatus);
        }

        private void UpdateEntry(ParticipationEntry pe, UpdatePlacement updatePlacement, IEnumerable<PlacementType> placementTypes, string modifiedBy, DateTime modifiedDate)
        {
            var placementTypeId = updatePlacement.PlacementEndDate == null
                                      ? placementTypes.FirstOrDefault(i => i.DB2Code == updatePlacement.PlacementType)?.Id
                                      : null;

            pe.CaseNumber      = updatePlacement.CaseNumber;
            pe.PlacementTypeId = placementTypeId;
            pe.IsProcessed     = false;
            pe.ModifiedBy      = modifiedBy;
            pe.ModifiedDate    = modifiedDate;
        }

        #endregion
    }

    public class SanctionableSPResult
    {
        public int      ParticipantId          { get; set; }
        public int      ParticipationEntryId   { get; set; }
        public DateTime ParticipationDate      { get; set; }
        public int      EPId                   { get; set; }
        public int      ActivityId             { get; set; }
        public int      PlacementTypeId        { get; set; }
        public decimal  NonParticipatedHours   { get; set; }
        public bool     FormalAssessmentExists { get; set; }
        public bool     HoursSanctionable      { get; set; }
        public string   ModifiedBy             { get; set; }
        public DateTime ModifiedDate           { get; set; }
    }

    public class NewPayment
    {
        public decimal  CaseNumber             { get; set; }
        public int      EffectiveMonth         { get; set; }
        public DateTime ParticipationBeginDate { get; set; }
        public DateTime ParticipationEndDate   { get; set; }
        public decimal  RevisedPaymentAmount   { get; set; }
    }

    public class USP_GetPaymentAmount_ByLastPaidPlacement
    {
        public int      ParticipantId                 { get; set; }
        public decimal  PinNumber                     { get; set; }
        public decimal  CaseNumber                    { get; set; }
        public DateTime ParticipationBeginDate        { get; set; }
        public DateTime ParticipationEndDate          { get; set; }
        public decimal  NewBaseW2Payment              { get; set; }
        public DateTime PlacementStartDate            { get; set; }
        public DateTime PlacementEndDate              { get; set; }
        public decimal  LastPaidPlacementPerDayAmount { get; set; }
        public decimal  DFPenaltyPct                  { get; set; }
        public decimal  LFPenalty                     { get; set; }
    }

    public class OverUnderPayment
    {
        public decimal                   CaseNumber             { get; set; }
        public DateTime                  Today                  { get; set; }
        public DateTime                  ParticipationBeginDate { get; set; }
        public DateTime                  ParticipationEndDate   { get; set; }
        public ParticipantPaymentHistory DelayedPayment         { get; set; }
        public UpdatePlacement           PlacementEntry         { get; set; }
        public ParticipantPlacement      LastPlacementEntry     { get; set; }
        public Participant               Participant            { get; set; }
        public int?                      WorkerId               { get; set; }
    }
}
