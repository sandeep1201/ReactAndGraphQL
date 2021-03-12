using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.Api.Library.Extensions;
using Dcf.Wwp.Api.Library.Interfaces;
using Dcf.Wwp.Api.Library.Utils;
using Dcf.Wwp.ConnectedServices.Cww;
using Dcf.Wwp.ConnectedServices.Mci;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Constants;
using Dcf.Wwp.Model.Interface.Core;
using Dcf.Wwp.Model.Interface.Repository;
using DCF.Common.Exceptions;
using DCF.Common.Logging;
using Dcf.Wwp.Data.Sql.Model;
using Dcf.Wwp.DataAccess.Interfaces;
using CountyAndTribe = Dcf.Wwp.Model.Interface.Constants.CountyAndTribe;

namespace Dcf.Wwp.Api.Library.ViewModels
{
    public class ClientRegistrationViewModel : IClientRegistrationViewModel
    {
        #region Properties

        private readonly string            _iPAddress         = "192.168.1.1";
        private readonly string            _externalAgencyId  = "TANF";
        private readonly short             _minimumMatchScore = 75; //TODO: inject this too
        private readonly IMciService       _mciSvc;
        private readonly ICwwIndService    _cwwIndSvc;
        private readonly ICwwKeySecService _cwwKeySvc;
        private readonly IGoogleApi        _googleApi;
        private readonly ILocations        _locations;
        private readonly IAuthUser         _authUser;
        private readonly IRepository       _repository;

        private ILog Logger { get; set; }

        Func<decimal?, string> _ssnForMciAsString = (d) =>
                                                    {
                                                        if (d == null) d = 0m;

                                                        var r = d != 0 ? d.ToString().PadLeft(9, '0') : "0";

                                                        return (r);
                                                    };

        Func<decimal?, long> _ssnForMciAsLong = (d) =>
                                                {
                                                    if (d == null) d = 0m;

                                                    return (Convert.ToInt64(d));
                                                };

        Func<decimal?, string> _ssnForCww = (d) =>
                                            {
                                                var r = string.Empty;

                                                if (d != null && d != 0)
                                                {
                                                    r = d != 0 ? d.ToString().PadLeft(9, '0') : "0";
                                                }

                                                return (r);
                                            };

        #endregion

        #region Methods

        public ClientRegistrationViewModel(IMciService mciSvc, ICwwIndService cwwIndSvc, ICwwKeySecService cwwKeySecSvc, IGoogleApi googleApi, ILocations locations, IRepository repository, IAuthUser authUser)
        {
            _mciSvc     = mciSvc;
            _cwwIndSvc  = cwwIndSvc;
            _cwwKeySvc  = cwwKeySecSvc;
            _googleApi  = googleApi;
            _locations  = locations;
            _repository = repository;
            _authUser   = authUser;
            Logger      = LogProvider.GetLogger(GetType());
        }

        /// <summary>
        ///     Finds participants in WWP and then also grabs data from CWW.
        /// </summary>
        /// <param name="mciId"></param>
        /// <param name="demographicContract"></param>
        /// <returns></returns>
        public ClientRegistrationContract GetClientRegistration(long mciId, DemographicResultContract demographicContract = null)
        {
            if (mciId == 0)
            {
                return new ClientRegistrationContract();
            }

            var part = _repository.GetParticipantByMciId(mciId);

            var contract = new ClientRegistrationContract();

            // We didnt find the part.
            if (part == null && demographicContract?.IsMciKnownToCww == true)
            {
                // This logic follows US1561...
                // This call will get the existing PIN since the isMciIdKnown indicates such.
                var response = CreateOrGetExistingCwwParticipant(mciId.ToString(), demographicContract.Name?.FirstName, demographicContract.Name?.MiddleInitial, demographicContract.Name?.LastName, demographicContract.Name?.Suffix, string.IsNullOrWhiteSpace(demographicContract.Ssn) ? 0 : Convert.ToInt64(demographicContract.Ssn), demographicContract.DateOfBirth, demographicContract.Gender, _authUser.MainFrameId);

                if (!string.IsNullOrWhiteSpace(response?.PINNumber))
                {
                    part = _repository.GetRefreshedParticipant(response.PINNumber);
                }
            }

            // It's technically possible that we still don't have a participant.
            if (part == null)
            {
                return contract;
            }

            contract.MciId             = part.MCI_ID;
            contract.PinNumber         = part.PinNumber;
            contract.IsPinConfidential = part.ConfidentialPinInformations.AsNotNull().Any(i => i.IsConfidential == true && !i.IsDeleted);

            if (part.ConfidentialPinInformations.FirstOrDefault() != null)
            {
                contract.IsPinConfidential = part.ConfidentialPinInformations.First().IsConfidential;
            }

            var suffix = _repository.GetSuffixTypeByName(part.SuffixName.Trim());

            contract.Name.FirstName     = part.FirstName?.SafeTrim();
            contract.Name.MiddleInitial = part.MiddleInitialName?.SafeTrim();
            contract.Name.LastName      = part.LastName?.SafeTrim();
            contract.Name.Suffix        = part.SuffixName;
            contract.Name.SuffixTypeId  = suffix?.Id;
            contract.DateOfBirth        = part.DateOfBirth.Value; // we should always have dob.
            contract.GenderIndicator    = part.GenderIndicator;

            var indvReg = new GetIndvDemographicsRequest
                          {
                              PINNumber        = part.PinNumber.ToString().PadLeft(10, '0'),
                              ExternalAgencyId = _externalAgencyId
                          };

            var cwwRes = _cwwIndSvc.GetIndvDemographics(indvReg);

            int.TryParse(cwwRes?.Individual?.SSNNumber, out var ssn);
            if (ssn > 0)
            {
                contract.Ssn = ssn.ToString().PadLeft(9, '0');
            }
            else
            {
                contract.IsNoSsn = (ssn == 0);
            }

            // Race And Ethnicity.
            contract.RaceEthnicity.IsHispanic        = cwwRes?.Individual?.Ethnicity?.Hispanic?.ToBool();
            contract.RaceEthnicity.IsAmericanIndian  = cwwRes?.Individual?.Race?.AmericanIndianOrAlaskan?.ToBool();
            contract.RaceEthnicity.IsPacificIslander = cwwRes?.Individual?.Race?.HawaiianOrPacificIslander?.ToBool();
            contract.RaceEthnicity.IsAsian           = cwwRes?.Individual?.Race?.Asian?.ToBool();
            contract.RaceEthnicity.IsWhite           = cwwRes?.Individual?.Race?.White?.ToBool();
            contract.RaceEthnicity.IsBlack           = cwwRes?.Individual?.Race?.BlackOrAfricanAmerican?.ToBool();

            //TODO: SSNVerificationCodeDescription not sure where we get it from.
            contract.SsnVerificationCode            = cwwRes?.Individual?.SSNVerificationCode;
            contract.SsnVerificationCodeDescription = string.Empty;

            if (cwwRes?.Individual?.AliasNames != null)
            {
                var i = 0;

                foreach (var aliasName in cwwRes?.Individual?.AliasNames)
                {
                    var pnc = new PersonNameContract
                              {
                                  Id            = ++i,
                                  FirstName     = aliasName.AliasFirstName?.Trim(),
                                  MiddleInitial = aliasName.AliasMiddleInitial?.Trim(),
                                  LastName      = aliasName.AliasLastName?.Trim(),
                                  Suffix        = aliasName.AliasSuffix?.Trim(),
                                  Alias         = AliasCodeToFullNameCww(aliasName.AliasType)
                              };

                    contract.Aliases.Add(pnc);
                }
            }

            foreach (var aliasName in part.AKAs)
            {
                var alias = new SsnAliasContract
                            {
                                Id      = aliasName.Id,
                                TypeId  = aliasName.SSNTypeId,
                                Ssn     = aliasName.SSNNumber,
                                Details = aliasName.Details
                            };

                contract.AltSsns.Add(alias);
            }

            contract.RaceEthnicity.HistorySequenceNumber = cwwRes.Individual == null ? 0 : int.Parse(cwwRes.Individual?.HistorySeqNum);

            var participantContactInfo = part.ParticipantContactInfoes.LastOrDefault();

            if (participantContactInfo != null)
            {
                contract.PrimaryPhone.PhoneNumber        = participantContactInfo.PrimaryPhoneNumber;
                contract.PrimaryPhone.CanVoiceMail       = participantContactInfo.CanLeaveVoiceMailPrimaryPhone;
                contract.PrimaryPhone.CanText            = participantContactInfo.CanTextPrimaryPhone;
                contract.SecondaryPhone.PhoneNumber      = participantContactInfo.SecondaryPhoneNumber;
                contract.SecondaryPhone.CanVoiceMail     = participantContactInfo.CanLeaveVoiceMailSecondaryPhone;
                contract.SecondaryPhone.CanText          = participantContactInfo.CanTextSecondaryPhone;
                contract.EmailAddress                    = participantContactInfo.EmailAddress;
                contract.CountyOfResidenceId             = participantContactInfo.CountyOfResidenceId;
                contract.IsHomeless                      = participantContactInfo.HomelessIndicator;
                contract.IsMailingSameAsHouseholdAddress = participantContactInfo.IsHouseHoldMailingAddressSame;
                contract.HouseholdAddress                = _locations.GetFinalistLocationInfo(participantContactInfo);

                if (participantContactInfo?.AlternateMailingAddress?.City != null)
                {
                    contract.MailingAddress = _locations.GetFinalistLocationInfo(participantContactInfo.AlternateMailingAddress);
                }

                contract.Notes = participantContactInfo.Notes;
            }

            var participantOtherDemographic = part.OtherDemographics.LastOrDefault();

            if (participantOtherDemographic != null)
            {
                contract.HomeLanguageId      = participantOtherDemographic.HomeLanguageId;
                contract.IsInterpreterNeeded = participantOtherDemographic.IsInterpreterNeeded;
                contract.InterpreterDetails  = participantOtherDemographic.InterpreterDetails;

                // Refugee.
                contract.IsRefugee               = participantOtherDemographic.IsRefugee;
                contract.CountryOfOriginId       = participantOtherDemographic.CountryOfOriginId;
                contract.EntryDate               = participantOtherDemographic.RefugeeEntryDate;
                contract.RefugeeEntryDateUnknown = participantOtherDemographic.RefugeeEntryDateUnknown;

                // American Indian.
                contract.IsInTribe     = participantOtherDemographic.TribalIndicator;
                contract.TribeId       = participantOtherDemographic.TribalId;
                contract.TribalDetails = participantOtherDemographic.TribalDetails;
            }

            return contract;
        }

