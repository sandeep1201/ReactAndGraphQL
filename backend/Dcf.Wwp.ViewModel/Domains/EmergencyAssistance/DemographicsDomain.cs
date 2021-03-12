using System;
using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.Api.Library.Contracts.EmergencyAssistance;
using Dcf.Wwp.DataAccess.Models;
using Dcf.Wwp.Model.Interface.Constants;

namespace Dcf.Wwp.Api.Library.Domains.EmergencyAssistance
{
    public partial class EmergencyAssistanceDomain
    {
        #region Properties

        #endregion

        #region Methods

        private EADemographicsContract GetDemographicsContract(EARequest request)
        {
            var contactInfo = request.EaRequestContactInfos.LastOrDefault();

            var contract = new EADemographicsContract
                           {
                               RequestId                       = request.Id,
                               ApplicationDate                 = request.ApplicationDate,
                               CaresCaseNumber                 = request.CaresCaseNumber,
                               DidApplicantTakeCareOfAnyChild  = request.DidApplicantTakeCareOfAnyChild,
                               WillTheChildStayInApplicantCare = request.WillTheChildStayInApplicantCare,
                               ApplicationInitiatedMethodId    = request.ApplicationInitiatedMethodId,
                               ApplicationInitiatedMethodCode  = request.EaApplicationInitiationMethodLookUp?.Code,
                               ApplicationInitiatedMethodName  = request.EaApplicationInitiationMethodLookUp?.Name,
                               AccessTrackingNumber            = request.AccessTrackingNumber,
                               ModifiedBy                      = _convertWIUIdToName(request.ModifiedBy),
                               ModifiedDate                    = request.ModifiedDate,
                               IsSubmittedViaDriverFlow        = true
                           };

            if (contactInfo == null) return contract;
            contract.EaDemographicsContact = GetContactInfoContract(contactInfo);

            return contract;
        }

        private EADemographicsContactContract GetContactInfoContract(EARequestContactInfo contactInfo)
        {
            var contract = new EADemographicsContactContract
                           {
                               PhoneNumber                     = contactInfo.PhoneNumber,
                               CanText                         = contactInfo.CanTextPhone,
                               AlternatePhoneNumber            = contactInfo.AlternatePhoneNumber,
                               CanTextAlternate                = contactInfo.CanTextAlternatePhone,
                               BestWayToReach                  = contactInfo.BestWayToReach,
                               EmailAddress                    = contactInfo.EmailAddress,
                               CountyOfResidenceId             = contactInfo.CountyOfResidenceId,
                               CountyOfResidenceName           = contactInfo.CountyAndTribe?.CountyName,
                               IsHomeless                      = contactInfo.HomelessIndicator,
                               IsMailingSameAsHouseholdAddress = contactInfo.IsHouseHoldMailingAddressSame,
                               HouseholdAddress = new FinalistAddressContract
                                                  {
                                                      AddressLine1 = contactInfo.AddressLine1,
                                                      City         = contactInfo.City?.Name,
                                                      State        = contactInfo.City?.State?.Code,
                                                      Zip          = contactInfo.ZipCode
                                                  }
                           };

            if (contactInfo.EAAlternateMailingAddress != null)
            {
                contract.MailingAddress = GetMailingAddressContract(contactInfo.EAAlternateMailingAddress);
            }

            return contract;
        }

