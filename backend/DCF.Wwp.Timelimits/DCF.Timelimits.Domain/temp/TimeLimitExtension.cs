namespace DCF.Core.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("wwp.TimeLimitExtension")]
    public partial class TimeLimitExtension
    {
        public Int32 Id { get; set; }

        public Int32? ParticipantId { get; set; }

        public Int32? ExtensionDecisionId { get; set; }

        public Int32? TimeLimitTypeId { get; set; }

        public DateTime? DecisionDate { get; set; }

        public DateTime? InitialDiscussionDate { get; set; }

        public Int32? ApprovalReasonId { get; set; }

        public Int32? DenialReasonId { get; set; }

        [StringLength(1000)]
        public String Details { get; set; }

        public Boolean? IsPendingDVR { get; set; }

        public Boolean? IsReceivingDVR { get; set; }

        public Boolean? IsPendingSSIorSSDI { get; set; }

        public DateTime? BeginMonth { get; set; }

        public DateTime? EndMonth { get; set; }

        public Int32? ExtensionSequence { get; set; }

        public Int32? DeleteReasonId { get; set; }

        [StringLength(1000)]
        public String Notes { get; set; }

        public Boolean IsDeleted { get; set; }

        public DateTime? CreatedDate { get; set; }

        [Required]
        [StringLength(100)]
        public String ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [Column(TypeName = "timestamp")]
        [MaxLength(8)]
        [Timestamp]
        public Byte[] RowVersion { get; set; }

        public Decimal? PIN_NUM { get; set; }

        public virtual ApprovalReason ApprovalReason { get; set; }

        public virtual DeleteReason DeleteReason { get; set; }

        public virtual DenialReason DenialReason { get; set; }

        public virtual Participant Participant { get; set; }

        public virtual TimeLimitType TimeLimitType { get; set; }
    }
}