        public StatusContract UpsertClientRegistration(ClientRegistrationContract contract)
        {
            var statusContract          = new StatusContract();
            var isMciParticipantUpdated = false;

            IParticipant participant = null;

            var ssn = 0.0m; // this is a quick fix. They want this to be a string now.
            if (!string.IsNullOrEmpty(contract.Ssn))
            {
                decimal.TryParse(contract.Ssn, out ssn);
            }

            // Brand new participant.
            if (!contract.MciId.HasValue)
            {
                participant = CreateNewWwpParticipant(contract.Name.FirstName, contract.Name.MiddleInitial, contract.Name.LastName, contract.Name.Suffix, ssn, contract.DateOfBirth, contract.GenderIndicator, _authUser.MainFrameId);
            }

            // Existing (MCI) participant.
            if (contract.MciId.HasValue)
            {
                participant = GetExistingMciParticipant(contract.MciId.Value, contract.Name.FirstName, contract.Name.MiddleInitial, contract.Name.LastName, contract.Name.Suffix, ssn, contract.DateOfBirth, contract.GenderIndicator, _authUser.MainFrameId);
            }

            // Working with existing WWP participant at this point; we will also have a mci.
            // Sanity check to make sure we are.
            if (participant == null)
            {
                throw new UserFriendlyException("Participant not found in WWP.");
            }

            // If user does not provide SSN lets set it to 0 for MCI. Use this ssn for all MCI requests.
            var ssnOrZeroForMci = ssn;

            statusContract.PinNumber = participant.PinNumber;

            contract.Name.Suffix = _repository.GetSuffixTypeById(contract.Name?.SuffixTypeId)?.Code;

            var indvReg = new GetIndvDemographicsRequest
                          {
                              PINNumber        = participant.PinNumber.ToString().PadLeft(10, '0'),
                              ExternalAgencyId = _externalAgencyId
                          };

            var cwwRes = _cwwIndSvc.GetIndvDemographics(indvReg);

            // Only update personal info if we have changes in the name or alias.
            var hasAliasChanged = contract.Aliases?.Any(x => x.IsNew()) != null && contract.Aliases.Any(x => x.IsNew());
            var hasNameChanged = participant.FirstName.NullStringToBlank().TrimAndLower()         != contract.Name?.FirstName.NullStringToBlank().TrimAndLower()     ||
                                 participant.MiddleInitialName.NullStringToBlank().TrimAndLower() != contract.Name?.MiddleInitial.NullStringToBlank().TrimAndLower() ||
                                 participant.LastName.NullStringToBlank().TrimAndLower()          != contract.Name?.LastName.NullStringToBlank().TrimAndLower()      ||
                                 participant.SuffixName.NullStringToBlank().TrimAndLower()        != contract.Name?.Suffix.NullStringToBlank().TrimAndLower();
            var hasGenderChanged = participant.GenderIndicator != contract.GenderIndicator;

            //Adding Middile Initial and Suffix to this check as this fails when there is a change to them through Client Reg
            var reVerifySsn = participant.FirstName.NullStringToBlank().TrimAndLower()                   != contract.Name?.FirstName.NullStringToBlank().TrimAndLower()     ||
                              participant.MiddleInitialName.NullStringToBlank().TrimAndLower()           != contract.Name?.MiddleInitial.NullStringToBlank().TrimAndLower() ||
                              participant.LastName.NullStringToBlank().TrimAndLower()                    != contract.Name?.LastName.NullStringToBlank().TrimAndLower()      ||
                              participant.SuffixName.NullStringToBlank().TrimAndLower()                  != contract.Name?.Suffix.NullStringToBlank().TrimAndLower()        ||
                              participant.DateOfBirth                                                    != contract.DateOfBirth                                            ||
                              (cwwRes.Individual.SSNNumber == "0" ? "0.0" : cwwRes.Individual.SSNNumber) != ssn.ToString();
            //participant.PrimarySSNNumber                             != contract.Ssn;

            var ssnVerificationCode     = contract.SsnVerificationCode;
            var hasBeenThroughClientReg = (participant.HasBeenThroughClientReg == true);

            if (reVerifySsn)
            {
                ssnVerificationCode = "C";
            }

            if (!hasBeenThroughClientReg && contract.SsnVerificationCode != "V")
            {
                ssnVerificationCode = "C";
            }

            // Only update on alias, name changes when we are working with an existing participant.
            if ((hasAliasChanged || hasNameChanged || hasGenderChanged || reVerifySsn) && participant.PinNumber.HasValue && participant.MCI_ID.HasValue && contract.MciId.HasValue)
            {
                UpdatePersonalInformationInMciAndCww(
                                                     participant.PinNumber.Value,
                                                     participant.MCI_ID.Value,
                                                     contract.Name.FirstName,
                                                     contract.Name.MiddleInitial,
                                                     contract.Name.LastName,
                                                     contract.Name.Suffix,
                                                     contract.Aliases,
                                                     ssn,
                                                     contract.IsNoSsn,
                                                     contract.DateOfBirth,
                                                     contract.GenderIndicator,
                                                     _authUser.MainFrameId,
                                                     contract.RaceEthnicity.HistorySequenceNumber.ToString(),
                                                     ssnVerificationCode
                                                    );

                contract.RaceEthnicity.HistorySequenceNumber++;
                isMciParticipantUpdated = true;
            }

            if (participant.ConfidentialPinInformations.LastOrDefault() == null)
            {
                var newConfidentialPinInformation = _repository.NewConfidentialPinInformation(participant);
                newConfidentialPinInformation.ModifiedBy     = _authUser.Username;
                newConfidentialPinInformation.IsConfidential = contract.IsPinConfidential;
                newConfidentialPinInformation.Worker         = _repository.WorkerByWIUID(_authUser.WIUID);
            }
            else
            {
                participant.ConfidentialPinInformations.Last().IsConfidential = contract.IsPinConfidential;
                participant.ConfidentialPinInformations.Last().Worker         = _repository.WorkerByWIUID(_authUser.WIUID);
            }

            // Race and Ethnicity for existing participants.
            if (contract.MciId.HasValue)
            {
                UpdateRaceAndEthnicity(
                                       ref participant,
                                       contract.RaceEthnicity.IsPacificIslander,
                                       contract.RaceEthnicity.IsAmericanIndian,
                                       contract.RaceEthnicity.IsAsian,
                                       contract.RaceEthnicity.IsWhite,
                                       contract.RaceEthnicity.IsBlack,
                                       contract.RaceEthnicity.IsHispanic,
                                       contract.RaceEthnicity.HistorySequenceNumber.ToString(),
                                       _authUser.MainFrameId
                                      );

                isMciParticipantUpdated = true;
            }

            // Alias but only for existing participants.
            if (contract.MciId.HasValue)
            {
                UpdateAliasesInMci((long) ssnOrZeroForMci, (long) participant.MCI_ID, contract.Aliases, contract.DeletedAliases);
                isMciParticipantUpdated = true;
            }

            // Alt SSN repeater.
            foreach (var aka in participant.AKAs.AsNotNull())
            {
                // Delete all.
                aka.IsDeleted = true;
            }

            foreach (var cAka in contract.AltSsns.AsNotNull().Where(x => x.IsEmpty == false))
            {
                var isNew = true;

                foreach (var aka in participant.AllAKAs.AsNotNull())
                {
                    if (cAka.Ssn == aka.SSNNumber && cAka.TypeId == aka.SSNTypeId && cAka.Details == aka.Details)
                    {
                        // Restore.
                        aka.IsDeleted    = false;
                        isNew            = false;
                        aka.ModifiedBy   = _authUser.Username;
                        aka.ModifiedDate = DateTime.Now;
                    }
                    else
                    {
                        if (aka.Id == cAka.Id && aka.Id > 0)
                        {
                            // Update.
                            aka.SSNNumber    = cAka.Ssn;
                            aka.SSNTypeId    = cAka.TypeId;
                            aka.Details      = cAka.Details;
                            aka.IsDeleted    = false;
                            isNew            = false;
                            aka.ModifiedBy   = _authUser.Username;
                            aka.ModifiedDate = DateTime.Now;
                        }
                    }
                }

                if (isNew)
                {
                    var x = _repository.NewAKA(participant);
                    x.SSNNumber    = cAka.Ssn;
                    x.SSNTypeId    = cAka.TypeId;
                    x.Details      = cAka.Details;
                    x.ModifiedBy   = _authUser.Username;
                    x.ModifiedDate = DateTime.Now;
                    x.CreatedDate  = _authUser.CDODate ?? DateTime.Now;
                }
            }

            // Primary Phone and Secondary Phone.
            if (contract.InEAMode != true)
            {
                var contactInfo = participant.ParticipantContactInfoes.LastOrDefault() ?? _repository.NewParticipantContactInfo(participant);
                contactInfo.PrimaryPhoneNumber              = contract.PrimaryPhone.PhoneNumber;
                contactInfo.CanTextPrimaryPhone             = contract.PrimaryPhone.CanText;
                contactInfo.CanLeaveVoiceMailPrimaryPhone   = contract.PrimaryPhone.CanVoiceMail;
                contactInfo.SecondaryPhoneNumber            = contract.SecondaryPhone.PhoneNumber;
                contactInfo.CanTextSecondaryPhone           = contract.SecondaryPhone.CanText;
                contactInfo.CanLeaveVoiceMailSecondaryPhone = contract.SecondaryPhone.CanVoiceMail;
                contactInfo.EmailAddress                    = contract.EmailAddress?.Trim() == "" ? null : contract.EmailAddress;

                contactInfo.ModifiedBy   = _authUser.Username;
                contactInfo.ModifiedDate = DateTime.Now;

                var otherDemographic = participant.OtherDemographics.LastOrDefault() ?? _repository.NewOtherDemographic(participant);

                // Home Language.
                otherDemographic.HomeLanguageId = contract.HomeLanguageId;

                if (contract.HomeLanguageId != Languages.EnglishId)
                {
                    otherDemographic.IsInterpreterNeeded = contract.IsInterpreterNeeded;
                    otherDemographic.InterpreterDetails  = contract.InterpreterDetails;
                }
                else
                {
                    otherDemographic.IsInterpreterNeeded = null;
                    otherDemographic.InterpreterDetails  = null;
                }

                // Refugee.
                otherDemographic.IsRefugee = contract.IsRefugee;

                if (contract.IsRefugee == true)
                {
                    otherDemographic.CountryOfOriginId       = contract.CountryOfOriginId;
                    otherDemographic.RefugeeEntryDate        = contract.EntryDate;
                    otherDemographic.RefugeeEntryDateUnknown = contract.RefugeeEntryDateUnknown;
                }
                else
                {
                    otherDemographic.CountryOfOriginId       = null;
                    otherDemographic.RefugeeEntryDate        = null;
                    otherDemographic.RefugeeEntryDateUnknown = null;
                }

                // American Indian.
                otherDemographic.TribalIndicator = contract.IsInTribe;

                if (contract.IsInTribe == true)
                {
                    otherDemographic.TribalId      = contract.TribeId;
                    otherDemographic.TribalDetails = otherDemographic.TribalId == CountyAndTribe.Other ? contract.TribalDetails : null;
                }
                else
                {
                    otherDemographic.TribalId      = null;
                    otherDemographic.TribalDetails = null;
                }

                contactInfo.CountyOfResidenceId = contract.CountyOfResidenceId;
                contactInfo.HomelessIndicator   = contract.IsHomeless ?? false;

                // See if we have a City location for the household address.
                if (contract.HouseholdAddress != null && !contract.HouseholdAddress.IsEmpty() && contract.IsHomeless != true)
                {
                    contactInfo.City = _repository.GetOrCreateCity(user: _authUser.Username, finalistAddress: contract.HouseholdAddress, isClientReg: true);

                    // In case we have a city that was deleted, we need to restore it.
                    contactInfo.City.IsDeleted                  = false;
                    contactInfo.AddressLine1                    = contract.HouseholdAddress.AddressLine1;
                    contactInfo.ZipCode                         = contract.HouseholdAddress.Zip;
                    contactInfo.IsHouseHoldMailingAddressSame   = contract.IsMailingSameAsHouseholdAddress;
                    contactInfo.AddressVerificationTypeLookupId = contract.HouseholdAddress.UseSuggestedAddress ? AddressVerificationType.FinalistVerifiedId : AddressVerificationType.WorkerOverrideId;
                }
                else
                {
                    contactInfo.CityAddressId                   = null;
                    contactInfo.AddressLine1                    = null;
                    contactInfo.ZipCode                         = null;
                    contactInfo.AddressLine2                    = null;
                    contactInfo.IsHouseHoldMailingAddressSame   = null;
                    contactInfo.AddressVerificationTypeLookupId = null;
                }

                // See if we have a City location for the mailing address.
                if (contract.IsHomeless == true || contract.MailingAddress != null && !contract.MailingAddress.IsEmpty() && contactInfo.IsHouseHoldMailingAddressSame == false)
                {
                    if (contactInfo.AlternateMailingAddress == null)
                    {
                        contactInfo.AlternateMailingAddress = _repository.NewAlternateMailingAddress(contactInfo);
                    }

                    contactInfo.AlternateMailingAddress.City = _repository.GetOrCreateCity(user: _authUser.Username, finalistAddress: contract.MailingAddress, isClientReg: true);
                    // In case we have a city that was deleted, we need to restore it.
                    contactInfo.AlternateMailingAddress.City.IsDeleted                  = false;
                    contactInfo.AlternateMailingAddress.AddressLine1                    = contract.MailingAddress.AddressLine1;
                    contactInfo.AlternateMailingAddress.ZipCode                         = contract.MailingAddress.Zip;
                    contactInfo.AlternateMailingAddress.AddressVerificationTypeLookupId = contract.MailingAddress.UseSuggestedAddress ? AddressVerificationType.FinalistVerifiedId : AddressVerificationType.WorkerOverrideId;
                    contactInfo.AlternateMailingAddress.ModifiedBy                      = _authUser.Username; // ZenHub #2552
                    contactInfo.AlternateMailingAddress.ModifiedDate                    = DateTime.Now;       // ZenHub #2552
                }
                else
                {
                    if (contactInfo.AlternateMailingAddress != null)
                    {
                        contactInfo.AlternateMailingAddress.CityAddressId                   = null;
                        contactInfo.AlternateMailingAddress.AddressLine1                    = null;
                        contactInfo.AlternateMailingAddress.ZipCode                         = null;
                        contactInfo.AlternateMailingAddress.AddressLine2                    = null;
                        contactInfo.AlternateMailingAddress.AddressVerificationTypeLookupId = null;
                        contactInfo.AlternateMailingAddress.ModifiedBy                      = _authUser.Username; // ZenHub #2552
                        contactInfo.AlternateMailingAddress.ModifiedDate                    = DateTime.Now;       // ZenHub #2552
                    }
                }

                contactInfo.Notes        = contract.Notes;
                contactInfo.ModifiedBy   = _authUser.Username;
                contactInfo.ModifiedDate = DateTime.Now;
            }
            else
            {
                if (contract.EaRequestId != null && participant.EaRequestParticipantBridges.AsNotNull().All(i => i.EARequestId != contract.EaRequestId))
                {
                    var newEaRequestParticipantBridge = _repository.NewEaRequestParticipantBridge(participant);
                    newEaRequestParticipantBridge.EARequestId = (int) contract.EaRequestId;
                    newEaRequestParticipantBridge.IsIncluded  = true;
                    participant.EaRequestParticipantBridges.Add(newEaRequestParticipantBridge);
                }
            }

            participant.HasBeenThroughClientReg = true;
            participant.ModifiedBy              = _authUser.Username;
            participant.ModifiedDate            = DateTime.Now;

            _repository.Save();

            if (contract.EaRequestId != null && isMciParticipantUpdated)
                _repository.GetRefreshedParticipant(participant.PinNumber.ToString());

            return statusContract;
        }

