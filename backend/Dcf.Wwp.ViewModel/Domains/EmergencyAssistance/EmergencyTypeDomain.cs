using System;
using System.Collections.Generic;
using System.Linq;
using DCF.Common.Extensions;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.Api.Library.Contracts.EmergencyAssistance;
using Dcf.Wwp.DataAccess.Models;
using Dcf.Wwp.Model.Interface.Constants;
using EAEmergencyType = Dcf.Wwp.Model.Interface.Constants.EAEmergencyType;

namespace Dcf.Wwp.Api.Library.Domains.EmergencyAssistance
{
    public partial class EmergencyAssistanceDomain
    {
        #region Properties

        #endregion

        #region Methods

        private EAEmergencyTypeContract GetEaEmergencyTypeContract(EARequest request, ICollection<EARequestEmergencyTypeBridge> emergencyTypeBridges)
        {
            emergencyTypeBridges = emergencyTypeBridges?.Where(i => !i.IsDeleted).ToList();

            var contract = new EAEmergencyTypeContract { RequestId = request.Id, EmergencyDetails = request.EmergencyDetails };

            if (emergencyTypeBridges == null || emergencyTypeBridges.Count == 0) return contract;

            contract.EmergencyTypeIds   = emergencyTypeBridges.Select(i => i.EmergencyTypeId).ToList();
            contract.EmergencyTypeCodes = emergencyTypeBridges.Select(i => i.EaEmergencyType?.Code).ToList();
            contract.EmergencyTypeNames = emergencyTypeBridges.OrderBy(i => i.EaEmergencyType?.SortOrder).Select(i => i.EaEmergencyType?.Name).ToList();

            emergencyTypeBridges.ForEach(i =>
                                         {
                                             switch (i.EaEmergencyType.Code)
                                             {
                                                 case EAEmergencyType.IHL:
                                                     contract.EaImpendingHomelessness = GetImpendingHomelessnessContract(request);
                                                     break;
                                                 case EAEmergencyType.HLN:
                                                     contract.EaHomelessness = GetHomelessnessContract(request);
                                                     break;
                                                 case EAEmergencyType.ENC:
                                                     contract.EaEnergyCrisis = GetEnergyCrisisContract(request);
                                                     break;
                                             }
                                         });

            var latestEmergencyType = emergencyTypeBridges.OrderByDescending(i => i.ModifiedDate).First();

            contract.ModifiedBy               = _convertWIUIdToName(latestEmergencyType.ModifiedBy);
            contract.ModifiedDate             = latestEmergencyType.ModifiedDate;
            contract.IsSubmittedViaDriverFlow = true;

            return contract;
        }

        private EAImpendingHomelessnessContract GetImpendingHomelessnessContract(EARequest request)
        {
            var impendingHomelessness = request.EaImpendingHomelessnesses.LastOrDefault();
            var contract              = new EAImpendingHomelessnessContract();
            var emergencyType         = request.EaRequestEmergencyTypeBridges.FirstOrDefault(i => i.EaEmergencyType?.Code == _impendingHomelessness);

            contract.EmergencyTypeReasonId   = emergencyType?.EmergencyTypeReasonId;
            contract.EmergencyTypeReasonName = emergencyType?.EaEmergencyTypeReason?.Name;

            if (impendingHomelessness == null) return contract;

            contract.HaveEvictionNotice                       = impendingHomelessness.HaveEvictionNotice;
            contract.DateOfEvictionNotice                     = impendingHomelessness.DateOfEvictionNotice?.ToString("MM/dd/yyyy");
            contract.DifficultToPayDetails                    = impendingHomelessness.DifficultToPayDetails;
            contract.IsCurrentLandLordUnknown                 = impendingHomelessness.IsCurrentLandLordUnknown;
            contract.LandLordName                             = impendingHomelessness.LandLordName;
            contract.ContactPerson                            = impendingHomelessness.ContactPerson;
            contract.LandLordPhone                            = impendingHomelessness.LandLordPhone;
            contract.NeedingDifferentHomeForAbuse             = impendingHomelessness.NeedingDifferentHomeForAbuse;
            contract.NeedingDifferentHomeForRentalForeclosure = impendingHomelessness.NeedingDifferentHomeForRentalForeclosure;
            contract.DateOfFamilyDeparture                    = impendingHomelessness.DateOfFamilyDeparture?.ToString("MM/dd/yyyy");
            contract.IsYourBuildingDecidedUnSafe              = impendingHomelessness.IsYourBuildingDecidedUnSafe;
            contract.DateBuildingWasDecidedUnSafe             = impendingHomelessness.DateBuildingWasDecidedUnSafe?.ToString("MM/dd/yyyy");
            contract.IsInspectionReportAvailable              = impendingHomelessness.IsInspectionReportAvailable;

            contract.LandLordAddress = new FinalistAddressContract
                                       {
                                           AddressLine1 = impendingHomelessness.LandLordAddress,
                                           City         = impendingHomelessness.City?.Name,
                                           State        = impendingHomelessness.City?.State?.Code,
                                           Zip          = impendingHomelessness.LandLordZip
                                       };


            return contract;
        }

