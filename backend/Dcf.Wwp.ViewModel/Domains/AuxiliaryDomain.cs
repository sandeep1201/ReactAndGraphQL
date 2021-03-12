using System;
using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.Api.Library.Extensions;
using Dcf.Wwp.Api.Library.Interfaces;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;
using Dcf.Wwp.Model.Interface.Core;
using Dcf.Wwp.Model.Interface.Constants;
using AuxiliaryStatusType = Dcf.Wwp.Model.Interface.Constants.AuxiliaryStatusType;
using EnrolledProgram = Dcf.Wwp.Model.Interface.Constants.EnrolledProgram;

namespace Dcf.Wwp.Api.Library.Domains
{
    public class AuxiliaryDomain : IAuxiliaryDomain
    {
        #region Properties

        private readonly IUnitOfWork                    _unitOfWork;
        private readonly IAuthUser                      _authUser;
        private readonly Func<string, string>           _convertWIUIdToName;
        private readonly IAuxiliaryRepository           _auxiliaryRepository;
        private readonly IAuxiliaryStatusRepository     _auxiliaryStatusRepository;
        private readonly IAuxiliaryStatusTypeRepository _auxiliaryStatusTypeRepository;
        private readonly IOfficeRepository              _officeRepository;
        private readonly ICountyAndTribeRepository      _countyAndTribeRepository;
        private readonly IEnrolledProgramRepository     _enrolledProgramRepository;
        private readonly IWorkerTaskListDomain          _workerTaskListDomain;
        private readonly IWorkerTaskCategoryRepository  _workerTaskCategoryRepository;
        private readonly IWorkerRepository              _workerRepository;

        #endregion

        #region Methods