        public List<DemographicResultContract> GetClearanceSearchResults(DemographicSearchContract contract)
        {
            var matches = new List<DemographicResultContract>();

            var results = SearchByDemographics(contract);

            if (results != null && results.Any())
            {
                matches = results.Select(m => new DemographicResultContract
                                              {
                                                  Name =
                                                  {
                                                      FirstName     = m.PrimaryName.FirstName?.Trim(),
                                                      MiddleInitial = m.PrimaryName.MiddleInitial?.Trim(),
                                                      LastName      = m.PrimaryName.LastName?.Trim(),
                                                      Suffix        = m.PrimaryName.SuffixName?.Trim()
                                                  },
                                                  Ssn                            = m.SSN.ToString(),
                                                  SsnVerificationCode            = m.SSNVerificationCode,
                                                  SsnVerificationCodeDescription = m.SSNVerificationCodeDescription?.Trim(),
                                                  DateOfBirth                    = m.DOB,
                                                  Gender                         = m.Gender,
                                                  Score                          = m.MatchScore,
                                                  MciId                          = m.MCIID,
                                                  IsMciKnownToCww                = m.IsMCIIDKnown
                                              }).ToList();
            }

            return (matches);
        }

        /// <summary>
        /// Searches MCI for participants.
        /// </summary>
        /// <param name="contract"></param>
        /// <returns></returns>
        public List<DemographicSearchMatch> SearchByDemographics(DemographicSearchContract contract)
        {
            var matches = new List<DemographicSearchMatch>();
            var ssn     = _ssnForMciAsLong(Convert.ToInt64(contract.Ssn?.Replace("-", "")));

            var req = new SearchByDemographicsRequest
                      {
                          SearchDetails = new DemographicSearchRequest
                                          {
                                              PrimaryName       = new Name { FirstName = contract.Name.FirstName, LastName = contract.Name.LastName, MiddleInitial = contract.Name.MiddleInitial, SuffixName = contract.Name.Suffix, NameType = NameType.PrimaryName },
                                              SSN               = ssn,
                                              DOB               = contract.DateOfBirth, // Workaround to for WCF/CWW
                                              Gender            = contract.Gender,
                                              MinimumMatchScore = _minimumMatchScore, // Set to 75 per BR.
                                              IncludeAliasSSN   = false,
                                              UserName          = _authUser.MainFrameId,

                                              AliasName = new[]
                                                          {
                                                              new Name
                                                              {
                                                                  FirstName     = contract.Aliases.ElementAtOrDefault(0)?.FirstName     ?? string.Empty,
                                                                  LastName      = contract.Aliases.ElementAtOrDefault(0)?.LastName      ?? string.Empty,
                                                                  MiddleInitial = contract.Aliases.ElementAtOrDefault(0)?.MiddleInitial ?? string.Empty,
                                                                  SuffixName    = contract.Aliases.ElementAtOrDefault(0)?.Suffix        ?? string.Empty,
                                                                  NameType      = AliasFullNameToEnumMci(contract.Aliases.ElementAtOrDefault(0)?.Alias)
                                                              },
                                                              new Name
                                                              {
                                                                  FirstName     = contract.Aliases.ElementAtOrDefault(1)?.FirstName     ?? string.Empty,
                                                                  LastName      = contract.Aliases.ElementAtOrDefault(1)?.LastName      ?? string.Empty,
                                                                  MiddleInitial = contract.Aliases.ElementAtOrDefault(1)?.MiddleInitial ?? string.Empty,
                                                                  SuffixName    = contract.Aliases.ElementAtOrDefault(1)?.Suffix        ?? string.Empty,
                                                                  NameType      = AliasFullNameToEnumMci(contract.Aliases.ElementAtOrDefault(1)?.Alias)
                                                              }
                                                          },
                                              ReasonForRequest = string.Empty
                                          }
                      };

            var res = _mciSvc.SearchByDemographics(req);

            if (res?.SearchByDemographicsResult?.Match != null)
            {
                matches.AddRange(res.SearchByDemographicsResult?.Match);
            }

            return (matches);
        }

