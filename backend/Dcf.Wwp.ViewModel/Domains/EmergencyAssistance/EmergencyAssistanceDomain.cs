using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using DCF.Common.Logging;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.Api.Library.Contracts.EmergencyAssistance;
using Dcf.Wwp.Api.Library.Interfaces;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;
using Dcf.Wwp.Model.Interface.Core;
using IParticipantRepository = Dcf.Wwp.DataAccess.Interfaces.IParticipantRepository;
using IWorkerRepository = Dcf.Wwp.DataAccess.Interfaces.IWorkerRepository;

namespace Dcf.Wwp.Api.Library.Domains.EmergencyAssistance
{
    public partial class EmergencyAssistanceDomain : IEmergencyAssistanceDomain
    {
        #region Properties

        private readonly IUnitOfWork                             _unitOfWork;
        private readonly IAuthUser                               _authUser;
        private readonly IOrganizationRepository                 _organizationRepository;
        private readonly Func<string, string>                    _convertWIUIdToName;
        private readonly ILog                                    _logger = LogProvider.GetLogger(typeof(EmergencyAssistanceDomain));
        private readonly IEARequestRepository                    _eaRequestRepository;
        private readonly IEARequestParticipantBridgeRepository   _eaRequestParticipantBridgeRepository;
        private readonly IEARequestEmergencyTypeBridgeRepository _eaRequestEmergencyTypeBridgeRepository;
        private readonly IEACommentRepository                    _eaCommentRepository;
        private readonly IEACommentTypeBridgeRepository          _eaCommentTypeBridgeRepository;
        private readonly IParticipantRepository                  _participantRepository;
        private readonly IEARequestContactInfoRepository         _eaRequestContactInfoRepository;
        private readonly IEAAlternateMailingAddressRepository    _eaAlternateMailingAddressRepository;
        private readonly IEAImpendingHomelessnessRepository      _eaImpendingHomelessnessRepository;
        private readonly IEAHomelessnessRepository               _eaHomelessnessRepository;
        private readonly IEAEnergyCrisisRepository               _eaEnergyCrisisRepository;
        private readonly IEAHouseHoldIncomeRepository            _eaHouseHoldIncomeRepository;
        private readonly IEAAssetsRepository                     _eaAssetsRepository;
        private readonly IEAIndividualTypeRepository             _eaIndividualTypeRepository;
        private readonly IEARelationshipTypeRepository           _eaRelationshipTypeRepository;
        private readonly IEAEmergencyTypeRepository              _eaEmergencyTypeRepository;
        private readonly IEAStatusRepository                     _eaStatusRepository;
        private readonly IEligibilityByFPLRepository             _eligibilityByFplRepository;
        private readonly IEAPaymentAmountRepository              _eaPaymentAmountRepository;
        private readonly IEAPaymentRepository                    _eaPaymentRepository;
        private readonly IEAIPVRepository                        _eaIpvRepository;
        private readonly IEAIPVReasonBridgeRepository            _eaIpvReasonBridgeRepository;
        private readonly IEAIPVStatusRepository                  _eaIpvStatusRepository;
        private readonly IEAIPVOccurrenceRepository              _eaIpvOccurrenceRepository;
        private readonly IEAVehiclesRepository                   _eaVehiclesRepository;
        private readonly IEAFinancialNeedRepository              _eaFinancialNeedRepository;
        private readonly IEARequestStatusReasonRepository        _eaRequestStatusReasonRepository;
        private readonly IEARequestStatusRepository              _eaRequestStatusRepository;
        private readonly IConfidentialityChecker                 _confidentialityChecker;
        private readonly ICityDomain                             _cityDomain;
        private readonly IMapper                                 _mapper;
        private const    string                                  _selfCode              = "SF";
        private const    string                                  _careTakeCode          = "CTR";
        private const    string                                  _otherCareTakeCode     = "OCTR";
        private const    string                                  _impendingHomelessness = "IHL";
        private const    string                                  _homelessness          = "HLN";
        private const    string                                  _inProgress            = "IP";
        private const    string                                  _approved              = "AP";
        private const    string                                  _pending               = "PN";
        private const    string                                  _active                = "AC";
        private const    string                                  _expired               = "EX";
        private const    string                                  _overturned            = "OSM";
        private const    string                                  _energyCrisis          = "ENC";
        private const    string                                  _initial30Days         = "NNHI";
        private const    string                                  _additional30Days      = "NNHA";
        private const    decimal                                 _maxAssetsValue        = 2500.00m;

