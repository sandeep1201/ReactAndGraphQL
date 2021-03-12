namespace DCF.Core.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("wwp.ParticipantEnrolledProgram")]
    public partial class ParticipantEnrolledProgram
    {
        public Int32 Id { get; set; }

        public Int32 ParticipantId { get; set; }

        public Int32? EnrolledProgramId { get; set; }

        public Decimal? RFANumber { get; set; }

        [Column(TypeName = "date")]
        public DateTime? RFADate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? EnrollmentDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DisenrollmentDate { get; set; }

        [StringLength(1)]
        public String CurrentRegCode { get; set; }

        [StringLength(2)]
        public String CompletionReasonCode { get; set; }

        [Column(TypeName = "date")]
        public DateTime? CompletionDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? WorkProgramReferralDate { get; set; }

        [StringLength(1)]
        public String AuditIndicator { get; set; }

        [Column(TypeName = "date")]
        public DateTime? CourtOrderedDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ConversionDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [StringLength(50)]
        public String ModifiedBy { get; set; }

        public Boolean IsDeleted { get; set; }

        [Column(TypeName = "timestamp")]
        [MaxLength(8)]
        [Timestamp]
        public Byte[] RowVersion { get; set; }

        public Int16? AGSequenceNumber { get; set; }

        [StringLength(6)]
        public String CaseManagerId { get; set; }

        public Decimal? CASENumber { get; set; }

        public Int16? CountyNumber { get; set; }

        public Int32? OfficeNumber { get; set; }

        public Int16? CourtOrderedCountyNumber { get; set; }

        [StringLength(2)]
        public String SpecialCircumstancesCode { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DisenrollmentCode { get; set; }

        [Column(TypeName = "date")]
        public DateTime? LastContactDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? LastDisenrollmentDate { get; set; }

        [StringLength(1)]
        public String ReferralRegistrationCode { get; set; }

        [StringLength(1)]
        public String StatusCode { get; set; }

        public Int16? RegionNumer { get; set; }

        public Decimal? CensusTractNumber { get; set; }

        public Decimal? LatitudeNumber { get; set; }

        public Decimal? LongitudeNumber { get; set; }

        public Int16? WorkProgramGeoArea { get; set; }

        public Int16? OverrideWorkProgramGeoArea { get; set; }

        public virtual EnrolledProgram EnrolledProgram { get; set; }

        public virtual Participant Participant { get; set; }
    }
}