        public IParticipant GetParticipantByMciId(decimal mciID) => _repository.GetParticipantByMciId(mciID);

        /// <summary>
        /// Updates Name, DOB, SSN, Gender and alias in both MCI and CWW.
        /// </summary>
        /// <param name="pinNumber"></param>
        /// <param name="mciId"></param>
        /// <param name="firstName"></param>
        /// <param name="middleInitialName"></param>
        /// <param name="lastName"></param>
        /// <param name="suffixName"></param>
        /// <param name="aliases"></param>
        /// <param name="ssn"></param>
        /// <param name="isNoSsn"></param>
        /// <param name="dateOfBirth"></param>
        /// <param name="genderChar"></param>
        /// <param name="mainframeId"></param>
        /// <param name="historySequenceNumber"></param>
        /// <param name="ssnVerifyCode"></param>
        private void UpdatePersonalInformationInMciAndCww(decimal  pinNumber, decimal mciId,   string   firstName,   string middleInitialName, string lastName,    string suffixName,            List<PersonNameContract> aliases,
                                                          decimal? ssn,       bool?   isNoSsn, DateTime dateOfBirth, string genderChar,        string mainframeId, string historySequenceNumber, string                   ssnVerifyCode)
        {
            // Update first in CWW.
            var updateCwwRequest = new UpdateIndvKeyDemographicsRequest();
            updateCwwRequest.Individual               = new IndividualType();
            updateCwwRequest.Individual.FirstName     = firstName;
            updateCwwRequest.Individual.MiddleInitial = middleInitialName;
            updateCwwRequest.Individual.LastName      = lastName;
            updateCwwRequest.Individual.Suffix        = suffixName.NullStringToBlank().Trim().ToUpper();
            updateCwwRequest.Individual.DOB           = dateOfBirth;
            updateCwwRequest.Individual.Gender        = genderChar;
            updateCwwRequest.Individual.MCIId         = mciId.ToString(CultureInfo.InvariantCulture).PadLeft(10, '0');
            updateCwwRequest.Individual.PINNumber     = pinNumber.ToString(CultureInfo.InvariantCulture).PadLeft(10, '0');

            var numberOfAliases = aliases.Count(x => x.IsNameEmpty);

            if (numberOfAliases > 0)
            {
                updateCwwRequest.Individual.AliasNames = new AliasNameType[numberOfAliases];
            }

            // Alias updated in CWW if not blank.
            if (numberOfAliases > 0)
            {
                updateCwwRequest.Individual.AliasNames[0]                    = new AliasNameType();
                updateCwwRequest.Individual.AliasNames[0].AliasFirstName     = aliases.First().FirstName;
                updateCwwRequest.Individual.AliasNames[0].AliasMiddleInitial = aliases.First().MiddleInitial;
                updateCwwRequest.Individual.AliasNames[0].AliasLastName      = aliases.First().LastName;

                // Do not send nulls or strings with spaces.
                updateCwwRequest.Individual.AliasNames[0].AliasSuffix = aliases.First().Suffix.SafeTrim().IsNullOrEmpty() ? "" : aliases.Last().Suffix.ToUpper();
                updateCwwRequest.Individual.AliasNames[0].AliasType   = AliasFullNameToCodeCww(aliases.First().Alias);

                if (numberOfAliases == 2)
                {
                    updateCwwRequest.Individual.AliasNames[1]                    = new AliasNameType();
                    updateCwwRequest.Individual.AliasNames[1].AliasFirstName     = aliases.Last().FirstName;
                    updateCwwRequest.Individual.AliasNames[1].AliasLastName      = aliases.Last().LastName;
                    updateCwwRequest.Individual.AliasNames[1].AliasSuffix        = aliases.Last().Suffix.SafeTrim().IsNullOrEmpty() ? "" : aliases.Last().Suffix.ToUpper();
                    updateCwwRequest.Individual.AliasNames[1].AliasMiddleInitial = aliases.Last().MiddleInitial;
                    updateCwwRequest.Individual.AliasNames[1].AliasType          = AliasFullNameToCodeCww(aliases.Last().Alias);
                }

                //if (numberOfAliases == 2)
                //{
                //    updateCwwRequest.Individual.AliasNames[1] = new AliasNameType
                //                                                {
                //                                                    AliasFirstName     = aliases.Last().FirstName,
                //                                                    AliasLastName      = aliases.Last().LastName,
                //                                                    AliasSuffix        = aliases.Last().Suffix?.Trim().IsNullOrEmpty() ? "" : aliases.Last().Suffix.ToUpper(),
                //                                                    AliasMiddleInitial = aliases.Last().MiddleInitial,
                //                                                    AliasType          = AliasFullNameToCodeCww(aliases.Last().Alias)
                //                                                };
                //}
            }

            //// If user does not provide SSN lets set it to null for CWW.
            ////if (ssn.HasValue && isNoSsn != true)
            ////{
            ////    var socialSecNum = ssn.ToString();
            ////    updateCwwRequest.Individual.SSNNumber = socialSecNum.PadLeft(9,'0');
            ////}
            ////else
            ////{
            ////    // This will not work with null. We need to pass empty string to set the ssn to zero in CWW.
            ////    updateCwwRequest.Individual.SSNNumber = "";
            ////}
            // the following line replaces the line above
            //updateCwwRequest.Individual.SSNNumber = (ssn.HasValue && isNoSsn != true) ? ssn.ToString().PadLeft(9, '0') : "";

            //// If user does not provide SSN lets set it to null for CWW.
            //if (ssn.HasValue && isNoSsn != true)
            //{
            //    var tmpSsn = ssn.ToString();

            //    updateCwwRequest.Individual.SSNNumber = tmpSsn != "0" ? tmpSsn.PadLeft(9, '0') : tmpSsn;
            //}
            //else
            //{
            //    // This will not work with null. We need to pass empty string to set the ssn to zero in CWW.
            //    updateCwwRequest.Individual.SSNNumber = "";
            //}

            updateCwwRequest.Individual.SSNNumber = _ssnForCww(ssn);

            updateCwwRequest.Individual.SSNVerificationCode = ssnVerifyCode;
            updateCwwRequest.WorkerId                       = mainframeId;
            updateCwwRequest.IPAddress                      = _iPAddress;
            updateCwwRequest.ExternalAgencyId               = _externalAgencyId;
            updateCwwRequest.Individual.HistorySeqNum       = historySequenceNumber;

            var resCww = _cwwIndSvc.UpdateIndvKeyDemographics(updateCwwRequest);

            if (resCww.Errors != null && resCww.Errors.Length > 0)
            {
                throw new UserFriendlyException("Error Updating CWW Personal Information: " + resCww.Errors[0].ErrorMessage);
            }

            // After updating in CWW lets up in MCI.
            var updateMciRequest = new UpdateRequest1();
            updateMciRequest.UpdateDetails                           = new UpdateRequest();
            updateMciRequest.UpdateDetails.PrimaryName               = new Name();
            updateMciRequest.UpdateDetails.PrimaryName.FirstName     = firstName;
            updateMciRequest.UpdateDetails.PrimaryName.MiddleInitial = middleInitialName;
            updateMciRequest.UpdateDetails.PrimaryName.LastName      = lastName;
            updateMciRequest.UpdateDetails.PrimaryName.SuffixName    = suffixName.NullStringToBlank().Trim().ToUpper();

            // If user does not provide SSN lets set it to 0 for MCI.
            if (ssn.HasValue && isNoSsn != true)
            {
                updateMciRequest.UpdateDetails.SSN = _ssnForMciAsLong(ssn);
            }
            else
            {
                updateMciRequest.UpdateDetails.SSN = 0;
            }

            updateMciRequest.UpdateDetails.Gender              = genderChar;
            updateMciRequest.UpdateDetails.DOB                 = dateOfBirth;
            updateMciRequest.UpdateDetails.SSNVerificationCode = ssnVerifyCode;
            //updateMciRequest.UpdateDetails.ReasonForRequest = "Update"; Breaks 
            updateMciRequest.UpdateDetails.UserName             = mainframeId;
            updateMciRequest.UpdateDetails.PrimaryName.NameType = NameType.PrimaryName;

            if (ssn.HasValue)
            {
                updateMciRequest.UpdateDetails.SSN = (long) ssn;
            }

            updateMciRequest.UpdateDetails.MCIID = (long) mciId;

            var resMci = _mciSvc.Update(updateMciRequest);

            if (resMci.UpdateResult.Errors != null && resMci.UpdateResult.Errors.Length > 0)
            {
                throw new UserFriendlyException("Error Updating MCI Personal Information");
            }
        }

