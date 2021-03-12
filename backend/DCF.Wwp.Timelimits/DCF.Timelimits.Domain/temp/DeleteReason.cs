namespace DCF.Core.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("wwp.DeleteReason")]
    public partial class DeleteReason
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DeleteReason()
        {
            TimeLimitExtensions = new HashSet<TimeLimitExtension>();
        }

        public Int32 Id { get; set; }

        [StringLength(250)]
        public String Name { get; set; }

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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TimeLimitExtension> TimeLimitExtensions { get; set; }
    }
}
