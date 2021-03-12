using System;
using System.Collections.Generic;
using System.Linq;
using DCF.Common.Dates;
using DCF.Common.Exceptions;
using DCF.Common.Extensions;
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

        public List<EAIPVContract> GetIPVs(string pin)
        {
            var decimalPin = decimal.Parse(pin);
            var ipvs       = _eaIpvRepository.GetMany(i => i.Participant.PinNumber == decimalPin && !i.IsDeleted).ToList();
            var contract   = _mapper.Map<List<EAIPVContract>>(ipvs);

            contract.ForEach(i =>
                             {
                                 var ipv = ipvs.FirstOrDefault(j => j.Id == i.Id);

                                 i.MailingAddress   = GetMailingAddressContract(ipv?.EaAlternateMailingAddress);
                                 i.OrganizationCode = ipv?.Organization.EntsecAgencyCode;
                                 i.CountyId         = ipv?.CountyId;
                                 i.CountyName       = ipv?.County?.CountyName;
                             });

            return contract.OrderByDescending(i => i.DeterminationDate).ThenByDescending(i => i.ModifiedDate).ToList();
        }

        public EAIPVContract GetIPV(int id)
        {
            var ipv            = _eaIpvRepository.Get(i => i.Id == id);
            var mailingAddress = ipv.EaAlternateMailingAddress;
            var contract       = _mapper.Map<EAIPVContract>(ipv);

            contract.MailingAddress   = GetMailingAddressContract(mailingAddress);
            contract.OrganizationCode = ipv.Organization.EntsecAgencyCode;
            contract.CountyId         = ipv.CountyId;
            contract.CountyName       = ipv.County?.CountyName;

            return contract;
        }

        public void UpsertIPV(EAIPVContract contract, string pin)
        {
            var modifiedBy     = _authUser.WIUID;
            var modifiedDate   = DateTime.Now;
            var decimalPin     = decimal.Parse(pin);
            var participantId  = _participantRepository.Get(i => i.PinNumber == decimalPin).Id;
            var ipv            = _mapper.Map(contract, contract.Id           == 0 ? _eaIpvRepository.New() : _eaIpvRepository.GetById(contract.Id));
            var overTurnedDate = contract.OverTurnedDate;

            SetPenaltyStartEndDatesAndStatus(ipv, overTurnedDate, new List<int> { participantId }, modifiedDate);

            ipv.ParticipantId = participantId;
            ipv.CountyId      = contract.CountyId;
            ipv.ModifiedBy    = modifiedBy;
            ipv.ModifiedDate  = modifiedDate;

            UpsertReasonsBridge(ipv, contract, modifiedBy, modifiedDate);
            UpsertMailingAddress(ipv, contract, modifiedBy, modifiedDate);


            if (ipv.Id == 0)
            {
                ipv.OrganizationId = _organizationRepository.Get(i => i.EntsecAgencyCode == _authUser.AgencyCode).Id;
                _eaIpvRepository.Add(ipv);
            }

            IPVTransactionalSave(contract, pin, ipv);
        }

        private void IPVTransactionalSave(EAIPVContract contract, string pin, EAIPV ipv)
        {
            using (var tx = _eaIpvRepository.GetDataBase().BeginTransaction())
            {
                try
                {
                    _unitOfWork.Commit();

                    if (contract.Id == 0)
                        GenerateNoticeTrigger(pin, ipv.Id, true, contract.Notes);

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

        private void SetPenaltyStartEndDatesAndStatus(EAIPV ipv, DateTime? overTurnedDate, IEnumerable<int> participantIds, DateTime modifiedDate)
        {
            var noOfOccurrenceMonths = _eaIpvOccurrenceRepository.Get(i => i.Id == ipv.OccurrenceId).NoOfMonths;

            ipv.PenaltyStartDate = RecentActiveApplicationInLastYear(participantIds)?.EaRequest.ApplicationDate.AddMonths(12)
                                   ?? (ipv.DeterminationDate.Day == 1
                                           ? ipv.DeterminationDate
                                           : ipv.DeterminationDate.AddMonths(1).StartOf(DateTimeUnit.Month));
            ipv.PenaltyEndDate = overTurnedDate
                                 ?? (noOfOccurrenceMonths != null
                                         ? ipv.PenaltyStartDate.AddDays(-1).AddMonths((int) noOfOccurrenceMonths)
                                         : (DateTime?) null);

            ipv.StatusId = overTurnedDate != null
                               ? _eaIpvStatusRepository.Get(i => i.Code == _overturned).Id
                               : modifiedDate.IsBefore(ipv.PenaltyStartDate)
                                   ? _eaIpvStatusRepository.Get(i => i.Code == _pending).Id
                                   : modifiedDate.IsSameOrAfter(ipv.PenaltyStartDate) && modifiedDate.IsBefore(ipv.PenaltyEndDate ?? DateTime.MaxValue)
                                       ? _eaIpvStatusRepository.Get(i => i.Code == _active).Id
                                       : modifiedDate.IsSameOrAfter(ipv.PenaltyEndDate ?? DateTime.MaxValue)
                                           ? _eaIpvStatusRepository.Get(i => i.Code == _expired).Id
                                           : _eaIpvStatusRepository.Get(i => i.Code == _pending).Id;
        }

        private void UpsertReasonsBridge(EAIPV ipv, EAIPVContract contract, string modifiedBy, DateTime modifiedDate)
        {
            var ipvReasons         = ipv.EaIpvReasonBridges?.Where(i => !i.IsDeleted).ToList();

            ipvReasons?.ForEach(i => i.IsDeleted = true);

            contract.ReasonIds.ForEach(i =>
                                       {
                                           var ipvReason = ipvReasons?.FirstOrDefault(j => j.ReasonId == i) ?? _eaIpvReasonBridgeRepository.New();

                                           ipvReason.EaIpv        = ipv;
                                           ipvReason.ReasonId     = i;
                                           ipvReason.IsDeleted    = false;
                                           ipvReason.ModifiedBy   = modifiedBy;
                                           ipvReason.ModifiedDate = modifiedDate;

                                           if (ipvReason.Id == 0)
                                               ipv.EaIpvReasonBridges.Add(ipvReason);
                                       });
        }

        private void UpsertMailingAddress(EAIPV ipv, EAIPVContract contract, string modifiedBy, DateTime modifiedDate)
        {
            if (ipv.Id == 0 || ipv.EaAlternateMailingAddress == null)
                ipv.EaAlternateMailingAddress = _eaAlternateMailingAddressRepository.New();

            ipv.EaAlternateMailingAddress.City = _cityDomain.GetOrCreateCity(user: _authUser.Username, finalistAddress: contract.MailingAddress, isClientReg: true);

            // In case we have a city that was deleted, we need to restore it.
            ipv.EaAlternateMailingAddress.City.IsDeleted                  = false;
            ipv.EaAlternateMailingAddress.AddressLine1                    = contract.MailingAddress.AddressLine1;
            ipv.EaAlternateMailingAddress.ZipCode                         = contract.MailingAddress.Zip;
            ipv.EaAlternateMailingAddress.AddressVerificationTypeLookupId = contract.MailingAddress.UseSuggestedAddress ? AddressVerificationType.FinalistVerifiedId : AddressVerificationType.WorkerOverrideId;
            ipv.EaAlternateMailingAddress.ModifiedBy                      = modifiedBy;
            ipv.EaAlternateMailingAddress.ModifiedDate                    = modifiedDate;
        }

        #endregion
    }
}
