using System;

namespace DCF.Timelimits.Rules.Domain
{
    public class AuxPayment
    {
        public DateTime IssueDate { get; }
        public W2Placement Placement { get; }
        public Int32 PaymentAmount { get; }

        public AuxPayment(Int32 paymentAmount, W2Placement placement, DateTime issueDate)
        {
            this.PaymentAmount = paymentAmount;
            this.Placement = placement;
            this.IssueDate = issueDate;
        }

        
    }
}