using System;
using System.ComponentModel.DataAnnotations.Schema;
using Dcf.Wwp.DataAccess.Base;

namespace Dcf.Wwp.DataAccess.Models
{
    public class ParticipantPaymentHistory : BaseEntity
    {
        #region Properties

        public decimal  CaseNumber             { get; set; }
        public int      EffectiveMonth         { get; set; }
        public DateTime ParticipationBeginDate { get; set; }
        public DateTime ParticipationEndDate   { get; set; }
        public decimal  BaseW2Payment          { get; set; }
        public decimal  DrugFelonPenalty       { get; set; }
        public decimal  Recoupment             { get; set; }
        public decimal  LearnFarePenalty       { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public decimal? AdjustedBasePayment { get; set; }

        public decimal  NonParticipationReduction { get; set; }
        public decimal? OverPayment               { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public decimal? FinalPayment { get; set; }

        public decimal  VendorPayment      { get; set; }
        public decimal  ParticipantPayment { get; set; }
        public bool     IsOriginal         { get; set; }
        public DateTime CreatedDate        { get; set; }
        public bool     IsDeleted          { get; set; }
        public string   ModifiedBy         { get; set; }
        public DateTime ModifiedDate       { get; set; }

        #endregion

        #region Navigation Properties

        #endregion

        #region Clone

        #endregion
    }
}
