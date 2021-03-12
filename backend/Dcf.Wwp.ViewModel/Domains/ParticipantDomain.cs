using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using AutoMapper;
using DCF.Common.Exceptions;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.Api.Library.Extensions;
using Dcf.Wwp.Api.Library.Interfaces;
using Dcf.Wwp.DataAccess.Models;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.Model.Interface.Constants;
using Dcf.Wwp.Model.Interface.Core;
using AuxiliaryStatusType = Dcf.Wwp.Model.Interface.Constants.AuxiliaryStatusType;

namespace Dcf.Wwp.Api.Library.Domains
{
    public class ParticipantDomain : IParticipantDomain
    {
        #region Properties

        private readonly IParticipantRepository                     _participantRepo;
        private readonly IParticipationStatusRepository             _profileStatusRepo;
        private readonly IUnitOfWork                                _uow;
        private readonly IAuthUser                                  _authUser;
        private readonly IMapper                                    _mapper;
        private readonly IActivityRepository                        _activityRepository;
        private readonly IParticipantPlacementRepository            _participantPlacementRepository;
        private readonly IParticipantPaymentHistoryRepository       _participantPaymentHistoryRepository;
        private readonly IEmployabilityPlanActivityBridgeRepository _epActivityBridgeRepository;
        private readonly IEmployabilityPlanRepository               _epRepository;
        private readonly IAuxiliaryRepository                       _auxiliaryRepository;
        private readonly IParticipationPeriodSummaryRepository      _participationPeriodSummaryRepository;
        private readonly IAfterPullDownWeeklyBatchDetailsRepository _afterPullDownWeeklyBatchDetailsRepository;
        private readonly IWorkerRepository                          _workerRepository;
        private readonly ITransactionDomain                         _transactionDomain;
        private const    string                                     _faStatusCd = "FA";

        #endregion

        #region Methods

        public ParticipantDomain(IParticipantRepository                     participantRepo,
                                 IParticipationStatusRepository             profileStatusRepo,
                                 IUnitOfWork                                uow,
                                 IAuthUser                                  authUser,
                                 IMapper                                    mapper,
                                 IActivityRepository                        activityRepository,
                                 IParticipantPlacementRepository            participantPlacementRepository,
                                 IParticipantPaymentHistoryRepository       participantPaymentHistoryRepository,
                                 IEmployabilityPlanActivityBridgeRepository epActivityBridgeRepository,
                                 IEmployabilityPlanRepository               epRepository,
                                 IAuxiliaryRepository                       auxiliaryRepository,
                                 IParticipationPeriodSummaryRepository      participationPeriodSummaryRepository,
                                 IAfterPullDownWeeklyBatchDetailsRepository afterPullDownWeeklyBatchDetailsRepository,
                                 ITransactionDomain                         transactionDomain, IWorkerRepository workerRepository)
        {
            _participantRepo                           = participantRepo;
            _profileStatusRepo                         = profileStatusRepo;
            _uow                                       = uow;
            _authUser                                  = authUser;
            _mapper                                    = mapper;
            _activityRepository                        = activityRepository;
            _participantPlacementRepository            = participantPlacementRepository;
            _participantPaymentHistoryRepository       = participantPaymentHistoryRepository;
            _epActivityBridgeRepository                = epActivityBridgeRepository;
            _epRepository                              = epRepository;
            _auxiliaryRepository                       = auxiliaryRepository;
            _participationPeriodSummaryRepository      = participationPeriodSummaryRepository;
            _afterPullDownWeeklyBatchDetailsRepository = afterPullDownWeeklyBatchDetailsRepository;
            _transactionDomain                         = transactionDomain;
            _workerRepository                          = workerRepository;
        }

        public bool IsValidPin(string pin)
        {
            if (string.IsNullOrEmpty(pin))
            {
                throw new ArgumentNullException(nameof(pin));
            }

            var pinNo      = decimal.Parse(pin);
            var isValidPin = _participantRepo.GetAsQueryable().Any(i => i.PinNumber == pinNo);

            return (isValidPin);
        }

