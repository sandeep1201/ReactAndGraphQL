namespace DCF.Core.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("wwp.Participant")]
    public partial class Participant
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Participant()
        {
            ParticipantEnrolledPrograms = new HashSet<ParticipantEnrolledProgram>();
            TimeLimits = new HashSet<TimeLimit>();
            TimeLimitExtensions = new HashSet<TimeLimitExtension>();
            TimeLimitSummaries = new HashSet<TimeLimitSummary>();
        }

        public Int32 Id { get; set; }

        public Decimal? PinNumber { get; set; }

        [StringLength(50)]
        public String FirstName { get; set; }

        [StringLength(1)]
        public String MiddleInitialName { get; set; }

        [StringLength(50)]
        public String LastName { get; set; }

        [StringLength(3)]
        public String SuffixName { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DateOfBirth { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DateOfDeath { get; set; }

        [StringLength(1)]
        public String GenderIndicator { get; set; }

        [StringLength(1)]
        public String AliasResponse { get; set; }

        [StringLength(2)]
        public String BirthVerificationCode { get; set; }

        [StringLength(2)]
        public String BirthPlaceCode { get; set; }

        [StringLength(2)]
        public String CitizenshipVerificationCode { get; set; }

        [StringLength(1)]
        public String DCLCitizenshipSwitch { get; set; }

        [StringLength(2)]
        public String DeathVerificationCode { get; set; }

        [StringLength(1)]
        public String LanguageCode { get; set; }

        public Int16? MaxHistorySequenceNumber { get; set; }

        public Decimal? PrimarySSNNumber { get; set; }

        public Decimal? PseudoSSNNumber { get; set; }

        [StringLength(1)]
        public String RaceCode { get; set; }

        [Column(TypeName = "date")]
        public DateTime? SSNAppointmentDate { get; set; }

        [StringLength(2)]
        public String SSNAppointmentVerificationCode { get; set; }

        [StringLength(2)]
        public String SSNValidatedCode { get; set; }

        [Column(TypeName = "date")]
        public DateTime? CaresUpdatedDate { get; set; }

        [StringLength(1)]
        public String USCitizenSwitch { get; set; }

        [StringLength(2)]
        public String WorkerAlert1Code { get; set; }

        [StringLength(2)]
        public String WorkerAlert2Code { get; set; }

        public Decimal? MaidNumber { get; set; }

        [StringLength(1)]
        public String ChildElsewhereSwitch { get; set; }

        [StringLength(2)]
        public String ChildVerificationCode { get; set; }

        [StringLength(1)]
        public String AmericanIndianIndicator { get; set; }

        [StringLength(1)]
        public String AsianIndicator { get; set; }

        [StringLength(1)]
        public String BlackIndicator { get; set; }

        [StringLength(1)]
        public String HispanicIndicator { get; set; }

        [StringLength(1)]
        public String PacificIslanderIndicator { get; set; }

        [StringLength(1)]
        public String WhiteIndicator { get; set; }

        public Decimal? MCI_ID { get; set; }

        [StringLength(2)]
        public String MACitizenVerificationCode { get; set; }

        [StringLength(1)]
        public String TribeChildMemberIndicator { get; set; }

        [StringLength(2)]
        public String TribeChildVerificationCode { get; set; }

        [StringLength(1)]
        public String TribalMemberIndicator { get; set; }

        [StringLength(2)]
        public String TribalMemberVerificationCode { get; set; }

        [StringLength(2)]
        public String DeathDateSourceCode { get; set; }

        [StringLength(2)]
        public String WorkerOverideVerificationCode { get; set; }

        [StringLength(100)]
        public String ConversionProjectDetails { get; set; }

        public DateTime? ConversionDate { get; set; }

        public Boolean IsDeleted { get; set; }

        public DateTime? CreatedDate { get; set; }

        [StringLength(100)]
        public String ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [Column(TypeName = "timestamp")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [MaxLength(8)]
        public Byte[] RowVersion { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ParticipantEnrolledProgram> ParticipantEnrolledPrograms { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TimeLimit> TimeLimits { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TimeLimitExtension> TimeLimitExtensions { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TimeLimitSummary> TimeLimitSummaries { get; set; }
    }
}