        public EARequestContract UpsertDemographics(string pin, EADemographicsContract contract)
        {
            var modifiedBy              = _authUser.WIUID;
            var modifiedDate            = DateTime.Now;
            var request                 = contract.RequestId == 0 ? _eaRequestRepository.New() : _eaRequestRepository.Get(i => i.Id == contract.RequestId && !i.IsDeleted);
            var isApplicationDateChange = request.Id         != 0 && request.ApplicationDate.CompareTo(contract.ApplicationDate) != 0;

            request.ApplicationDate                 = contract.ApplicationDate;
            request.DidApplicantTakeCareOfAnyChild  = contract.DidApplicantTakeCareOfAnyChild;
            request.WillTheChildStayInApplicantCare = contract.WillTheChildStayInApplicantCare;
            request.CaresCaseNumber                 = contract.CaresCaseNumber;
            request.ApplicationInitiatedMethodId    = contract.ApplicationInitiatedMethodId;
            request.AccessTrackingNumber            = contract.AccessTrackingNumber;
            request.ModifiedBy                      = modifiedBy;
            request.ModifiedDate                    = modifiedDate;

            UpsertEaContactInfo(contract, request, modifiedBy, modifiedDate);

            if (request.Id == 0 || isApplicationDateChange)
            {
                var inProgressStatus = _eaStatusRepository.Get(i => i.Code == _inProgress);
                var parms = new Dictionary<string, object>
                            {
                                ["StartDate"] = request.ApplicationDate,
                                ["NoofDays"]  = 5
                            };
                var statusDeadLineDate = _eaRequestRepository.ExecFunction<DateTime>("FN_GetComputedBusniessDays", parms).FirstOrDefault();

                if (request.Id == 0)
                {
                    request.EaRequestStatuses.Add(new EARequestStatus
                                                  {
                                                      EaRequest          = request,
                                                      EaStatus           = inProgressStatus,
                                                      StatusDeadLineDate = statusDeadLineDate,
                                                      ModifiedBy         = modifiedBy,
                                                      ModifiedDate       = modifiedDate
                                                  });

                    request.OrganizationId = _organizationRepository.Get(i => i.EntsecAgencyCode == _authUser.AgencyCode)?.Id;
                    AddEaRequestParticipant(pin, request, modifiedBy, modifiedDate);

                    _eaRequestRepository.Add(request);
                }
                else
                {
                    var eaRequestStatuses = _eaRequestStatusRepository.Get(i => i.RequestId == request.Id && i.StatusId == inProgressStatus.Id);
                    eaRequestStatuses.StatusDeadLineDate = statusDeadLineDate;
                    eaRequestStatuses.ModifiedBy         = modifiedBy;
                    eaRequestStatuses.ModifiedDate       = modifiedDate;
                }
            }

            _unitOfWork.Commit();

            return GetRequest(request.Id, request);
        }

        private void AddEaRequestParticipant(string pin, EARequest request, string modifiedBy, DateTime modifiedDate)
        {
            var participantBridge = _eaRequestParticipantBridgeRepository.New();
            var decimalPin        = decimal.Parse(pin);

            participantBridge.ParticipantId      = _participantRepository.Get(i => i.PinNumber == decimalPin)?.Id ?? 0;
            participantBridge.EARequestId        = request.Id;
            participantBridge.EAIndividualTypeId = _eaIndividualTypeRepository.Get(i => i.Code   == _careTakeCode).Id;
            participantBridge.EARelationTypeId   = _eaRelationshipTypeRepository.Get(i => i.Code == _selfCode).Id;
            participantBridge.IsIncluded         = true;
            participantBridge.ModifiedBy         = modifiedBy;
            participantBridge.ModifiedDate       = modifiedDate;

            _eaRequestParticipantBridgeRepository.Add(participantBridge);
        }