        #endregion

        #region Methods

        public EmergencyAssistanceDomain(IUnitOfWork                             unitOfWork,
                                         IAuthUser                               authUser,
                                         IWorkerRepository                       workerRepository,
                                         IOrganizationRepository                 organizationRepository,
                                         IEARequestRepository                    eaRequestRepository,
                                         IEARequestParticipantBridgeRepository   eaRequestParticipantBridgeRepository,
                                         IEARequestEmergencyTypeBridgeRepository eaRequestEmergencyTypeBridgeRepository,
                                         IEACommentRepository                    eaCommentRepository,
                                         IEACommentTypeBridgeRepository          eaCommentTypeBridgeRepository,
                                         IParticipantRepository                  participantRepository,
                                         IEARequestContactInfoRepository         eaRequestContactInfoRepository,
                                         IEAAlternateMailingAddressRepository    eaAlternateMailingAddressRepository,
                                         IEAImpendingHomelessnessRepository      eaImpendingHomelessnessRepository,
                                         IEAHomelessnessRepository               eaHomelessnessRepository,
                                         IEAEnergyCrisisRepository               eaEnergyCrisisRepository,
                                         IEAHouseHoldIncomeRepository            eaHouseHoldIncomeRepository,
                                         IEAAssetsRepository                     eaAssetsRepository,
                                         IEAIndividualTypeRepository             eaIndividualTypeRepository,
                                         IEARelationshipTypeRepository           eaRelationshipTypeRepository,
                                         IEAEmergencyTypeRepository              eaEmergencyTypeRepository,
                                         IEAStatusRepository                     eaStatusRepository,
                                         IEligibilityByFPLRepository             eligibilityByFplRepository,
                                         IEAPaymentAmountRepository              eaPaymentAmountRepository,
                                         IEAPaymentRepository                    eaPaymentRepository,
                                         IEAIPVRepository                        eaIpvRepository,
                                         IEAIPVReasonBridgeRepository            eaIpvReasonBridgeRepository,
                                         IEAIPVStatusRepository                  eaIpvStatusRepository,
                                         IEAIPVOccurrenceRepository              eaIpvOccurrenceRepository,
                                         IEAVehiclesRepository                   eaVehiclesRepository,
                                         IEAFinancialNeedRepository              eaFinancialNeedRepository,
                                         IEARequestStatusReasonRepository        eaRequestStatusReasonRepository,
                                         IEARequestStatusRepository              eaRequestStatusRepository,
                                         IConfidentialityChecker                 confidentialityChecker,
                                         ICityDomain                             cityDomain,
                                         IMapper                                 mapper)
        {
            _unitOfWork                             = unitOfWork;
            _authUser                               = authUser;
            _organizationRepository                 = organizationRepository;
            _eaRequestRepository                    = eaRequestRepository;
            _eaRequestParticipantBridgeRepository   = eaRequestParticipantBridgeRepository;
            _eaRequestEmergencyTypeBridgeRepository = eaRequestEmergencyTypeBridgeRepository;
            _eaCommentRepository                    = eaCommentRepository;
            _eaCommentTypeBridgeRepository          = eaCommentTypeBridgeRepository;
            _participantRepository                  = participantRepository;
            _eaRequestContactInfoRepository         = eaRequestContactInfoRepository;
            _eaAlternateMailingAddressRepository    = eaAlternateMailingAddressRepository;
            _eaImpendingHomelessnessRepository      = eaImpendingHomelessnessRepository;
            _eaHomelessnessRepository               = eaHomelessnessRepository;
            _eaEnergyCrisisRepository               = eaEnergyCrisisRepository;
            _eaHouseHoldIncomeRepository            = eaHouseHoldIncomeRepository;
            _eaAssetsRepository                     = eaAssetsRepository;
            _eaIndividualTypeRepository             = eaIndividualTypeRepository;
            _eaRelationshipTypeRepository           = eaRelationshipTypeRepository;
            _eaEmergencyTypeRepository              = eaEmergencyTypeRepository;
            _eaStatusRepository                     = eaStatusRepository;
            _eligibilityByFplRepository             = eligibilityByFplRepository;
            _eaPaymentAmountRepository              = eaPaymentAmountRepository;
            _eaPaymentRepository                    = eaPaymentRepository;
            _eaIpvRepository                        = eaIpvRepository;
            _eaIpvReasonBridgeRepository            = eaIpvReasonBridgeRepository;
            _eaIpvStatusRepository                  = eaIpvStatusRepository;
            _eaIpvOccurrenceRepository              = eaIpvOccurrenceRepository;
            _eaVehiclesRepository                   = eaVehiclesRepository;
            _eaFinancialNeedRepository              = eaFinancialNeedRepository;
            _eaRequestStatusReasonRepository        = eaRequestStatusReasonRepository;
            _eaRequestStatusRepository              = eaRequestStatusRepository;
            _confidentialityChecker                 = confidentialityChecker;
            _cityDomain                             = cityDomain;
            _mapper                                 = mapper;

            _convertWIUIdToName = (wiuId) =>
                                  {
                                      string wn;
                                      switch (wiuId)
                                      {
                                          case "WWP Conversion":
                                              wn = wiuId;
                                              break;
                                          case "WWP Batch":
                                              wn = wiuId;
                                              break;
                                          case "WWP":
                                              wn = wiuId;
                                              break;
                                          case "CWW":
                                              wn = wiuId;
                                              break;
                                          default:
                                          {
                                              var wo = workerRepository.GetAsQueryable()
                                                                       .Where(i => i.WIUId == wiuId)
                                                                       .Select(i => new { i.FirstName, i.MiddleInitial, i.LastName })
                                                                       .FirstOrDefault();

                                              wn = $"{wo?.FirstName} {wo?.MiddleInitial}. {wo?.LastName}".Replace(" . ", " ");
                                              break;
                                          }
                                      }

                                      return (wn);
                                  };
        }

