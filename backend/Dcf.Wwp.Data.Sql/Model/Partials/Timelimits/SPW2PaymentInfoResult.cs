using System;
using System.Globalization;

namespace Dcf.Wwp.Data.Sql.Model
{
    public class SPW2PaymentInfoResult
    {
        private DateTime? _effectivePaymentMonthDateTime;
        public  decimal   CaseNumber            { get; set; }
        public  decimal   EffectivePaymentMonth { get; set; }

        public DateTime EffectivePaymentMonthDateTime
        {
            get
            {
                if (!this._effectivePaymentMonthDateTime.HasValue)
                {
                    this._effectivePaymentMonthDateTime = DateTime.ParseExact(this.EffectivePaymentMonth.ToString(CultureInfo.InvariantCulture), "yyyyMM", CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal);
                }

                return this._effectivePaymentMonthDateTime.Value;
            }
        }

        public DateTime PayPeriodBeginDate    { get; set; }
        public DateTime PayPeriodEndDate      { get; set; }
        public Decimal  OriginalPaymentAmount { get; set; }
        public Decimal  OrignalCheckAmount    { get; set; }
        public Decimal  VendorPayment         { get; set; }
        public Decimal  AdjustedNetAmount     { get; set; }
    }
}