        public List<ParticipantStatusContract> GetCurrentStatusesForPin(string pin)
        {
            if (string.IsNullOrEmpty(pin))
            {
                throw new ArgumentNullException(nameof(pin));
            }

            var results = new List<ParticipantStatusContract>();

            var pinNo = decimal.Parse(pin);

            var participantStatuses = _profileStatusRepo.GetMany(i => (i.Participant.PinNumber == pinNo) & (i.EndDate == null) && i.IsDeleted == false)
                                                        .ToList();

            if (participantStatuses.Any())
            {
                results = participantStatuses.OrderByDescending(i => i.BeginDate)
                                             .ThenByDescending(i => i.EndDate)
                                             .Select(i => new ParticipantStatusContract
                                                          {
                                                              Id                  = i.Id,
                                                              ParticipantId       = i.ParticipantId,
                                                              Pin                 = pinNo,
                                                              StatusId            = i.StatusId,
                                                              StatusName          = i.Status?.Name,
                                                              StatusCode          = i.Status?.Code,
                                                              Details             = i.Details,
                                                              BeginDate           = i.BeginDate,
                                                              EndDate             = i.EndDate,
                                                              EnrolledProgramId   = i.EnrolledProgramId,
                                                              EnrolledProgramName = i.EnrolledProgram?.Name,
                                                              EnrolledProgramCode = i.EnrolledProgram?.ProgramCode
                                                          }
                                                    ).ToList();
            }

            return (results);
        }

        public List<ParticipantStatusContract> GetAllStatusesForPin(string pin)
        {
            if (string.IsNullOrEmpty(pin))
            {
                throw new ArgumentNullException(nameof(pin));
            }

            var results = new List<ParticipantStatusContract>();

            var pinNo = decimal.Parse(pin);

            var participantStatuses = _profileStatusRepo.GetMany(i => i.Participant.PinNumber == pinNo && i.IsDeleted == false)
                                                        .ToList();

            if (participantStatuses.Any())
            {
                results = participantStatuses.OrderByDescending(i => i.isCurrent)
                                             .ThenByDescending(i => i.BeginDate)
                                             .Select(i => new ParticipantStatusContract
                                                          {
                                                              Id                  = i.Id,
                                                              ParticipantId       = i.ParticipantId,
                                                              Pin                 = pinNo,
                                                              StatusId            = i.StatusId,
                                                              StatusName          = i.Status?.Name,
                                                              StatusCode          = i.Status?.Code,
                                                              Details             = i.Details,
                                                              BeginDate           = i.BeginDate,
                                                              EndDate             = i.EndDate,
                                                              IsCurrent           = i.isCurrent,
                                                              EnrolledProgramId   = i.EnrolledProgramId,
                                                              EnrolledProgramName = i.EnrolledProgram?.Name,
                                                              EnrolledProgramCode = i.EnrolledProgram?.ProgramCode
                                                          }
                                                    ).ToList();
            }

            return (results);
        }

        public ParticipantStatusContract GetStatus(string pin, int id)
        {
            if (string.IsNullOrEmpty(pin))
            {
                throw new ArgumentNullException(nameof(pin));
            }

            var ps = new ParticipantStatusContract();

            var participantStatus = _profileStatusRepo.Get(i => i.Id == id);
            if (participantStatus != null)
            {
                ps = new ParticipantStatusContract
                     {
                         Id                  = participantStatus.Id,
                         ParticipantId       = participantStatus.ParticipantId,
                         Pin                 = participantStatus.Participant.PinNumber,
                         BeginDate           = participantStatus.BeginDate,
                         EndDate             = participantStatus.EndDate,
                         StatusId            = participantStatus.StatusId,
                         StatusCode          = participantStatus.Status.Code,
                         Details             = participantStatus.Details,
                         IsCurrent           = participantStatus.isCurrent,
                         StatusName          = participantStatus.Status.Name,
                         EnrolledProgramId   = participantStatus.EnrolledProgramId,
                         EnrolledProgramName = participantStatus.EnrolledProgram?.Name
                     };
            }

            return (ps);
        }

        public List<ParticipantStatusContract> GetStatusesForPin(string pin, bool getAllStatuses = true)
        {
            if (string.IsNullOrEmpty(pin))
            {
                throw new ArgumentNullException(nameof(pin));
            }

            var results = new List<ParticipantStatusContract>();

            var pinNo = decimal.Parse(pin);

            var q = _profileStatusRepo.GetAsQueryable();

            q = getAllStatuses ? q.Where(i => i.Participant.PinNumber == pinNo && i.IsDeleted == false) : q.Where(i => i.Participant.PinNumber == pinNo && i.EndDate == null && i.IsDeleted == false);

            var participantStatuses = q.ToList();

            if (participantStatuses.AsNotNull().Any())
            {
                results = participantStatuses.OrderByDescending(i => i.BeginDate)
                                             .ThenByDescending(i => i.EndDate)
                                             .Select(i => new ParticipantStatusContract
                                                          {
                                                              Id                  = i.Id,
                                                              ParticipantId       = i.ParticipantId,
                                                              Pin                 = pinNo,
                                                              StatusId            = i.StatusId,
                                                              StatusName          = i.Status?.Name,
                                                              StatusCode          = i.Status?.Code,
                                                              Details             = i.Details,
                                                              BeginDate           = i.BeginDate,
                                                              EndDate             = i.EndDate,
                                                              IsCurrent           = i.isCurrent,
                                                              EnrolledProgramId   = i.EnrolledProgramId,
                                                              EnrolledProgramName = i.EnrolledProgram?.Name
                                                          }
                                                    ).ToList();
            }

            return (results);
        }