        public ParticipantsContract GetEAGroupConfidentialParticipant(string pin)
        {
            var decimalPin = decimal.Parse(pin);
            var eaRequests = _eaRequestParticipantBridgeRepository.GetMany(i => i.Participant.PinNumber == decimalPin && !i.IsDeleted && !i.EaRequest.IsDeleted)
                                                                  .Select(i => i.EaRequest)
                                                                  .OrderByDescending(i => i.ApplicationDate)
                                                                  .ToList();
            var participants = eaRequests.SelectMany(i => i.EaRequestParticipantBridges)
                                         .Select(i => i.Participant)
                                         .ToList();

            foreach (var participant in participants)
            {
                var cwwReply           = _confidentialityChecker.Check(Convert.ToDecimal(participant.PinNumber), null, _authUser.MainFrameId);
                var isConfidentialCase = cwwReply.CaseCofidentailStatus?.ToUpper() == "Y";

                if (isConfidentialCase)
                    return ParticipantsContract.Create(
                                                       participant.Id,
                                                       participant.FirstName,
                                                       participant.MiddleInitial,
                                                       participant.LastName,
                                                       participant.SuffixName,
                                                       participant.PinNumber,
                                                       participant.DateOfBirth,
                                                       true,
                                                       false,
                                                       null,
                                                       participant.Is60DaysVerified,
                                                       participant.GenderIndicator,
                                                       eaRequests: GetRequests(pin, eaRequests));
            }

            return null;
        }