        private EAHomelessnessContract GetHomelessnessContract(EARequest request)
        {
            var homelessness  = request.EaHomelessnesses.LastOrDefault();
            var contract      = new EAHomelessnessContract();
            var emergencyType = request.EaRequestEmergencyTypeBridges.FirstOrDefault(i => i.EaEmergencyType?.Code == _homelessness);

            contract.EmergencyTypeReasonId   = emergencyType?.EmergencyTypeReasonId;
            contract.EmergencyTypeReasonName = emergencyType?.EaEmergencyTypeReason?.Name;

            if (homelessness == null) return contract;

            contract.InLackOfPlace                = homelessness.InLackOfPlace;
            contract.DateOfStart                  = homelessness.DateOfStart?.ToString("MM/dd/yyyy");
            contract.PlanOnPermanentPlace         = homelessness.PlanOnPermanentPlace;
            contract.InShelterForDomesticAbuse    = homelessness.InShelterForDomesticAbuse;
            contract.IsYourBuildingDecidedUnSafe  = homelessness.IsYourBuildingDecidedUnSafe;
            contract.DateBuildingWasDecidedUnSafe = homelessness.DateBuildingWasDecidedUnSafe?.ToString("MM/dd/yyyy");
            contract.IsInspectionReportAvailable  = homelessness.IsInspectionReportAvailable;

            return contract;
        }

        private EAEnergyCrisisContract GetEnergyCrisisContract(EARequest request)
        {
            var energyCrisis = request.EaEnergyCrises.LastOrDefault();
            var contract     = new EAEnergyCrisisContract();

            if (energyCrisis == null) return contract;

            contract.InNeedForUtilities       = energyCrisis.InNeedForUtilities;
            contract.DifficultyForUtilityBill = energyCrisis.DifficultyForUtilityBill;
            contract.ExistingAppliedHelp      = energyCrisis.ExistingAppliedHelp;
            contract.HaveThreat               = energyCrisis.HaveThreat;

            return contract;
        }