        public ParticipantStatusContract AddStatus(ParticipantStatusContract psc)
        {
            if (psc == null)
            {
                throw new ArgumentNullException(nameof(psc));
            }

            var modifiedBy   = _authUser.WIUID;
            var modifiedDate = DateTime.Now;
            var ps           = _profileStatusRepo.New();

            ps.ParticipantId     = psc.ParticipantId;
            ps.StatusId          = psc.StatusId;
            ps.BeginDate         = psc.BeginDate;
            ps.EndDate           = psc.EndDate;
            ps.isCurrent         = psc.IsCurrent;
            ps.EnrolledProgramId = psc.EnrolledProgramId;
            ps.Details           = psc.Details;
            ps.IsDeleted         = false;
            ps.ModifiedBy        = modifiedBy;
            ps.ModifiedDate      = modifiedDate;

            _profileStatusRepo.Add(ps);

            var participant = _participantRepo.Get(i => i.Id == psc.ParticipantId);

            if (psc.StatusCode == _faStatusCd)
            {
                var maxDate = psc.IsCurrent == true ? DateTime.MaxValue : psc.EndDate;
                var pe = participant.ParticipationEntries
                                    .Where(p => !p.IsDeleted && p.ParticipationDate >= psc.BeginDate && p.ParticipationDate <= maxDate);

                pe.ForEach(i =>
                           {
                               if (i.HoursSanctionable != true) return;
                               i.HoursSanctionable = false;
                               i.ModifiedBy        = modifiedBy;
                               i.ModifiedDate      = modifiedDate;
                           });
            }

            var officeId = participant.ParticipantEnrolledPrograms.First(j => j.EnrolledProgramId == psc.EnrolledProgramId).OfficeId;
            var transactionContract = new TransactionContract
                                      {
                                          ParticipantId       = psc.ParticipantId.GetValueOrDefault(),
                                          WorkerId            = _workerRepository.Get(x => x.WIUId == _authUser.WIUID).Id,
                                          OfficeId            = officeId.GetValueOrDefault(),
                                          EffectiveDate       = psc.BeginDate.GetValueOrDefault(),
                                          CreatedDate         = modifiedDate,
                                          TransactionTypeCode = TransactionTypes.ParticipationStatusBegins,
                                          ModifiedBy          = _authUser.WIUID,
                                          StatusCode          = psc.StatusCode
                                      };

            _transactionDomain.InsertTransaction(transactionContract);


            if (psc.EndDate != null)
            {
                transactionContract.EffectiveDate       = psc.EndDate ?? modifiedDate;
                transactionContract.TransactionTypeCode = TransactionTypes.ParticipationStatusEnd;

                _transactionDomain.InsertTransaction(transactionContract);
            }

            var rowsCommitted = _uow.Commit();

            if (rowsCommitted > 0)
            {
                psc.Id         = ps.Id;
                psc.StatusCode = ps.Status.Code;
                psc.StatusName = ps.Status.Name;
            }

            return (psc);
        }

