using System;
using System.Collections.Generic;
using System.Linq;
using DCF.Common.Exceptions;
using Dcf.Wwp.Api.Library.Contracts.EmergencyAssistance;
using Dcf.Wwp.Api.Library.Extensions;
using Dcf.Wwp.DataAccess.Models;

namespace Dcf.Wwp.Api.Library.Domains.EmergencyAssistance
{
    public partial class EmergencyAssistanceDomain
    {
        #region Properties

        #endregion

        #region Methods

        public EAAgencySummaryContract GetEARequestAgencySummary(int id)
        {
            var request = _eaRequestRepository.Get(i => i.Id == id);

            return GetAgencySummaryContract(request, request.EaRequestEmergencyTypeBridges, request.EaRequestParticipantBridges, request.EaHouseHoldIncomes, request.EaAssetses, request.EaVehicleses);
        }

        private EAAgencySummaryContract GetAgencySummaryContract(EARequest                                 request,
                                                                 ICollection<EARequestEmergencyTypeBridge> emergencyTypeBridges,
                                                                 ICollection<EARequestParticipantBridge>   requestParticipants,
                                                                 ICollection<EAHouseHoldIncome>            houseHoldIncomes,
                                                                 ICollection<EAAssets>                     assets,
                                                                 ICollection<EAVehicles>                   vehicles)
        {
            emergencyTypeBridges = emergencyTypeBridges?.Where(i => !i.IsDeleted).ToList();
            requestParticipants  = requestParticipants?.Where(i => !i.IsDeleted).ToList();
            houseHoldIncomes     = houseHoldIncomes?.Where(i => !i.IsDeleted).ToList();
            assets               = assets?.Where(i => !i.IsDeleted).ToList();
            vehicles             = vehicles?.Where(i => !i.IsDeleted).ToList();

            var status = request.EaRequestStatuses.OrderByDescending(i => i.Id).FirstOrDefault();

            var groupSize = requestParticipants == null || requestParticipants.Count == 0
                                ? 0
                                : requestParticipants.Count(i => i.IsIncluded == true);

            var maxPaymentAmount = emergencyTypeBridges != null && emergencyTypeBridges.Count > 0 && emergencyTypeBridges.All(i => i.EaEmergencyType.Code == _energyCrisis)
                                       ? _eaPaymentAmountRepository.Get(i => i.EaEmergencyType.Code == _energyCrisis)?.MaxPaymentAmount
                                       : groupSize < 6
                                           ? _eaPaymentAmountRepository.Get(i => groupSize >= i.MinGroupSize && groupSize <= i.MaxGroupSize && i.MaxGroupSize != 999999999)?.MaxPaymentAmount
                                           : _eaPaymentAmountRepository.Get(i => i.MinGroupSize == 6 && i.MaxGroupSize == 999999999)?.AmountPerMember * groupSize;

            var totalIncome = houseHoldIncomes == null || houseHoldIncomes.Count == 0
                                  ? 0.00m
                                  : houseHoldIncomes.Sum(i => i.MonthlyIncome ?? 0.00m);

            var totalFinancialAssets = assets == null || assets.Count == 0
                                           ? 0.00m
                                           : assets.Sum(i => i.CurrentValue ?? 0.00m);

            var totalVehicleEquity = vehicles == null || vehicles.Count == 0
                                         ? 0.00m
                                         : vehicles.Sum(i => (i.VehicleValue.GetValueOrDefault() - i.AmountOwed.GetValueOrDefault() > 0
                                                                  ? i.VehicleValue.GetValueOrDefault() - i.AmountOwed.GetValueOrDefault()
                                                                  : 0.00m));

            var totalVehicleAssets = totalVehicleEquity - 10000 >= 0 ? totalVehicleEquity - 10000 : 0.00m;

            var totalAssets = decimal.Add(totalFinancialAssets, totalVehicleAssets);

            var pct115PerMonthByGroupSize = _eligibilityByFplRepository.Get(i => i.GroupSize == groupSize
                                                                                 && !i.IsDeleted
                                                                                 && (i.EndDate == null || i.EndDate >= DateTime.Today)
                                                                                 && i.EffectiveDate <= DateTime.Today)
                                                                       ?.Pct115PerMonth;

            var hasFinancialEligibilityPassed = (totalIncome    == 0 || totalIncome <= pct115PerMonthByGroupSize)
                                                && (totalAssets == 0 || totalAssets <= _maxAssetsValue);

            var selfParticipantId = requestParticipants?.FirstOrDefault(i => i.EaRelationshipType.Code == _selfCode)?.ParticipantId;
            var currentCRIds =  _eaRequestParticipantBridgeRepository.GetMany(i => (i.ParticipantId         == selfParticipantId || i.EARequestId           == request.Id) && i.IsIncluded == true &&
                                                                                   (i.EaIndividualType.Code == _careTakeCode     || i.EaIndividualType.Code == _otherCareTakeCode))
                                                                     .Select(i => i.ParticipantId)
                                                                     .ToList();
            var ipvs          = _eaIpvRepository.GetMany(i => currentCRIds.Contains(i.ParticipantId)).ToList();
            var statusReasons = status?.EaRequestStatusReasons?.Select(i => i.StatusReason).ToList();

            var contract = new EAAgencySummaryContract
                           {
                               GroupSize                     = groupSize,
                               TotalIncome                   = totalIncome.ToString("N2"),
                               TotalFinancialAssets          = totalFinancialAssets.ToString("N2"),
                               TotalVehicleAssets            = totalVehicleAssets.ToString("N2"),
                               TotalAssets                   = totalAssets.ToString("N2"),
                               MaxPaymentAmount              = maxPaymentAmount ?? 0.00m,
                               ApprovedPaymentsPast12Months  = RecentActiveApplicationInLastYear(currentCRIds, request.Id) != null,
                               HasActiveIPV                  = ipvs.Any(i => i.Status.Code == _active),
                               HasPendingIPV                 = ipvs.Any(i => i.Status.Code == _pending),
                               EaEmergencyType               = GetEaEmergencyTypeContract(request, emergencyTypeBridges),
                               StatusId                      = status?.StatusId,
                               StatusName                    = status?.EaStatus?.Name,
                               StatusReasonIds               = statusReasons?.Select(i => (int?) i.Id).ToList(),
                               StatusReasonCodes             = statusReasons?.Select(i => i.Code).ToList(),
                               StatusReasonNames             = statusReasons?.Select(i => i.Name).ToList(),
                               ApprovedPaymentAmount         = request.ApprovedPaymentAmount?.ToString("N2"),
                               HasFinancialEligibilityPassed = hasFinancialEligibilityPassed,
                               HasComment                    = request.EaComments.AsNotNull().Any(),
                               ModifiedBy                    = _convertWIUIdToName(request.ModifiedBy),
                               ModifiedDate                  = request.ModifiedDate,
                               IsSubmittedViaDriverFlow = status?.EaStatus?.Code           != _inProgress
                                                          || request.ApprovedPaymentAmount != null
                                                          || emergencyTypeBridges       != null
                                                          && emergencyTypeBridges.Count > 0 && emergencyTypeBridges.Any(i => i.EmergencyTypeReasonId != null),
                               EaFinancialNeeds = request.EaFinancialNeeds
                                                         .Where(i => !i.IsDeleted)
                                                         .Select(i => new EAFinancialNeedContract
                                                                      {
                                                                          Id                    = i.Id,
                                                                          Amount                = i.Amount?.ToString("N2"),
                                                                          FinancialNeedTypeId   = i.FinancialNeedTypeId,
                                                                          FinancialNeedTypeName = i.EaFinancialNeedType?.Name
                                                                      }).ToList()
                           };

            return contract;
        }