        public AuxiliaryDomain(IUnitOfWork                    unitOfWork,
                               IAuthUser                      authUser,
                               IAuxiliaryRepository           auxiliaryRepository,
                               IAuxiliaryStatusRepository     auxiliaryStatusRepository,
                               IAuxiliaryStatusTypeRepository auxiliaryStatusTypeRepository,
                               IOfficeRepository              officeRepository,
                               ICountyAndTribeRepository      countyAndTribeRepository,
                               IEnrolledProgramRepository     enrolledProgramRepository,
                               IWorkerTaskListDomain          workerTaskListDomain,
                               IWorkerTaskCategoryRepository  workerTaskCategoryRepository,
                               IWorkerRepository              workerRepository)
        {
            _unitOfWork                    = unitOfWork;
            _authUser                      = authUser;
            _auxiliaryRepository           = auxiliaryRepository;
            _auxiliaryStatusRepository     = auxiliaryStatusRepository;
            _auxiliaryStatusTypeRepository = auxiliaryStatusTypeRepository;
            _officeRepository              = officeRepository;
            _countyAndTribeRepository      = countyAndTribeRepository;
            _enrolledProgramRepository     = enrolledProgramRepository;
            _workerTaskListDomain          = workerTaskListDomain;
            _workerTaskCategoryRepository  = workerTaskCategoryRepository;
            _workerRepository              = workerRepository;

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

        public List<AuxiliaryContract> GetAuxiliaries(int? participantId)
        {
            var contract        = new List<AuxiliaryContract>();
            var agencyCd        = _authUser.AgencyCode.ToLower().Trim();
            var auxCodesToCheck = new List<string> { AuxiliaryStatusType.SubmitCd, AuxiliaryStatusType.ReviewCd, AuxiliaryStatusType.ReturnCd };
            var auxes = participantId != null
                            ? _auxiliaryRepository.GetMany(i => i.ParticipantId == participantId && !i.IsDeleted).ToList()
                            : _auxiliaryRepository.GetAsQueryable()
                                                  .Where(i => !i.IsDeleted && i.Organization.EntsecAgencyCode.ToLower().Trim() == agencyCd)
                                                  .SelectMany(i => i.AuxiliaryStatuses
                                                                    .OrderByDescending(j => j.Id)
                                                                    .Take(1))
                                                  .Where(i => auxCodesToCheck.Contains(i.AuxiliaryStatusType.Code) || (i.AuxiliaryStatusType.Code == AuxiliaryStatusType.ApproveCd && i.AuxiliaryStatusDate == DateTime.Today))
                                                  .Select(i => i.Auxiliary)
                                                  .ToList();

            auxes.ForEach(i =>
                          {
                              var office          = i.Office;
                              var county          = i.CountyAndTribe;
                              var enrolledProgram = i.EnrolledProgram;
                              var auxStatus       = i.AuxiliaryStatuses.OrderByDescending(a => a.Id).FirstOrDefault();

                              contract.Add(
                                           new AuxiliaryContract
                                           {
                                               Id                             = i.Id,
                                               PinNumber                      = i.Participant.PinNumber,
                                               FirstName                      = i.Participant.FirstName,
                                               MiddleInitial                  = i.Participant.MiddleInitial,
                                               LastName                       = i.Participant.LastName,
                                               SuffixName                     = i.Participant.SuffixName,
                                               ParticipantId                  = i.ParticipantId,
                                               CaseNumber                     = i.CaseNumber,
                                               CountyNumber                   = county?.CountyNumber,
                                               CountyName                     = county?.CountyName,
                                               OfficeNumber                   = office?.OfficeNumber,
                                               OfficeName                     = office?.OfficeName,
                                               AgencyCode                     = i.Organization?.EntsecAgencyCode,
                                               ProgramCd                      = enrolledProgram.ProgramCode.Trim(),
                                               SubProgramCd                   = enrolledProgram.SubProgramCode.SafeTrim(),
                                               AGSequenceNumber               = i.AGSequenceNumber,
                                               ParticipationPeriodId          = i.ParticipationPeriodId,
                                               ParticipationPeriodName        = i.ParticipationPeriod.Name,
                                               ParticipationPeriodYear        = i.ParticipationPeriodYear,
                                               OriginalPayment                = i.OriginalPayment,
                                               RequestedAmount                = i.RequestedAmount,
                                               AuxiliaryReasonId              = i.AuxiliaryReasonId,
                                               AuxiliaryReasonName            = i.AuxiliaryReason.Name,
                                               AuxiliaryStatusTypeId          = auxStatus?.AuxiliaryStatusTypeId,
                                               AuxiliaryStatusTypeCode        = auxStatus?.AuxiliaryStatusType.Code,
                                               AuxiliaryStatusTypeName        = auxStatus?.AuxiliaryStatusType.Name,
                                               AuxiliaryStatusTypeDisplayName = auxStatus?.AuxiliaryStatusType.DisplayName,
                                               AuxiliaryStatusDate            = auxStatus?.AuxiliaryStatusDate,
                                               Details                        = auxStatus?.Details,
                                               ModifiedBy                     = _convertWIUIdToName(i.ModifiedBy),
                                               ModifiedDate                   = i.ModifiedDate,
                                               WIUID                          = i.ModifiedBy,
                                               RequestedUserForApprovalAndDB2 = i.RequestedUserForApprovalAndDB2,
                                               AuxiliaryStatuses = i.AuxiliaryStatuses?.OrderByDescending(a => a.Id)
                                                                    .Select(a => new AuxiliaryStatusContract
                                                                                 {
                                                                                     Id                         = a.Id,
                                                                                     AuxiliaryStatusTypeId      = a.AuxiliaryStatusTypeId,
                                                                                     AuxiliaryStatusName        = a.AuxiliaryStatusType.Name,
                                                                                     AuxiliaryStatusDisplayName = a.AuxiliaryStatusType.DisplayName,
                                                                                     AuxiliaryStatusDate        = a.AuxiliaryStatusDate,
                                                                                     Details                    = a.Details,
                                                                                     ModifiedBy                 = _convertWIUIdToName(a.ModifiedBy),
                                                                                     ModifiedDate               = a.ModifiedDate
                                                                                 }).ToList()
                                           });
                          });

            return contract.OrderByDescending(i => i.AuxiliaryStatusDate).ThenByDescending(i => i.ModifiedDate).ToList();
        }

        public AuxiliaryContract GetAuxiliary(int id)
        {
            var aux             = _auxiliaryRepository.Get(i => i.Id == id);
            var office          = aux.Office;
            var county          = aux.CountyAndTribe;
            var enrolledProgram = aux.EnrolledProgram;
            var contract = new AuxiliaryContract
                           {
                               Id                             = aux.Id,
                               PinNumber                      = aux.Participant.PinNumber,
                               FirstName                      = aux.Participant.FirstName,
                               MiddleInitial                  = aux.Participant.MiddleInitial,
                               LastName                       = aux.Participant.LastName,
                               SuffixName                     = aux.Participant.SuffixName,
                               CaseNumber                     = aux.CaseNumber,
                               CountyNumber                   = county?.CountyNumber,
                               CountyName                     = county?.CountyName,
                               OfficeNumber                   = office?.OfficeNumber,
                               OfficeName                     = office?.OfficeName,
                               AgencyCode                     = aux.Organization?.EntsecAgencyCode,
                               ProgramCd                      = enrolledProgram.ProgramCode.Trim(),
                               SubProgramCd                   = enrolledProgram.SubProgramCode.SafeTrim(),
                               AGSequenceNumber               = aux.AGSequenceNumber,
                               ParticipationPeriodId          = aux.ParticipationPeriodId,
                               ParticipationPeriodName        = aux.ParticipationPeriod.Name,
                               ParticipationPeriodYear        = aux.ParticipationPeriodYear,
                               OriginalPayment                = aux.OriginalPayment,
                               RequestedAmount                = aux.RequestedAmount,
                               AuxiliaryReasonId              = aux.AuxiliaryReasonId,
                               AuxiliaryReasonName            = aux.AuxiliaryReason.Name,
                               ModifiedBy                     = _convertWIUIdToName(aux.ModifiedBy),
                               ModifiedDate                   = aux.ModifiedDate,
                               WIUID                          = aux.ModifiedBy,
                               RequestedUserForApprovalAndDB2 = aux.RequestedUserForApprovalAndDB2,
                               AuxiliaryStatuses = aux.AuxiliaryStatuses?.OrderByDescending(a => a.Id)
                                                      .Select(a => new AuxiliaryStatusContract
                                                                   {
                                                                       Id                         = a.Id,
                                                                       AuxiliaryStatusTypeId      = a.AuxiliaryStatusTypeId,
                                                                       AuxiliaryStatusName        = a.AuxiliaryStatusType.Name,
                                                                       AuxiliaryStatusDisplayName = a.AuxiliaryStatusType.DisplayName,
                                                                       AuxiliaryStatusDate        = a.AuxiliaryStatusDate,
                                                                       Details                    = a.Details,
                                                                       ModifiedBy                 = _convertWIUIdToName(a.ModifiedBy),
                                                                       ModifiedDate               = a.ModifiedDate
                                                                   }).ToList()
                           };

            return contract;
        }


        public AuxiliaryContract GetDetailsBasedOnParticipationPeriod(string pin, int participantId, string participationPeriod, short year)
        {
            var splitPeriod = participationPeriod.SplitStringToDate(year);
            var beginDate   = splitPeriod[0];
            var endDate     = splitPeriod[1];

            var parms = new Dictionary<string, object>
                        {
                            ["PinNumber"]     = pin,
                            ["ParticipantId"] = participantId,
                            ["BeginDate"]     = beginDate,
                            ["EndDate"]       = endDate,
                            ["AgencyCode"]    = _authUser.AgencyCode
                        };

            var rs = _auxiliaryRepository.ExecStoredProc<USP_GetDB2AuxDetails_Result>("USP_GetDB2AuxDetails", parms).FirstOrDefault();

            if (rs == null) return new AuxiliaryContract();

            var contract = new AuxiliaryContract
                           {
                               CaseNumber              = rs.CaseNumber,
                               CountyNumber            = rs.CountyNumber,
                               CountyName              = rs.CountyName,
                               OfficeNumber            = rs.OfficeNumber,
                               ProgramCd               = rs.ProgramCd,
                               SubProgramCd            = rs.SubProgramCd,
                               AGSequenceNumber        = rs.AGSequenceNumber,
                               ParticipationPeriodName = participationPeriod,
                               ParticipationPeriodYear = year,
                               OriginalPayment         = rs.PaymentAmount,
                               RecoupmentAmount        = rs.RecoupmentAmount,
                               IsAllowed               = rs.IsAllowed,
                               OverPayAmount           = rs.OverPymtAmt
                           };


            return contract;
        }


        public void UpsertAuxiliary(AuxiliaryContract contract)
        {
            if (contract == null)
            {
                throw new ArgumentNullException(nameof(contract));
            }

            var modifiedBy        = _authUser.WIUID;
            var modifiedDate      = DateTime.Now;
            var currentDate       = DateTime.Today;
            var aux               = contract.Id == 0 ? _auxiliaryRepository.New() : _auxiliaryRepository.Get(i => i.Id == contract.Id && !i.IsDeleted);
            var auxStatus         = _auxiliaryStatusRepository.New();
            var enrolledProgramId = _enrolledProgramRepository.Get(i => i.ProgramCode == contract.ProgramCd && i.SubProgramCode == contract.SubProgramCd).Id;

            if (contract.Id == 0)
            {
                aux.ParticipantId  = contract.ParticipantId;
                aux.OrganizationId = _workerRepository.Get(i => i.WIUId == _authUser.WIUID).OrganizationId.GetValueOrDefault();
                aux.OfficeId = _officeRepository.Get(i => !i.IsDeleted && i.OfficeNumber                    == contract.OfficeNumber
                                                                       && i.ContractArea.EnrolledProgram.Id == EnrolledProgram.WW
                                                                       && (i.InActivatedDate == null || i.InActivatedDate >= currentDate)
                                                                       && i.ActiviatedDate <= currentDate)?.Id;
                aux.CountyId                = _countyAndTribeRepository.Get(i => i.CountyNumber == contract.CountyNumber && !i.IsDeleted)?.Id;
                aux.AuxiliaryReasonId       = contract.AuxiliaryReasonId;
                aux.ParticipationPeriodId   = contract.ParticipationPeriodId;
                aux.CaseNumber              = contract.CaseNumber;
                aux.EnrolledProgramId       = enrolledProgramId;
                aux.AGSequenceNumber        = contract.AGSequenceNumber;
                aux.ParticipationPeriodYear = contract.ParticipationPeriodYear;
                aux.OriginalPayment         = contract.OriginalPayment.GetValueOrDefault();
                aux.IsSystemRequested       = false;
            }

            var auxStatusType = contract.IsSubmit
                                    ? _auxiliaryStatusTypeRepository.Get(i => i.Code == AuxiliaryStatusType.SubmitCd)
                                    : contract.IsWithdraw
                                        ? _auxiliaryStatusTypeRepository.Get(i => i.Code == AuxiliaryStatusType.WithdrewCd)
                                        : _auxiliaryStatusTypeRepository.Get(i => i.Id   == contract.AuxiliaryStatusTypeId);

            if (contract.IsSubmit)
                aux.RequestedUserForApprovalAndDB2 = modifiedBy;

            aux.RequestedAmount = contract.RequestedAmount;
            aux.ModifiedBy      = modifiedBy;
            aux.ModifiedDate    = modifiedDate;

            auxStatus.Auxiliary             = aux;
            auxStatus.AuxiliaryStatusTypeId = auxStatusType.Id;
            auxStatus.AuxiliaryStatusDate   = currentDate;
            auxStatus.Details               = contract.Details;
            auxStatus.ModifiedBy            = modifiedBy;
            auxStatus.ModifiedDate          = modifiedDate;

            var auxStatusCode = auxStatusType.Code;
            if (auxStatusCode == AuxiliaryStatusType.ApproveCd || auxStatusCode == AuxiliaryStatusType.DeniedCd || auxStatusCode == AuxiliaryStatusType.ReturnCd)
            {
                var categoryId = 0;
                var submittedBy = _auxiliaryStatusRepository.GetMany(i => i.AuxiliaryId == aux.Id && i.AuxiliaryStatusType.Code == AuxiliaryStatusType.SubmitCd)
                                                            ?.OrderByDescending(i => i.ModifiedDate)
                                                            .FirstOrDefault()
                                                            ?.ModifiedBy;
                switch (auxStatusCode)
                {
                    case AuxiliaryStatusType.ApproveCd:
                        categoryId = _workerTaskCategoryRepository.Get(i => i.Code == WorkerTaskCategoryCodes.AuxApprovedCode).Id;
                        break;
                    case AuxiliaryStatusType.DeniedCd:
                        categoryId = _workerTaskCategoryRepository.Get(i => i.Code == WorkerTaskCategoryCodes.AuxDeniedCode).Id;
                        break;
                    case AuxiliaryStatusType.ReturnCd:
                        categoryId = _workerTaskCategoryRepository.Get(i => i.Code == WorkerTaskCategoryCodes.AuxReturnedCode).Id;
                        break;
                }

                var workerTaskList = new WorkerTaskListContract
                                     {
                                         Id            = 0,
                                         CategoryId    = categoryId,
                                         StatusDate    = (_authUser.CDODate ?? modifiedDate).ToString("MM/dd/yyyy"),
                                         ParticipantId = contract.ParticipantId,
                                         TaskDate      = modifiedDate.ToString("MM/dd/yyyy"),
                                         TaskDetails   = contract.Details,
                                         WorkerId      = _workerRepository.Get(i => i.WIUId == submittedBy).Id,
                                         ModifiedBy    = modifiedBy,
                                         ModifiedDate  = modifiedDate
                                     };


                _workerTaskListDomain.UpsertWorkerTaskList(workerTaskList, true);
            }


            if (contract.Id == 0) _auxiliaryRepository.Add(aux);
            else _auxiliaryRepository.Update(aux);

            _auxiliaryStatusRepository.Add(auxStatus);

            _unitOfWork.Commit();
        }

        #endregion
    }
}