        public EAEmergencyTypeContract UpsertEmergencyType(EAEmergencyTypeContract contract)
        {
            var modifiedBy           = _authUser.WIUID;
            var modifiedDate         = DateTime.Now;
            var request              = _eaRequestRepository.Get(i => i.Id == contract.RequestId && !i.IsDeleted);
            var emergencyTypeBridges = request.EaRequestEmergencyTypeBridges?.Where(i => !i.IsDeleted).ToList();

            emergencyTypeBridges?.Where(i => contract.EmergencyTypeIds != null && !contract.EmergencyTypeIds.Contains(i.EmergencyTypeId))
                                .ForEach(i =>
                                         {
                                             i.IsDeleted = true;

                                             switch (i.EaEmergencyType.Code)
                                             {
                                                 case EAEmergencyType.IHL:
                                                     if (request.EaImpendingHomelessnesses.LastOrDefault() != null)
                                                         request.EaImpendingHomelessnesses.Last().IsDeleted = true;
                                                     break;
                                                 case EAEmergencyType.HLN:
                                                     if (request.EaHomelessnesses.LastOrDefault() != null)
                                                         request.EaHomelessnesses.Last().IsDeleted = true;
                                                     break;
                                                 case EAEmergencyType.ENC:
                                                     if (request.EaEnergyCrises.LastOrDefault() != null)
                                                         request.EaEnergyCrises.Last().IsDeleted = true;
                                                     break;
                                             }
                                         });

            contract.EmergencyTypeIds
                    ?.ForEach(i =>
                              {
                                  var emergencyTypeBridge = emergencyTypeBridges?.FirstOrDefault(j => j.EmergencyTypeId == i && !j.IsDeleted) ?? _eaRequestEmergencyTypeBridgeRepository.New();

                                  emergencyTypeBridge.EaRequest       = request;
                                  emergencyTypeBridge.EaEmergencyType = _eaEmergencyTypeRepository.GetById(i);
                                  emergencyTypeBridge.IsDeleted       = false;
                                  emergencyTypeBridge.ModifiedBy      = modifiedBy;
                                  emergencyTypeBridge.ModifiedDate    = modifiedDate;

                                  if (emergencyTypeBridge.Id == 0)
                                      request.EaRequestEmergencyTypeBridges.Add(emergencyTypeBridge);
                              });

            request.EaRequestEmergencyTypeBridges
                   ?.Where(i => !i.IsDeleted)
                   .ForEach(i =>
                            {
                                switch (i.EaEmergencyType.Code)
                                {
                                    case EAEmergencyType.IHL:
                                        UpsertEaImpendingHomelessness(request, contract.EaImpendingHomelessness, modifiedBy, modifiedDate);
                                        break;
                                    case EAEmergencyType.HLN:
                                        UpsertEaHomelessness(request, contract.EaHomelessness, modifiedBy, modifiedDate);
                                        break;
                                    case EAEmergencyType.ENC:
                                        UpsertEaEnergyCrisis(request, contract.EaEnergyCrisis, modifiedBy, modifiedDate);
                                        break;
                                }
                            });

            request.EmergencyDetails = contract.EmergencyDetails;
            request.ModifiedBy       = modifiedBy;
            request.ModifiedDate     = modifiedDate;

            _unitOfWork.Commit();

            return GetEaEmergencyTypeContract(request, request.EaRequestEmergencyTypeBridges);
        }