        public EAAgencySummaryContract UpsertAgencySummary(string pin, EAAgencySummaryContract contract)
        {
            var modifiedBy                         = _authUser.WIUID;
            var modifiedDate                       = DateTime.Now;
            var request                            = _eaRequestRepository.Get(i => i.Id == contract.RequestId && !i.IsDeleted);
            var status                             = request.EaRequestStatuses?.OrderByDescending(i => i.Id).FirstOrDefault();
            var emergencyTypeBridges               = request.EaRequestEmergencyTypeBridges;
            var impendingHomelessnessEmergencyType = emergencyTypeBridges.FirstOrDefault(i => i.EaEmergencyType?.Code == _impendingHomelessness);
            var homelessnessEmergencyType          = emergencyTypeBridges.FirstOrDefault(i => i.EaEmergencyType?.Code == _homelessness);
            var isNewStatusApproved                = false;

            if (impendingHomelessnessEmergencyType != null && contract.EaEmergencyType?.EaImpendingHomelessness != null)
            {
                impendingHomelessnessEmergencyType.EmergencyTypeReasonId = contract.EaEmergencyType.EaImpendingHomelessness.EmergencyTypeReasonId;
                impendingHomelessnessEmergencyType.ModifiedBy            = modifiedBy;
                impendingHomelessnessEmergencyType.ModifiedDate          = modifiedDate;
            }

            if (homelessnessEmergencyType != null && contract.EaEmergencyType?.EaHomelessness != null)
            {
                homelessnessEmergencyType.EmergencyTypeReasonId = contract.EaEmergencyType.EaHomelessness.EmergencyTypeReasonId;
                homelessnessEmergencyType.ModifiedBy            = modifiedBy;
                homelessnessEmergencyType.ModifiedDate          = modifiedDate;
            }

            request.ApprovedPaymentAmount = contract.ApprovedPaymentAmount.ToDecimal();
            request.ModifiedBy            = modifiedBy;
            request.ModifiedDate          = modifiedDate;

            var eaFinancialNeeds = request.EaFinancialNeeds?.Where(i => !i.IsDeleted).ToList();

            eaFinancialNeeds?.ForEach(i => i.IsDeleted = true);

            contract.EaFinancialNeeds?.ForEach(i =>
                                              {
                                                  var financialNeed = eaFinancialNeeds?.FirstOrDefault(j => j.Id == i.Id) ?? _eaFinancialNeedRepository.New();

                                                  if (financialNeed == null) return;
                                                  financialNeed.EaRequest           = request;
                                                  financialNeed.Amount              = i.Amount.ToDecimalNull();
                                                  financialNeed.FinancialNeedTypeId = i.FinancialNeedTypeId;
                                                  financialNeed.IsDeleted           = false;
                                                  financialNeed.ModifiedBy          = modifiedBy;
                                                  financialNeed.ModifiedDate        = modifiedDate;

                                                  if (i.Id == 0)
                                                      request.EaFinancialNeeds.Add(financialNeed);
                                              });

            if (status != null && (status.StatusId != contract.StatusId || !status.EaRequestStatusReasons.Select(i => (int?) i.StatusReasonId).OrderBy(i => i)
                                                                                  .SequenceEqual(contract.StatusReasonIds.OrderBy(i => i))))
            {
                var newStatus           = _eaStatusRepository.Get(i => i.Id                   == contract.StatusId);
                var newStatusReasonCode = newStatus.EaStatusReasons?.FirstOrDefault(i => i.Id == contract.StatusReasonIds?.FirstOrDefault())?.Code;
                var newStatsDeadLine = newStatus.Code == _pending
                                           ? newStatusReasonCode == _initial30Days
                                                 ? DateTime.Today.AddDays(30)
                                                 : newStatusReasonCode == _additional30Days
                                                     ? status.StatusDeadLineDate?.AddDays(30)
                                                     : null
                                           : null;

                var newRequestStatus = _eaRequestStatusRepository.New();

                newRequestStatus.EaRequest          = request;
                newRequestStatus.EaStatus           = newStatus;
                newRequestStatus.StatusDeadLineDate = newStatsDeadLine;
                newRequestStatus.Notes              = contract.Notes;
                newRequestStatus.ModifiedBy         = modifiedBy;
                newRequestStatus.ModifiedDate       = modifiedDate;

                contract.StatusReasonIds.ForEach(i =>
                                                 {
                                                     var requestStatusReason = _eaRequestStatusReasonRepository.New();

                                                     requestStatusReason.EaRequestStatus = newRequestStatus;
                                                     requestStatusReason.StatusReasonId  = i.GetValueOrDefault();
                                                     requestStatusReason.ModifiedBy      = modifiedBy;
                                                     requestStatusReason.ModifiedDate    = modifiedDate;

                                                     _eaRequestStatusReasonRepository.Add(requestStatusReason);
                                                 });

                _eaRequestStatusRepository.Add(newRequestStatus);

                isNewStatusApproved = newStatus.Code == _approved;
            }

            AgencySummaryTransactionalSave(pin, status?.EaStatus.Code == _approved || isNewStatusApproved, request, contract, modifiedBy, modifiedDate);

            return GetAgencySummaryContract(request, emergencyTypeBridges, request.EaRequestParticipantBridges, request.EaHouseHoldIncomes, request.EaAssetses, request.EaVehicleses);
        }

