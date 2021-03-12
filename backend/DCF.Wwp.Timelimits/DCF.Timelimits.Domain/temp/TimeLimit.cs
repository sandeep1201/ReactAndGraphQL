namespace DCF.Core.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("wwp.TimeLimit")]
    public partial class TimeLimit
    {
        public Int32 Id { get; set; }

        public Int32? ParticipantID { get; set; }

        public DateTime? EffectiveMonth { get; set; }

        public Int32? TimeLimitTypeId { get; set; }

        public Boolean? TwentyFourMonthLimit { get; set; }

        public Boolean? StateTimelimit { get; set; }

        public Boolean? FederalTimeLimit { get; set; }

        public Int32? StateId { get; set; }

        public Int32? ChangeReasonId { get; set; }

        [StringLength(1000)]
        public String ChangeReasonDetails { get; set; }

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

        public virtual ChangeReason ChangeReason { get; set; }

        public virtual Participant Participant { get; set; }

        public virtual TimeLimitState TimeLimitState { get; set; }

        public virtual TimeLimitType TimeLimitType { get; set; }
    }
}