        public List<EARequestContract> GetRequests(string pin, List<EARequest> eaRequests)
        {
            var contract = new List<EARequestContract>();
            var decPin   = decimal.Parse(pin);
            eaRequests = eaRequests ?? _eaRequestParticipantBridgeRepository.GetMany(i => i.Participant.PinNumber == decPin && i.IsIncluded == true && !i.IsDeleted && !i.EaRequest.IsDeleted)
                                                                            .Select(i => i.EaRequest)
                                                                            .OrderByDescending(i => i.ApplicationDate)
                                                                            .ToList();

            eaRequests.ForEach(i =>
                               {
                                   var status        = i.EaRequestStatuses.OrderByDescending(j => j.Id).FirstOrDefault();
                                   var statusReasons = status?.EaRequestStatusReasons?.Select(j => j.StatusReason).ToList();
                                   contract.Add(
                                                new EARequestContract
                                                {
                                                    Id                    = i.Id,
                                                    RequestNumber         = i.RequestNumber,
                                                    CaresCaseNumber       = i.CaresCaseNumber,
                                                    StatusId              = status?.StatusId,
                                                    StatusCode            = status?.EaStatus?.Code,
                                                    StatusName            = status?.EaStatus?.Name,
                                                    StatusReasonIds       = statusReasons?.Select(j => (int?) j.Id).ToList(),
                                                    StatusReasonCodes     = statusReasons?.Select(j => j.Code).ToList(),
                                                    StatusReasonNames     = statusReasons?.Select(j => j.Name).ToList(),
                                                    OrganizationId        = i.OrganizationId,
                                                    OrganizationCode      = i.Organization?.EntsecAgencyCode,
                                                    OrganizationName      = i.Organization?.AgencyName,
                                                    ApprovedPaymentAmount = i.ApprovedPaymentAmount?.ToString("N2"),
                                                    EaDemographics        = GetDemographicsContract(i),
                                                    EaEmergencyType       = GetEaEmergencyTypeContract(i, i.EaRequestEmergencyTypeBridges),
                                                    EaGroupMembers = new EAGroupMembersContract
                                                                     {
                                                                         EaGroupMembers = i.EaRequestParticipantBridges.Select(earp => new EARequestParticipantContract
                                                                                                                                       {
                                                                                                                                           Id                   = earp.Id,
                                                                                                                                           PinNumber            = earp.Participant.PinNumber,
                                                                                                                                           ParticipantId        = earp.ParticipantId,
                                                                                                                                           FirstName            = earp.Participant.FirstName,
                                                                                                                                           MiddleInitial        = earp.Participant.MiddleInitial,
                                                                                                                                           LastName             = earp.Participant.LastName,
                                                                                                                                           SuffixName           = earp.Participant.SuffixName,
                                                                                                                                           ParticipantDOB       = earp.Participant.DateOfBirth,
                                                                                                                                           EARequestId          = earp.EARequestId,
                                                                                                                                           EAIndividualTypeId   = earp.EAIndividualTypeId,
                                                                                                                                           EAIndividualTypeCode = earp.EaIndividualType?.Code,
                                                                                                                                           EAIndividualTypeName = earp.EaIndividualType?.Name,
                                                                                                                                           EARelationTypeId     = earp.EARelationTypeId,
                                                                                                                                           EARelationTypeName   = earp.EaRelationshipType?.Name,
                                                                                                                                           IsIncluded           = earp.IsIncluded,
                                                                                                                                           SSNAppliedDate       = earp.SSNAppliedDate,
                                                                                                                                           SSNExemptTypeId      = earp.SSNExemptTypeId,
                                                                                                                                           SSNExemptTypeName    = earp.EaSsnExemptType?.Name,
                                                                                                                                           ModifiedBy           = _convertWIUIdToName(earp.ModifiedBy),
                                                                                                                                           ModifiedDate         = earp.ModifiedDate
                                                                                                                                       }).ToList()
                                                                     },
                                                    EaComments = i.EaComments.Select(comment => new CommentContract
                                                                                                {
                                                                                                    Id          = comment.Id,
                                                                                                    CommentText = comment.Comment,
                                                                                                    CommentTypes = comment.EaCommentTypeBridges.Select(j => new CommentTypeContract
                                                                                                                                                            {
                                                                                                                                                                CommentTypeId   = j.CommentTypeId,
                                                                                                                                                                CommentTypeName = j.EaCommentType.Name,
                                                                                                                                                                IsSystemUseOnly = j.EaCommentType.IsSystemUseOnly
                                                                                                                                                            }).ToList(),
                                                                                                    IsEdited     = comment.IsEdited,
                                                                                                    CreatedDate  = comment.CreatedDate,
                                                                                                    ModifiedBy   = _convertWIUIdToName(comment.ModifiedBy),
                                                                                                    ModifiedDate = comment.ModifiedDate,
                                                                                                    WIUID        = comment.ModifiedBy
                                                                                                }).ToList()
                                                });
                               });

            return contract;
        }