        /// <summary>
        ///     Updates Race and Ethnicity in WWP and CWW.
        ///     1. Update in WWP.
        ///     2. Update in CWW.
        /// </summary>
        private void UpdateRaceAndEthnicity(ref IParticipant participant, bool? isPacificIslander, bool?  isAmericanIndian,      bool?  isAsian, bool? isWhite,
                                            bool?            isBlack,     bool? isHispanic,        string historySequenceNumber, string mainframeId)
        {
            if (participant.MCI_ID == null || participant.PinNumber == null)
            {
                throw new UserFriendlyException("Error Updating CWW Race And Ethnicity; Participant neeeds MCI ID and PinNumber");
            }

            // Update Race and Ethnicity when it changes otherwise do not.
            var hasRaceOrEthnicityChanged = participant.IsHispanic        != isHispanic        ||
                                            participant.IsPacificIslander != isPacificIslander ||
                                            participant.IsAmericanIndian  != isAmericanIndian  ||
                                            participant.IsAsian           != isAsian           ||
                                            participant.IsWhite           != isWhite           ||
                                            participant.IsBlack           != isBlack;

            if (!hasRaceOrEthnicityChanged)
                return;

            // Update Race and Ethnicity when it changes otherwise do not.
            var isRaceOrEthnicityEmpty = isHispanic == null && isPacificIslander == null && isAmericanIndian == null && isAsian == null && isWhite == null && isBlack == null;

            if (isRaceOrEthnicityEmpty)
            {
                // We should not get here because we only update race and ethnicity for existing participants and do not allow a WWP user to blank out the race and ethnicity fields.
                Logger.Warn($"Tried sending Blank CWW Race And Ethnicity {participant?.PinNumber}");
                return;
            }

            // Update Race and Ethnicity in WWP. 
            participant.IsHispanic = isHispanic;

            participant.IsPacificIslander = isPacificIslander;
            participant.IsAmericanIndian  = isAmericanIndian;
            participant.IsAsian           = isAsian;
            participant.IsWhite           = isWhite;
            participant.IsBlack           = isBlack;

            // Update Race And Ethnicity in CWW.
            var raceType = new RaceType
                           {
                               HawaiianOrPacificIslander = isPacificIslander.ToYn(),
                               AmericanIndianOrAlaskan   = isAmericanIndian.ToYn(),
                               Asian                     = isAsian.ToYn(),
                               White                     = isWhite.ToYn(),
                               BlackOrAfricanAmerican    = isBlack.ToYn()
                           };

            var ethnicityType = new EthnicityType
                                {
                                    Hispanic = isHispanic.ToYn()
                                };

            // A pin and mciId should always be 10 digits.
            var pinNumber = participant.PinNumber.ToString().PadLeft(10, '0');
            var mciId     = participant.MCI_ID.ToString().PadLeft(10, '0');

            var req = new UpdateRaceEthnicityInformationRequest(pinNumber, mciId, historySequenceNumber, mainframeId, _externalAgencyId, _iPAddress, raceType, ethnicityType);

            var res = _cwwIndSvc.UpdateRaceEthnicityInformation(req);

            if (res.Errors != null && res.Errors.Length > 0)
            {
                Logger.Error($"Error Updating CWW Race And Ethnicity {res.Errors[0].ErrorMessage}");
                throw new UserFriendlyException("Error Updating CWW Race And Ethnicity");
            }
        }