        private void AgencySummaryTransactionalSave(string pin, bool updateIpv, EARequest request, EAAgencySummaryContract contract, string modifiedBy, DateTime modifiedDate)
        {
            using (var tx = _eaRequestRepository.GetDataBase().BeginTransaction())
            {
                try
                {
                    _unitOfWork.Commit();

                    if (updateIpv)
                    {
                        var selfParticipantId = request.EaRequestParticipantBridges?.FirstOrDefault(i => i.EaRelationshipType.Code == _selfCode)?.ParticipantId;
                        var currentCRIds = _eaRequestParticipantBridgeRepository.GetMany(i => (i.ParticipantId         == selfParticipantId || i.EARequestId           == request.Id) && i.IsIncluded == true &&
                                                                                              (i.EaIndividualType.Code == _careTakeCode     || i.EaIndividualType.Code == _otherCareTakeCode))
                                                                                .Select(i => i.ParticipantId)
                                                                                .ToList();
                        var ipvs = _eaIpvRepository.GetMany(i => currentCRIds.Contains(i.ParticipantId)).Where(i => i.Status.Code == _pending).ToList();

                        ipvs.ForEach(i =>
                                     {
                                         if (i.Status.Code != _pending) return;
                                         SetPenaltyStartEndDatesAndStatus(i, null, currentCRIds, modifiedDate);
                                         i.ModifiedBy   = modifiedBy;
                                         i.ModifiedDate = modifiedDate;
                                     });

                        _unitOfWork.Commit();
                    }

                    if (contract.IsSubmit == true)
                        GenerateNoticeTrigger(pin, request.Id, false, contract.Notes);

                    tx.Commit();
                }
                catch (Exception ex)
                {
                    tx.Dispose();
                    throw new DCFApplicationException("Failed to save. Please try again.", ex);
                }

                tx.Dispose();
            }
        }
    }

    #endregion
}
