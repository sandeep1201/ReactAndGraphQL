using System;
using Dcf.Wwp.DataAccess.Base;

namespace Dcf.Wwp.DataAccess.Models
{
    public class EAPayment : BaseEntity
    {
        #region Properties

        public int       RequestId            { get; set; }
        public string    VoucherOrCheckNumber { get; set; }
        public DateTime? VoucherOrCheckDate   { get; set; }
        public decimal   VoucherOrCheckAmount { get; set; }
        public string    PayeeName            { get; set; }
        public int?      MailingAddressId     { get; set; }
        public string    Notes                { get; set; }
        public bool      IsDeleted            { get; set; }
        public string    ModifiedBy           { get; set; }
        public DateTime  ModifiedDate         { get; set; }

        #endregion

        #region Navigation Properties

        public virtual EARequest                 EaRequest                 { get; set; }
        public virtual EAAlternateMailingAddress EaAlternateMailingAddress { get; set; }

        #endregion
    }
}