        public ParticipantStatusContract UpdateStatus(ParticipantStatusContract psc)
        {
            if (psc == null)
            {
                throw new ArgumentNullException(nameof(psc));
            }

            var modifiedBy   = _authUser.WIUID;
            var modifiedDate = DateTime.Now;
            var ps           = _profileStatusRepo.Get(i => i.Id == psc.Id && !i.IsDeleted);

            var officeId = _participantRepo.Get(i => i.Id                                              == psc.ParticipantId)
                                           .ParticipantEnrolledPrograms.First(j => j.EnrolledProgramId == psc.EnrolledProgramId)
                                           .OfficeId;

            if (ps.EndDate == null && psc.EndDate != null)
            {
                var transactionContract = new TransactionContract
                                          {
                                              ParticipantId       = psc.ParticipantId.GetValueOrDefault(),
                                              WorkerId            = _workerRepository.Get(x => x.WIUId == _authUser.WIUID).Id,
                                              OfficeId            = officeId.GetValueOrDefault(),
                                              EffectiveDate       = psc.EndDate ?? modifiedDate,
                                              CreatedDate         = modifiedDate,
                                              TransactionTypeCode = TransactionTypes.ParticipationStatusEnd,
                                              ModifiedBy          = _authUser.WIUID,
                                              StatusCode          = psc.StatusCode
                                          };

                _transactionDomain.InsertTransaction(transactionContract);
            }

            ps.EndDate      = psc.EndDate;
            ps.isCurrent    = psc.IsCurrent;
            ps.Details      = psc.Details;
            ps.ModifiedBy   = modifiedBy;
            ps.ModifiedDate = modifiedDate;

            _profileStatusRepo.Update(ps);

            int rowsCommitted;

            if (psc.EndDate != null && psc.StatusCode == _faStatusCd)
            {
                using (var tx = _profileStatusRepo.GetDataBase().BeginTransaction())
                {
                    try
                    {
                        rowsCommitted = _uow.Commit();

                        var beginDate = psc.EndDate.Value.AddDays(1);
                        var endDate   = DateTime.Today;
                        var rs        = ExecSanctionableSP(modifiedBy, modifiedDate, psc.ParticipantId, beginDate, endDate);

                        if (rs == 0)
                            tx.Commit();
                    }
                    catch (Exception ex)
                    {
                        tx.Dispose();
                        throw new DCFApplicationException("Failed due to SProc issue. Please try again.", ex);
                    }

                    tx.Dispose();
                }
            }
            else
            {
                rowsCommitted = _uow.Commit();
            }

            if (rowsCommitted <= 0) return (psc);
            psc.Id         = ps.Id;
            psc.StatusCode = ps.Status.Code;
            psc.StatusName = ps.Status.Name;

            return (psc);
        }

        public List<Activity> GetActivitiesForTjOrTmjProgramsForPreCheck(string pin)
        {
            var decimalPin = decimal.Parse(pin);
            var epIds = _epRepository.GetMany(i => i.Participant.PinNumber == decimalPin
                                                   && (i.EnrolledProgramId    == Wwp.Model.Interface.Constants.EnrolledProgram.TransformMilwaukeeJobsId
                                                       || i.EnrolledProgramId == Wwp.Model.Interface.Constants.EnrolledProgram.TransitionalJobsId))
                                     .Select(j => j.Id).ToList();
            var activityIds = _epActivityBridgeRepository.GetMany(i => epIds.Contains(i.EmployabilityPlanId)).Select(j => j.ActivityId).ToList();
            var activities  = _activityRepository.GetMany(i => activityIds.Contains(i.Id)).Select(j => j).ToList();

            return activities;
        }

        public List<Activity> GetActivitiesForPreCheckBasedOnProgram(string pin, int? enrolledProgramId)
        {
            var decimalPin = decimal.Parse(pin);
            var epIds = _epRepository.GetMany(i => i.Participant.PinNumber == decimalPin
                                                   && (i.EnrolledProgramId == enrolledProgramId))
                                     .Select(j => j.Id).ToList();
            var activityIds = _epActivityBridgeRepository.GetMany(i => epIds.Contains(i.EmployabilityPlanId)).Select(j => j.ActivityId).ToList();
            var activities  = _activityRepository.GetMany(i => activityIds.Contains(i.Id)).Select(j => j).ToList();

            return activities;
        }

