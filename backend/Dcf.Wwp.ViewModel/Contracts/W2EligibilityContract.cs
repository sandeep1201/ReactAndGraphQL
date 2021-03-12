using System;

namespace Dcf.Wwp.Api.Library.Contracts
{
    public class W2EligibilityContract
    {
        public string PlacementCode { get; set; }
        public int? DaysInPlacement { get; set; }
        public string StateLifeTimeLimit { get; set; }
        public DateTime? EPReviewDueDate { get; set; }
        public DateTime? ReviewDueDate { get; set; }
        public bool TwoParentStatus { get; set; }
        public bool LearnFareStatus { get; set; }
        public string AGStatuseCode { get; set; }
        public short? AGSequenceNumber { get; set; }
        public DateTime? EligibilityBeginDate { get; set; }
        public DateTime? EligibilityEndDate { get; set; }
        public DateTime? PaymentBeginDate { get; set; }
        public DateTime? PaymentEndDate { get; set; }
        public string AGFailureReasonCode1 { get; set; }
        public string AGFailureReasonCode2 { get; set; }
        public string AGFailureReasonCode3 { get; set; }
        public bool? FSAgOpen { get; set; }
        public bool? MAAgOpen { get; set; }
        public bool? FPWAgOpen { get; set; }
        public bool? CCAgOpen { get; set; }
        public string FsetStatus { get; set; }
        public bool? ChildSupportStatus { get; set; }
        public bool? MoreThanSixIndv { get; set; }
  
    }
}