        /// <summary>
        ///     When a participant exists in MCI, return him/her from WWP or CWW.
        /// </summary>
        /// <param name="mciId"></param>
        /// <param name="firstName"></param>
        /// <param name="middleInitial"></param>
        /// <param name="lastName"></param>
        /// <param name="suffix"></param>
        /// <param name="ssn"></param>
        /// <param name="dateOfBirth"></param>
        /// <param name="genderChar"></param>
        /// <param name="mainframeId"></param>
        /// <returns>A WWP Participant</returns>
        private IParticipant GetExistingMciParticipant(decimal mciId, string firstName, string middleInitial, string lastName, string suffix, decimal? ssn, DateTime dateOfBirth, string genderChar, string mainframeId)
        {
            var part = _repository.GetParticipantByMciId(mciId);

            // We dont have the participant in WWP so lets do some more work to import it from CWW. 
            if (part?.PinNumber == null)
            {
                // Grab pin from CCW.
                var ssnOrZero = ssn == null ? 0 : (long) ssn;
                // Do clear method here. If it does not exist in cww. Import and set selected. Otherwise just import.
                var response = ClearMciParticipant(firstName, middleInitial, lastName, suffix, ssnOrZero, dateOfBirth, genderChar, mainframeId);

                var selectResponse = MarkMCIParticipantAsInCww(firstName, middleInitial, lastName, (long) mciId, response.ClearResult.RequestID, response.ClearResult.Match[0].SequenceNumber, dateOfBirth, genderChar, ssnOrZero);

                if (selectResponse.SelectResult?.Errors != null && selectResponse.SelectResult.Errors.Length >= 1)
                {
                    throw new UserFriendlyException("MCI failed selecting participant");
                }

                var ssnOrNull = ssn == null ? null : (long?) ssn;

                var responseCww = CreateOrGetExistingCwwParticipant(mciId.ToString(), firstName, middleInitial, lastName, suffix, ssnOrNull, dateOfBirth, genderChar, mainframeId);
                part = _repository.GetRefreshedParticipant(responseCww.PINNumber);
            }

            return part;
        }