        public int ExecSanctionableSP(string modifiedBy, DateTime modifiedDate, int? participantId = null, DateTime? beginDate = null, DateTime? maxDate = null, IEnumerable<ParticipationEntry> pe = null)
        {
            pe = pe ?? _participantRepo.Get(i => !i.IsDeleted   && i.Id                == participantId).ParticipationEntries
                                       .Where(p => !p.IsDeleted && p.ParticipationDate >= beginDate && p.ParticipationDate <= maxDate);

            var xml = new XElement("ParticipationEntries", pe.Select(i => new XElement("ParticipationEntry", new XElement("ParticipantId", i.ParticipantId),
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
                            ["ModifiedDate"]            = modifiedDate
                        };

            var rs = _profileStatusRepo.GetStoredProcReturnValue("USP_IsNonParticipationSanctionable", parms);

            return rs;
        }

        public List<decimal> GetCaseNumbersBasedOnParticipationPeriod(string pin, string participationPeriod, short year)
        {
            var splitPeriod   = participationPeriod.SplitStringToDate(year);
            var beginDate     = splitPeriod[0];
            var endDate       = splitPeriod[1];
            var decimalPin    = decimal.Parse(pin);
            var beginDateTime = DateTime.Parse(beginDate);
            var endDateTime   = DateTime.Parse(endDate);
            var participant   = _participantRepo.Get(i => i.PinNumber == decimalPin);
            var caseNumbers = _participantPlacementRepository.GetMany(i => i.ParticipantId == participant.Id
                                                                           && (i.PlacementStartDate <= endDateTime)
                                                                           && ((i.PlacementEndDate    >= i.PlacementStartDate
                                                                                && i.PlacementEndDate >= beginDateTime)
                                                                               || i.PlacementEndDate == null))
                                                             .Select(i => i.CaseNumber)
                                                             .Distinct()
                                                             .ToList();

            return caseNumbers;
        }

        public PaymentDetailsContract GetDetailsBasedOnParticipationPeriod(string pin, string participationPeriod, short year, string caseNumber)
        {
            var splitPeriod       = participationPeriod.SplitStringToDate(year);
            var beginDate         = splitPeriod[0];
            var endDate           = splitPeriod[1];
            var decimalPin        = decimal.Parse(pin);
            var decimalCaseNumber = decimal.Parse(caseNumber);
            var beginDateTime     = DateTime.Parse(beginDate);
            var endDateTime       = DateTime.Parse(endDate);

            var participantPaymentHistories = _mapper.Map<List<ParticipantPaymentHistoryContract>>(_participantPaymentHistoryRepository.GetMany(i => i.CaseNumber                == decimalCaseNumber
                                                                                                                                                     && i.ParticipationBeginDate >= beginDateTime
                                                                                                                                                     && i.ParticipationEndDate   <= endDateTime))
                                                     .OrderByDescending(i => i.IsOriginal & i.IsDelayed)
                                                     .ThenByDescending(i => !i.IsOriginal & i.IsDelayed)
                                                     .ThenByDescending(i =>  i.IsOriginal & !i.IsDelayed)
                                                     .ThenBy(i => i.CreatedDate)
                                                     .ToList();

            var manualAux = _auxiliaryRepository.GetMany(i => i.Participant.PinNumber       == decimalPin
                                                              && i.CaseNumber               == decimalCaseNumber
                                                              && i.ParticipationPeriod.Name == participationPeriod
                                                              && i.AuxiliaryStatuses.Any(j => j.AuxiliaryStatusType.Code == AuxiliaryStatusType.ApproveCd)
                                                              && i.ModifiedBy != "WWP Batch"
                                                              && !i.IsDeleted)
                                                .Select(i => new ManualAuxiliary
                                                             {
                                                                 Reason       = $"{i.AuxiliaryReason.Code} - {i.AuxiliaryReason.Name}",
                                                                 Amount       = i.RequestedAmount.ToString(CultureInfo.InvariantCulture),
                                                                 ApprovalDate = i.AuxiliaryStatuses.FirstOrDefault(j => j.AuxiliaryStatusType.Code == AuxiliaryStatusType.ApproveCd)?.AuxiliaryStatusDate.ToString("MM/dd/yyyy")
                                                             }).ToList();

            var unAppliedHours = _participationPeriodSummaryRepository.GetMany(i => i.CaseNumber                      == decimalCaseNumber
                                                                                    && i.ParticipationPeriodBeginDate == beginDateTime
                                                                                    && i.ParticipationPeriodEndDate   == endDateTime
                                                                                    && !i.IsDeleted)
                                                                      .GroupBy(i => new
                                                                                    {
                                                                                        i.CaseNumber,
                                                                                        i.ParticipationPeriodBeginDate,
                                                                                        i.ParticipationPeriodEndDate
                                                                                    })
                                                                      .Select(i => new UnAppliedSanctionableHours
                                                                                   {
                                                                                       UnAppliedHours = i.Sum(j => j.UnAppliedHours) ?? 0.0m,
                                                                                       LastUpdated    = i.Max(j => j.ModifiedDate)?.ToString("MM/dd/yyyy")
                                                                                   }).FirstOrDefault();

            var contract = new PaymentDetailsContract
                           {
                               ParticipantPaymentHistories = participantPaymentHistories,
                               ManualAuxiliaries           = manualAux,
                               UnAppliedSanctionableHours  = unAppliedHours
                           };

            return contract;
        }

        #endregion
    }
}
