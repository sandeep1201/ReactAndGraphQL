using System;
using System.Collections.Generic;
using System.Linq;
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

        private List<EAPaymentContract> GetPayments(IEnumerable<EAPayment> payments)
        {
            var contract = _mapper.Map<List<EAPaymentContract>>(payments);

            contract.ForEach(i => i.MailingAddress = GetMailingAddressContract(payments.FirstOrDefault(j => j.Id == i.Id)?.EaAlternateMailingAddress));

            return contract;
        }

        public EAPaymentContract GetPayment(int id, EAPayment payment = null)
        {
            payment = payment ?? _eaPaymentRepository.Get(i => i.Id == id);

            var mailingAddress = payment.EaAlternateMailingAddress;
            var contract       = _mapper.Map<EAPaymentContract>(payment);

            contract.VoucherOrCheckAmount = payment.VoucherOrCheckAmount.ToString("N2");
            contract.MailingAddress       = GetMailingAddressContract(mailingAddress);

            return contract;
        }

        public EAPaymentContract UpsertPayment(EAPaymentContract contract)
        {
            var modifiedBy   = _authUser.WIUID;
            var modifiedDate = DateTime.Now;
            var payment      = _mapper.Map(contract, contract.Id == 0 ? _eaPaymentRepository.New() : _eaPaymentRepository.GetById(contract.Id));

            payment.ModifiedBy   = modifiedBy;
            payment.ModifiedDate = modifiedDate;

            if (payment.Id == 0 || payment.EaAlternateMailingAddress == null)
                payment.EaAlternateMailingAddress = _eaAlternateMailingAddressRepository.New();

            payment.EaAlternateMailingAddress.City = _cityDomain.GetOrCreateCity(user: _authUser.Username, finalistAddress: contract.MailingAddress, isClientReg: true);

            // In case we have a city that was deleted, we need to restore it.
            payment.EaAlternateMailingAddress.City.IsDeleted                  = false;
            payment.EaAlternateMailingAddress.AddressLine1                    = contract.MailingAddress.AddressLine1;
            payment.EaAlternateMailingAddress.ZipCode                         = contract.MailingAddress.Zip;
            payment.EaAlternateMailingAddress.AddressVerificationTypeLookupId = contract.MailingAddress.UseSuggestedAddress ? AddressVerificationType.FinalistVerifiedId : AddressVerificationType.WorkerOverrideId;
            payment.EaAlternateMailingAddress.ModifiedBy                      = modifiedBy;
            payment.EaAlternateMailingAddress.ModifiedDate                    = modifiedDate;

            if (payment.Id == 0)
                _eaPaymentRepository.Add(payment);

            _unitOfWork.Commit();

            return GetPayment(payment.Id, payment);
        }

        #endregion
    }
}
