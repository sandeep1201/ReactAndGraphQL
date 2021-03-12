using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Api.Library.Contracts.EmergencyAssistance
{
    public class EAPaymentContract
    {
        public int                     Id                   { get; set; }
        public int                     RequestId            { get; set; }
        public string                  VoucherOrCheckNumber { get; set; }
        public DateTime?               VoucherOrCheckDate   { get; set; }
        public string                  VoucherOrCheckAmount { get; set; }
        public string                  PayeeName            { get; set; }
        public FinalistAddressContract MailingAddress       { get; set; }
        public string                  Notes                { get; set; }
        public bool                    IsDeleted            { get; set; }
        public string                  ModifiedBy           { get; set; }
        public DateTime                ModifiedDate         { get; set; }
    }
}
