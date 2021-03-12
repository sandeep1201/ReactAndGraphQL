namespace DCF.Core.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("wwp.TimeLimitSummary")]
    public partial class TimeLimitSummary
    {
        public Int32 Id { get; set; }

        public Int32? ParticipantId { get; set; }

        public Int32? FederalUsed { get; set; }

        public Int32? FederalMax { get; set; }

        public Int32? StateUsed { get; set; }

        public Int32? StateMax { get; set; }

        public Int32? CSJUsed { get; set; }

        public Int32? CSJMax { get; set; }

        public Int32? W2TUsed { get; set; }

        public Int32? W2TMax { get; set; }

        public Int32? TMPUsed { get; set; }

        public Int32? TNPUsed { get; set; }

        public Int32? TempUsed { get; set; }

        public Int32? TempMax { get; set; }

        public Int32? CMCUsed { get; set; }

        public Int32? CMCMax { get; set; }

        public Int32? OPCUsed { get; set; }

        public Int32? OPCMax { get; set; }

        public Int32? OtherUsed { get; set; }

        public Int32? OtherMax { get; set; }

        public Int32? OTF { get; set; }

        public Int32? Tribal { get; set; }

        public Int32? TJB { get; set; }

        public Int32? JOBS { get; set; }

        public Int32? NO24 { get; set; }

        [StringLength(4000)]
        public String FactDetails { get; set; }

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

        public virtual Participant Participant { get; set; }
    }
}