        private void UpsertEaImpendingHomelessness(EARequest request, EAImpendingHomelessnessContract contract, string modifiedBy, DateTime modifiedDate)
        {
            var impendingHomelessness = request.EaImpendingHomelessnesses.LastOrDefault(i => !i.IsDeleted) ?? _eaImpendingHomelessnessRepository.New();

            impendingHomelessness.RequestId                                = request.Id;
            impendingHomelessness.HaveEvictionNotice                       = contract.HaveEvictionNotice;
            impendingHomelessness.DateOfEvictionNotice                     = contract.DateOfEvictionNotice.ToValidDateTimeMonthDayYear();
            impendingHomelessness.DifficultToPayDetails                    = contract.DifficultToPayDetails;
            impendingHomelessness.IsCurrentLandLordUnknown                 = contract.IsCurrentLandLordUnknown;
            impendingHomelessness.LandLordName                             = contract.LandLordName;
            impendingHomelessness.ContactPerson                            = contract.ContactPerson;
            impendingHomelessness.LandLordPhone                            = contract.LandLordPhone;
            impendingHomelessness.NeedingDifferentHomeForAbuse             = contract.NeedingDifferentHomeForAbuse;
            impendingHomelessness.NeedingDifferentHomeForRentalForeclosure = contract.NeedingDifferentHomeForRentalForeclosure;
            impendingHomelessness.DateOfFamilyDeparture                    = contract.DateOfFamilyDeparture.ToValidDateTimeMonthDayYear();
            impendingHomelessness.IsYourBuildingDecidedUnSafe              = contract.IsYourBuildingDecidedUnSafe;
            impendingHomelessness.DateBuildingWasDecidedUnSafe             = contract.DateBuildingWasDecidedUnSafe.ToValidDateTimeMonthDayYear();
            impendingHomelessness.IsInspectionReportAvailable              = contract.IsInspectionReportAvailable;
            impendingHomelessness.ModifiedBy                               = modifiedBy;
            impendingHomelessness.ModifiedDate                             = modifiedDate;

            if (impendingHomelessness.IsCurrentLandLordUnknown != true && !string.IsNullOrWhiteSpace(contract.LandLordAddress?.City))
            {
                impendingHomelessness.City = _cityDomain.GetOrCreateCity(user: _authUser.Username, finalistAddress: contract.LandLordAddress, isClientReg: true);

                // In case we have a city that was deleted, we need to restore it.
                impendingHomelessness.City.IsDeleted                  = false;
                impendingHomelessness.LandLordAddress                 = contract.LandLordAddress.AddressLine1;
                impendingHomelessness.LandLordZip                     = contract.LandLordAddress.Zip;
                impendingHomelessness.AddressVerificationTypeLookupId = contract.LandLordAddress.UseSuggestedAddress ? AddressVerificationType.FinalistVerifiedId : AddressVerificationType.WorkerOverrideId;
            }
            else
            {
                impendingHomelessness.LandLordCityId                  = null;
                impendingHomelessness.LandLordAddress                 = contract.LandLordAddress?.AddressLine1;
                impendingHomelessness.LandLordZip                     = contract.LandLordAddress?.Zip;
                impendingHomelessness.AddressVerificationTypeLookupId = null;
            }

            if (impendingHomelessness.Id == 0)
                _eaImpendingHomelessnessRepository.Add(impendingHomelessness);
        }

        private void UpsertEaHomelessness(EARequest request, EAHomelessnessContract contract, string modifiedBy, DateTime modifiedDate)
        {
            var homelessness = request.EaHomelessnesses.LastOrDefault(i => !i.IsDeleted) ?? _eaHomelessnessRepository.New();

            homelessness.RequestId                    = request.Id;
            homelessness.InLackOfPlace                = contract.InLackOfPlace;
            homelessness.DateOfStart                  = contract.DateOfStart.ToValidDateTimeMonthDayYear();
            homelessness.PlanOnPermanentPlace         = contract.PlanOnPermanentPlace;
            homelessness.InShelterForDomesticAbuse    = contract.InShelterForDomesticAbuse;
            homelessness.IsYourBuildingDecidedUnSafe  = contract.IsYourBuildingDecidedUnSafe;
            homelessness.DateBuildingWasDecidedUnSafe = contract.DateBuildingWasDecidedUnSafe.ToValidDateTimeMonthDayYear();
            homelessness.IsInspectionReportAvailable  = contract.IsInspectionReportAvailable;
            homelessness.ModifiedBy                   = modifiedBy;
            homelessness.ModifiedDate                 = modifiedDate;

            if (homelessness.Id == 0)
                _eaHomelessnessRepository.Add(homelessness);
        }

        private void UpsertEaEnergyCrisis(EARequest request, EAEnergyCrisisContract contract, string modifiedBy, DateTime modifiedDate)
        {
            var energyCrisis = request.EaEnergyCrises.LastOrDefault(i => !i.IsDeleted) ?? _eaEnergyCrisisRepository.New();

            energyCrisis.RequestId                = request.Id;
            energyCrisis.InNeedForUtilities       = contract.InNeedForUtilities;
            energyCrisis.DifficultyForUtilityBill = contract.DifficultyForUtilityBill;
            energyCrisis.ExistingAppliedHelp      = contract.ExistingAppliedHelp;
            energyCrisis.HaveThreat               = contract.HaveThreat;
            energyCrisis.ModifiedBy               = modifiedBy;
            energyCrisis.ModifiedDate             = modifiedDate;

            if (energyCrisis.Id == 0)
                _eaEnergyCrisisRepository.Add(energyCrisis);
        }

        #endregion
    }
}
