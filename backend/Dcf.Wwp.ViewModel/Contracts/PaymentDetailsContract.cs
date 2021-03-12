using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Api.Library.Contracts
{
    public class PaymentDetailsContract
    {
        public List<ParticipantPaymentHistoryContract> ParticipantPaymentHistories { get; set; }
        public List<ManualAuxiliary>                   ManualAuxiliaries           { get; set; }
        public UnAppliedSanctionableHours              UnAppliedSanctionableHours  { get; set; }
    }

    public class ParticipantPaymentHistoryContract
    {
        public decimal? CaseNumber                { get; set; }
        public int      EffectiveMonth            { get; set; }
        public DateTime ParticipationBeginDate    { get; set; }
        public DateTime ParticipationEndDate      { get; set; }
        public decimal? BaseW2Payment             { get; set; }
        public decimal? DrugFelonPenalty          { get; set; }
        public decimal? Recoupment                { get; set; }
        public decimal? LearnfarePenalty          { get; set; }
        public decimal? AdjustedBasePayment       { get; set; }
        public decimal? NonParticipationReduction { get; set; }
        public decimal? FinalPayment              { get; set; }
        public decimal? VendorPayment             { get; set; }
        public decimal? ParticipantPayment        { get; set; }
        public decimal? OverPayment               { get; set; }
        public bool     IsOriginal                { get; set; }
        public bool     IsDelayed                 { get; set; }
        public DateTime CreatedDate               { get; set; }
        public string   ModifiedBy                { get; set; }
        public DateTime ModifiedDate              { get; set; }
        public bool?    IsDeleted                 { get; set; }
    }

    public class ManualAuxiliary
    {
        public string Reason       { get; set; }
        public string Amount       { get; set; }
        public string ApprovalDate { get; set; }
    }

    public class UnAppliedSanctionableHours
    {
        public decimal? UnAppliedHours { get; set; }
        public string   LastUpdated    { get; set; }
    }
}