        /// <summary>
        ///     Creates a brand new WWP participant. A brand new participant in WWP means that we also create one in MCI and then
        ///     CWW.
        ///     Steps in this method:
        ///     1. Create participant in MCI .
        ///     2. Create participant in CWW using MCI ID .
        ///     3. Use PIN generated from CWW to bring participant into WWP via SP.
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="suffix"></param>
        /// <param name="middleInitial"></param>
        /// <param name="lastName"></param>
        /// <param name="ssn"></param>
        /// <param name="dateOfBirth"></param>
        /// <param name="genderChar"></param>
        /// <param name="mainframeId"></param>
        /// <returns>A WWP Participant</returns>
        private IParticipant CreateNewWwpParticipant(string firstName, string middleInitial, string lastName, string suffix, decimal? ssn, DateTime dateOfBirth, string genderChar, string mainframeId)
        {
            // If user does not provide SSN lets set it to 0 for MCI.
            var ssnOrZero = ssn == null ? 0 : (long) ssn;

            // 1. Create in MCI to get MciID.
            var mciId = CreateNewMciParticipant(firstName, middleInitial, lastName, suffix, ssnOrZero, dateOfBirth, genderChar, mainframeId);

            // For our next CWW call we need a valid MCI Id.
            if (mciId == 0)
            {
                throw new UserFriendlyException("MCI Invalid");
            }

            // If user does not provide SSN lets set it to null for CWW.
            var ssnOrNull = (long?) ssn;

            // 2. If new to MCI then new to CWW as well, so lets create our participant in cww.
            var cwwResponse = CreateOrGetExistingCwwParticipant(mciId.ToString(), firstName, middleInitial, lastName, suffix, ssnOrNull, dateOfBirth, genderChar, mainframeId);

            if (cwwResponse.Status == "F")
            {
                Logger.Error($"CWW returned back a F status {cwwResponse.Errors.FirstOrDefault()}");
                throw new UserFriendlyException("CWW failed getting pin");
            }

            // 3. Bring Participant into WWP via SP.
            try
            {
                return _repository.GetRefreshedParticipant(cwwResponse.PINNumber);
            }
            catch (Exception e)
            {
                Logger.Error($"SP failed for importing participant({cwwResponse?.PINNumber}) from CWW {e}");
                throw new UserFriendlyException("SP failed for importing participant from CWW");
            }
        }

        /// <summary>
        ///     Use to create a new participant in MCI.
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="middleInitial"></param>
        /// <param name="lastName"></param>
        /// <param name="suffix"></param>
        /// <param name="ssn">Cannot Pass Null. Null should be a zero.</param>
        /// <param name="dateOfBirth"></param>
        /// <param name="genderChar"></param>
        /// <param name="mainframeId"></param>
        /// <returns>A response that contains the MCI ID</returns>
        private long CreateNewMciParticipant(string firstName, string middleInitial, string lastName, string suffix, long ssn, DateTime dateOfBirth, string genderChar, string mainframeId)
        {
            // We need the request id from a clear to do an establish.
            var response = ClearMciParticipant(firstName, middleInitial, lastName, suffix, ssn, dateOfBirth, genderChar, mainframeId);

            // Any errors lets log and back out.
            if (response == null || (response.ClearResult?.Errors != null && response.ClearResult.Errors.Length > 0))
            {
                throw new UserFriendlyException("MCI Errored Out.");
            }

            // If match is returned, ignore it. The web service's makers prefer we make new participants with clear operation majority of the time.
            if (response.ClearResult?.Match != null && response.ClearResult.Match.Length > 0)
            {
                var estRequest = new EstablishRequest1
                                 {
                                     EstablishDetails = new EstablishRequest
                                                        {
                                                            PrimaryName         = new Name { FirstName = firstName, MiddleInitial = middleInitial, LastName = lastName, SuffixName = suffix?.ToUpper() },
                                                            MinimumMatchScore   = 75,
                                                            RequestID           = response.ClearResult.RequestID,
                                                            SSNVerificationCode = "C",
                                                            DOB                 = dateOfBirth,
                                                            Gender              = genderChar,
                                                            SSN                 = _ssnForMciAsLong(ssn),
                                                            AliasSSN            = "0", // _ssnForMciAsString(0), _ssnForMciAsString(ssn) ?
                                                            UserName            = mainframeId
                                                        }
                                 };

                // Set to 75 to always force it to create a new participant.
                // There seems to be a problem in the connected services code/WCF where we have a proper
                // DateTime of midnight local time at this point (in dateOfBirth variable... the kind is
                // Local), but it is intepreting that time as midnight Greenwich and writing it out
                // in a local format T-05:00... so that gets written to the DB2 tables as a day earlier.
                // We will adjust here for that issue by converting the time to Universal to offset
                // what WCF is doing.

                // We have to send a 0.

                try
                {
                    var establishResponse = _mciSvc.Establish(estRequest);
                    return establishResponse.EstablishResult.MCIID;
                }
                catch (Exception e)
                {
                    throw new UserFriendlyException("MCI Experienced Internal Error.", e);
                }
            }

            return response.ClearResult.MCIID;
        }

        /// <summary>
        ///     Creates a new participant in CWW or returns an existing one.
        ///     As a note this method generates the pin in CWW for new pins.
        /// </summary>
        /// <param name="mciId"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="ssn"></param>
        /// <param name="dateOfBirth"></param>
        /// <param name="genderChar"></param>
        /// <param name="mainframeId"></param>
        /// <returns>A response that contains the Pin Number</returns>
        private InsertIndvKeyDemographicsResponse CreateOrGetExistingCwwParticipant(string mciId, string firstName, string middleInitial, string lastName, string suffix, long? ssn, DateTime dateOfBirth, string genderChar, string mainframeId)
        {
            var socialSecurityNumber = _ssnForCww(ssn);

            var request = new InsertIndvKeyDemographicsRequest
                          {
                              IPAddress        = _iPAddress,
                              ExternalAgencyId = _externalAgencyId,
                              Individual = new IndividualType
                                           {
                                               MCIId               = mciId.PadLeft(10, '0'),
                                               FirstName           = firstName,
                                               MiddleInitial       = middleInitial,
                                               LastName            = lastName,
                                               Suffix              = suffix?.ToUpper(),
                                               SSNNumber           = socialSecurityNumber,
                                               DOB                 = dateOfBirth,
                                               Gender              = genderChar,
                                               SSNVerificationCode = "C"
                                           },
                              WorkerId = mainframeId
                          };

            return _cwwIndSvc.InsertIndvKeyDemographics(request);
        }

        /// <summary>
        ///     Marks in MCI that CWW contains this participant in it's database.
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="middleInitial"></param>
        /// <param name="lastName"></param>
        /// <param name="mciId"></param>
        /// <param name="requestId"></param>
        /// <param name="sequenceNumber"></param>
        /// <param name="dateOfBirth"></param>
        /// <param name="genderChar"></param>
        /// <param name="ssn"></param>
        private SelectResponse MarkMCIParticipantAsInCww(string firstName, string middleInitial, string lastName, long mciId, string requestId, short sequenceNumber, DateTime dateOfBirth, string genderChar, long ssn)
        {
            var request = new SelectRequest1();
            request.RequestDetails             = new SelectRequest();
            request.RequestDetails.PrimaryName = new Name();

            request.RequestDetails.PrimaryName.FirstName     = firstName;
            request.RequestDetails.PrimaryName.MiddleInitial = middleInitial;
            request.RequestDetails.PrimaryName.LastName      = lastName;
            request.RequestDetails.PrimaryName.NameType      = NameType.PrimaryName;
            request.RequestDetails.MCIID                     = mciId;
            request.RequestDetails.RequestID                 = requestId;
            request.RequestDetails.SequenceNumber            = sequenceNumber;
            request.RequestDetails.DOB                       = dateOfBirth;
            request.RequestDetails.SSN                       = _ssnForMciAsLong(ssn);
            request.RequestDetails.Gender                    = genderChar;
            request.RequestDetails.MinimumMatchScore         = 75;
            request.RequestDetails.SSNVerificationCode       = "C";
            request.RequestDetails.UserName                  = _authUser.MainFrameId;
            request.RequestDetails.AliasSSN                  = "0";

            return _mciSvc.Select(request);
        }