        private void UpsertEaContactInfo(EADemographicsContract contract, EARequest request, string modifiedBy, DateTime modifiedDate)
        {
            var deleteAlternateAddress = false;
            var contactInfo            = request.EaRequestContactInfos.LastOrDefault() ?? _eaRequestContactInfoRepository.New();
            contactInfo.EaRequest             = request;
            contactInfo.PhoneNumber           = contract.EaDemographicsContact.PhoneNumber;
            contactInfo.CanTextPhone          = contract.EaDemographicsContact.CanText;
            contactInfo.AlternatePhoneNumber  = contract.EaDemographicsContact.AlternatePhoneNumber;
            contactInfo.CanTextAlternatePhone = contract.EaDemographicsContact.CanTextAlternate;
            contactInfo.BestWayToReach        = contract.EaDemographicsContact.BestWayToReach;
            contactInfo.EmailAddress          = contract.EaDemographicsContact.EmailAddress?.Trim() == "" ? null : contract.EaDemographicsContact.EmailAddress;
            contactInfo.CountyOfResidenceId   = contract.EaDemographicsContact.CountyOfResidenceId;
            contactInfo.HomelessIndicator     = contract.EaDemographicsContact.IsHomeless ?? false;

            if (contract.EaDemographicsContact.HouseholdAddress != null && !contract.EaDemographicsContact.HouseholdAddress.IsEmpty() && contract.EaDemographicsContact.IsHomeless != true)
            {
                contactInfo.City = _cityDomain.GetOrCreateCity(user: _authUser.Username, finalistAddress: contract.EaDemographicsContact.HouseholdAddress, isClientReg: true);

                // In case we have a city that was deleted, we need to restore it.
                contactInfo.City.IsDeleted                  = false;
                contactInfo.AddressLine1                    = contract.EaDemographicsContact.HouseholdAddress.AddressLine1;
                contactInfo.ZipCode                         = contract.EaDemographicsContact.HouseholdAddress.Zip;
                contactInfo.IsHouseHoldMailingAddressSame   = contract.EaDemographicsContact.IsMailingSameAsHouseholdAddress;
                contactInfo.AddressVerificationTypeLookupId = contract.EaDemographicsContact.HouseholdAddress.UseSuggestedAddress ? AddressVerificationType.FinalistVerifiedId : AddressVerificationType.WorkerOverrideId;
            }
            else
            {
                contactInfo.CityAddressId                   = null;
                contactInfo.AddressLine1                    = null;
                contactInfo.ZipCode                         = null;
                contactInfo.AddressLine2                    = null;
                contactInfo.AddressVerificationTypeLookupId = null;
                contactInfo.IsHouseHoldMailingAddressSame   = contract.EaDemographicsContact.IsMailingSameAsHouseholdAddress;
            }

            // See if we have a City location for the mailing address.
            if ((contract.EaDemographicsContact.IsHomeless == true && !contract.EaDemographicsContact.MailingAddress.IsEmpty()) || (contract.EaDemographicsContact.MailingAddress != null && !contract.EaDemographicsContact.MailingAddress.IsEmpty() && contactInfo.IsHouseHoldMailingAddressSame == false))
            {
                if (contactInfo.EAAlternateMailingAddress == null)
                {
                    contactInfo.EAAlternateMailingAddress = _eaAlternateMailingAddressRepository.New();
                }

                contactInfo.EAAlternateMailingAddress.City = _cityDomain.GetOrCreateCity(user: _authUser.Username, finalistAddress: contract.EaDemographicsContact.MailingAddress, isClientReg: true);

                // In case we have a city that was deleted, we need to restore it.
                contactInfo.EAAlternateMailingAddress.City.IsDeleted                  = false;
                contactInfo.EAAlternateMailingAddress.AddressLine1                    = contract.EaDemographicsContact.MailingAddress.AddressLine1;
                contactInfo.EAAlternateMailingAddress.ZipCode                         = contract.EaDemographicsContact.MailingAddress.Zip;
                contactInfo.EAAlternateMailingAddress.AddressVerificationTypeLookupId = contract.EaDemographicsContact.MailingAddress.UseSuggestedAddress ? AddressVerificationType.FinalistVerifiedId : AddressVerificationType.WorkerOverrideId;
                contactInfo.EAAlternateMailingAddress.ModifiedBy                      = modifiedBy;
                contactInfo.EAAlternateMailingAddress.ModifiedDate                    = modifiedDate;
            }
            else
            {
                if (contactInfo.EAAlternateMailingAddress != null)
                {
                    deleteAlternateAddress                = true;
                    contactInfo.EAAlternateMailingAddress = null;
                }
            }

            contactInfo.ModifiedBy   = modifiedBy;
            contactInfo.ModifiedDate = modifiedDate;

            if (contactInfo.Id == 0)
            {
                contactInfo.CreatedDate = modifiedDate;
                _eaRequestContactInfoRepository.Add(contactInfo);
            }
            else
                _eaRequestContactInfoRepository.Update(contactInfo);

            if (deleteAlternateAddress)
            {
                _eaAlternateMailingAddressRepository.Delete(i => i.Id == contactInfo.AlternateMailingAddressId);
            }
        }

        #endregion
    }
}
