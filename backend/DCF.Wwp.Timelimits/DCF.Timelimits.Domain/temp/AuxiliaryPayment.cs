namespace DCF.Core.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("wwp.AuxiliaryPayment")]
    public partial class AuxiliaryPayment
    {
        public Int32 Id { get; set; }

        public Decimal? PinNumber { get; set; }

        public DateTime? EffectiveMonth { get; set; }

        public Int32? TimeLimitTypeId { get; set; }

        public Boolean? StateTimelimit { get; set; }

        public Boolean? FederalTimeLimit { get; set; }

        public Boolean? TwentyFourMonthLimit { get; set; }

        [Column(TypeName = "date")]
        public DateTime? CreatedDateFromCARES { get; set; }

        [Required]
        [StringLength(100)]
        public String ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [Column(TypeName = "timestamp")]
        [MaxLength(8)]
        [Timestamp]
        public Byte[] RowVersion { get; set; }
    }
}
