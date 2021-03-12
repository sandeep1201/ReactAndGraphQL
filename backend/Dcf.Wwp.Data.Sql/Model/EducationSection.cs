using System;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class EducationSection
    {
        #region Properties

        public int       ParticipantId                 { get; set; }
        public int?      SchoolGraduationStatusId      { get; set; }
        public int?      SchoolCollegeEstablishmentId  { get; set; }
        public int?      LastGradeLevelCompletedId     { get; set; }
        public int?      CertificateIssuingAuthorityId { get; set; }
        public int?      CertificateYearAwarded        { get; set; }
        public bool?     HasEverAttendedSchool         { get; set; }
        public bool?     IsCurrentlyEnrolled           { get; set; }
        public bool?     IsWorkingOnCertificate        { get; set; }
        public int?      LastYearAttended              { get; set; }
        public bool?     HasEducationPlan              { get; set; }
        public string    EducationPlanDetails          { get; set; }
        public string    Notes                         { get; set; }
        public bool      IsDeleted                     { get; set; }
        public string    ModifiedBy                    { get; set; }
        public DateTime? ModifiedDate                  { get; set; }

        #endregion

        #region Navigation Properties

        public virtual Participant                 Participant                 { get; set; }
        public virtual SchoolGraduationStatus      SchoolGraduationStatus      { get; set; }
        public virtual SchoolCollegeEstablishment  SchoolCollegeEstablishment  { get; set; }
        public virtual SchoolGradeLevel            SchoolGradeLevel            { get; set; }
        public virtual CertificateIssuingAuthority CertificateIssuingAuthority { get; set; }

        #endregion
    }
}
