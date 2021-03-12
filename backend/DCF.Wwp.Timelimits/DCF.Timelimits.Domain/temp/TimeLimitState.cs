namespace DCF.Core.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("wwp.TimeLimitState")]
    public partial class TimeLimitState
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TimeLimitState()
        {
            TimeLimits = new HashSet<TimeLimit>();
        }

        public Int32 Id { get; set; }

        [StringLength(100)]
        public String Code { get; set; }

        [StringLength(100)]
        public String Name { get; set; }

        public Int32? CountryId { get; set; }

        [Required]
        [StringLength(50)]
        public String ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [Column(TypeName = "timestamp")]
        [MaxLength(8)]
        [Timestamp]
        public Byte[] RowVersion { get; set; }

        public Boolean IsDeleted { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TimeLimit> TimeLimits { get; set; }
    }
}