        public EARequestContract GetRequest(int id, EARequest request = null)
        {
            request = request ?? _eaRequestRepository.Get(i => i.Id == id);

            var status               = request.EaRequestStatuses.OrderByDescending(i => i.Id).FirstOrDefault();
            var emergencyTypeBridges = request.EaRequestEmergencyTypeBridges;
            var requestParticipants  = request.EaRequestParticipantBridges;
            var houseHoldIncome      = request.EaHouseHoldIncomes;
            var assets               = request.EaAssetses;
            var vehicles             = request.EaVehicleses;
            var statusReasons        = status?.EaRequestStatusReasons?.Select(i => i.StatusReason).ToList();
            var deadLineReasons      = new List<string> { _initial30Days, _additional30Days };
            var contract             = _mapper.Map<EARequestContract>(request);

            contract.StatusId          = status?.StatusId;
            contract.StatusCode        = status?.EaStatus?.Code;
            contract.StatusName        = status?.EaStatus?.Name;
            contract.StatusReasonIds   = statusReasons?.Select(i => (int?) i.Id).ToList();
            contract.StatusReasonCodes = statusReasons?.Select(i => i.Code).ToList();
            contract.StatusReasonNames = statusReasons?.Select(i => i.Name).ToList();
            contract.StatusLastUpdated = status?.ModifiedDate;
            contract.StatusDeadLine = status?.EaStatus?.Code == _inProgress || (statusReasons != null && statusReasons.Any(i => deadLineReasons.Contains(i.Code)))
                                          ? status.StatusDeadLineDate
                                          : null;
            contract.OrganizationId        = request.OrganizationId;
            contract.OrganizationCode      = request.Organization?.EntsecAgencyCode;
            contract.OrganizationName      = request.Organization?.AgencyName;
            contract.EaDemographics        = GetDemographicsContract(request);
            contract.EaEmergencyType       = GetEaEmergencyTypeContract(request, emergencyTypeBridges);
            contract.EaGroupMembers        = GetEaGroupMembers(request, requestParticipants);
            contract.EaHouseHoldFinancials = GetEaHouseHoldFinancials(request, houseHoldIncome, assets, vehicles);
            contract.EaAgencySummary       = GetAgencySummaryContract(request, emergencyTypeBridges, requestParticipants, houseHoldIncome, assets, vehicles);
            contract.EaComments            = GetComments(request.EaComments.OrderByDescending(i => i.ModifiedDate));
            contract.EaPayments            = GetPayments(request.EaPayments).OrderByDescending(i => i.VoucherOrCheckDate).ToList();
            return contract;
        }

        private FinalistAddressContract GetMailingAddressContract(EAAlternateMailingAddress mailingAddress)
        {
            return new FinalistAddressContract
                   {
                       AddressLine1 = mailingAddress?.AddressLine1,
                       City         = mailingAddress?.City?.Name,
                       State        = mailingAddress?.City?.State?.Code,
                       Zip          = mailingAddress?.ZipCode
                   };
        }

        private EARequestParticipantBridge RecentActiveApplicationInLastYear(IEnumerable<int> includedCrIds, int requestId = 0)
        {
            var pastYearDate = DateTime.Today.AddYears(-1);
            var bridge =  _eaRequestParticipantBridgeRepository.GetMany(i => includedCrIds.Contains(i.ParticipantId) && i.EARequestId                                  != requestId && i.EaRequest.ApplicationDate >= pastYearDate && i.IsIncluded == true &&
                                                                             i.EaRequest.EaRequestStatuses.OrderByDescending(j => j.Id).FirstOrDefault().EaStatus.Code == _approved &&
                                                                             (i.EaIndividualType.Code    == _careTakeCode
                                                                              || i.EaIndividualType.Code == _otherCareTakeCode))
                                                               .OrderByDescending(i => i.EaRequest.ApplicationDate)
                                                               .FirstOrDefault();

            return bridge;
        }

        private void GenerateNoticeTrigger(string pin, int id, bool isIpv, string notes)
        {
            var parms = new Dictionary<string, object>
                        {
                            ["PinNumber"] = pin,
                            ["Id"]        = id,
                            ["IsIPV"]     = isIpv,
                            ["MFUserId"]  = _authUser.MainFrameId,
                            ["Notes"]     = notes ?? (object) DBNull.Value
                        };

            _eaRequestRepository.GetStoredProcReturnValue("USP_Generate_EA_Trigger_Notice", parms);
        }

        #endregion
    }
}