        /// <summary>
        /// Keeps Aliases in sync in MCI.
        /// </summary>
        /// <param name="ssn"></param>
        /// <param name="mciId"></param>
        /// <param name="newAliases"></param>
        /// <param name="deletedAliases"></param>
        private void UpdateAliasesInMci(long ssn, long mciId, List<PersonNameContract> newAliases, List<PersonNameContract> deletedAliases)
        {
            // Delete before we add so we dont hit a upper ceiling; max being 6 total.
            // We try to be in sync with MCI otherwise move on. 
            try
            {
                if (deletedAliases != null && deletedAliases.Count(x => !x.IsEmpty()) > 0)
                    // Make sure to not send any empty.
                    DeleteAliasInMci(ssn, mciId, deletedAliases.Where(x => !x.IsEmpty()).ToList());
            }
            catch (Exception e)
            {
                Logger.Error($"Failed Deleting Alias in MCI({mciId}) {deletedAliases} {e}");
            }

            try
            {
                if (newAliases != null && newAliases.Count(x => !x.IsEmpty()) > 0)
                    // Make sure to not send any empty.
                    AddAliasToMci(ssn, mciId, newAliases.Where(x => !x.IsEmpty()).ToList());
            }
            catch (Exception e)
            {
                Logger.Error($"Failed Adding Alias in MCI({mciId}) {newAliases} {e}");
            }
        }

        private AddAliasResponse AddAliasToMci(long ssn, long mciId, List<PersonNameContract> aliases)
        {
            var addAliasRequest = new AddAliasRequest();
            addAliasRequest.RequestDetails          = new AliasRequest();
            addAliasRequest.RequestDetails.SSN      = ssn;
            addAliasRequest.RequestDetails.MCIID    = mciId;
            addAliasRequest.RequestDetails.UserName = _authUser.MainFrameId;

            addAliasRequest.RequestDetails.AliasName = new Name[aliases.Count];

            for (var i = 0; i < aliases.Count; i++)
            {
                addAliasRequest.RequestDetails.AliasName[i]               = new Name();
                addAliasRequest.RequestDetails.AliasName[i].FirstName     = aliases[i].FirstName;
                addAliasRequest.RequestDetails.AliasName[i].MiddleInitial = aliases[i].MiddleInitial;
                addAliasRequest.RequestDetails.AliasName[i].LastName      = aliases[i].LastName;
                addAliasRequest.RequestDetails.AliasName[i].SuffixName    = aliases[i].Suffix == null ? null : aliases[i].Suffix.ToUpper();

                if (aliases[i].IsMaiden)
                {
                    addAliasRequest.RequestDetails.AliasName[i].NameType = NameType.MaidenName;
                }
                else
                {
                    if (aliases[i].IsAlias)
                    {
                        addAliasRequest.RequestDetails.AliasName[i].NameType = NameType.Alias;
                    }
                }
            }

            try
            {
                return _mciSvc.AddAlias(addAliasRequest);
            }
            catch (Exception e)
            {
                throw new UserFriendlyException("MCI had a problem writing back alias", e);
            }
        }

        private RemoveAliasResponse DeleteAliasInMci(long ssn, long mciId, List<PersonNameContract> aliases)
        {
            var removeAliasRequest = new RemoveAliasRequest();
            removeAliasRequest.RequestDetails          = new AliasRequest();
            removeAliasRequest.RequestDetails.MCIID    = mciId;
            removeAliasRequest.RequestDetails.SSN      = _ssnForMciAsLong(ssn);
            removeAliasRequest.RequestDetails.UserName = _authUser.MainFrameId;

            removeAliasRequest.RequestDetails.AliasName = new Name[aliases.Count];

            for (var i = 0; i < aliases.Count; i++)
            {
                removeAliasRequest.RequestDetails.AliasName[i]               = new Name();
                removeAliasRequest.RequestDetails.AliasName[i].FirstName     = aliases[i].FirstName;
                removeAliasRequest.RequestDetails.AliasName[i].MiddleInitial = aliases[i].MiddleInitial;
                removeAliasRequest.RequestDetails.AliasName[i].LastName      = aliases[i].LastName;
                removeAliasRequest.RequestDetails.AliasName[i].SuffixName    = aliases[i].Suffix;
                removeAliasRequest.RequestDetails.AliasName[i].NameType      = AliasFullNameToEnumMci(aliases[i].Alias);
            }

            var removeAliasResponse = _mciSvc.RemoveAlias(removeAliasRequest);

            return removeAliasResponse;
        }

        private ClearResponse ClearMciParticipant(string firstName, string middleInitial, string lastName, string suffix, long ssn, DateTime dateOfBirth, string genderChar, string mainframeId)
        {
            var clearRequest = new ClearRequest
                               {
                                   RequestDetails = new ClearanceRequest
                                                    {
                                                        PrimaryName = new Name
                                                                      {
                                                                          FirstName     = firstName,
                                                                          MiddleInitial = middleInitial,
                                                                          LastName      = lastName,
                                                                          SuffixName    = suffix,
                                                                          NameType      = NameType.PrimaryName
                                                                      },
                                                        SSN              = _ssnForMciAsLong(ssn),
                                                        Gender           = genderChar,
                                                        DOB              = dateOfBirth,
                                                        ReasonForRequest = "50YY",
                                                        UserName         = mainframeId,
                                                        // Ne need to send a zero here. Null will return back fatal response.
                                                        AliasSSN          = "0",
                                                        MinimumMatchScore = 80,
                                                        // Set to always be C.
                                                        SSNVerificationCode = "C"
                                                    }
                               };

            try
            {
                return _mciSvc.Clear(clearRequest);
            }
            catch (Exception e)
            {
                Logger.Error($"{e} Failed during clear call in MCI");
                throw new UserFriendlyException("MCI Experienced Internal Error.", e);
            }
        }

        private string AliasCodeToFullNameCww(string code)
        {
            var fullName = "";

            switch (code)
            {
                case "O":
                    fullName = "Other";
                    break;
                case "M":
                    fullName = "Maiden";
                    break;
                default:
                    fullName = string.Empty;
                    break;
            }

            return fullName;
        }

        private string AliasFullNameToCodeCww(string code)
        {
            var fullName = "";

            switch (code)
            {
                case "Other":
                    fullName = "O";
                    break;
                case "Maiden":
                    fullName = "M";
                    break;
                default:
                    fullName = string.Empty;
                    break;
            }

            return fullName;
        }

        private NameType AliasFullNameToEnumMci(string code)
        {
            switch (code)
            {
                case "Other":
                    return NameType.Alias;
                case "Maiden":
                    return NameType.MaidenName;
                default:
                    return NameType.Alias;
            }
        }

        #endregion
    }
}
