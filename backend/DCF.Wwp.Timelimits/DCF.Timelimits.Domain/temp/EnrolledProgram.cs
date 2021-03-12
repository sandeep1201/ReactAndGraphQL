namespace DCF.Core.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("wwp.EnrolledProgram")]
    public partial class EnrolledProgram
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public EnrolledProgram()
        {
            ParticipantEnrolledPrograms = new HashSet<ParticipantEnrolledProgram>();
        }

        public Int32 Id { get; set; }

        [StringLength(3)]
        public String ProgramName { get; set; }

        [StringLength(1)]
        public String SubprogramName { get; set; }

        [StringLength(20)]
        public String ProgramType { get; set; }

        [StringLength(100)]
        public String DescriptionText { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [StringLength(50)]
        public String ModifiedBy { get; set; }

        public Boolean IsDeleted { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ParticipantEnrolledProgram> ParticipantEnrolledPrograms { get; set; }
    }
}
