using Dcf.Wwp.Model.Interface;
using System;
using System.ComponentModel.DataAnnotations;

namespace Dcf.Wwp.Data.Sql.Model
{
    [MetadataType(typeof(ModelExtension))]
    public partial class EducationSection : BaseCommonModel, IEducationSection, IEquatable<EducationSection>
    {
        ICertificateIssuingAuthority IEducationSection.CertificateIssuingAuthority
        {
            get { return CertificateIssuingAuthority; }

            set { CertificateIssuingAuthority = (CertificateIssuingAuthority) value; }
        }

        ISchoolCollegeEstablishment IEducationSection.SchoolCollegeEstablishment
        {
            get { return SchoolCollegeEstablishment; }
            set { SchoolCollegeEstablishment = (SchoolCollegeEstablishment) value; }
        }

        ISchoolGradeLevel IEducationSection.SchoolGradeLevel
        {
            get { return SchoolGradeLevel; }
            set { SchoolGradeLevel = (SchoolGradeLevel) value; }
        }

        ISchoolGraduationStatus IEducationSection.SchoolGraduationStatus
        {
            get { return SchoolGraduationStatus; }
            set { SchoolGraduationStatus = (SchoolGraduationStatus) value; }
        }

        #region ICloneable

        public object Clone()
        {
            var es = new EducationSection();

            es.Id                            = this.Id;
            es.IsCurrentlyEnrolled           = this.IsCurrentlyEnrolled;
            es.IsWorkingOnCertificate        = this.IsWorkingOnCertificate;
            es.LastYearAttended              = this.LastYearAttended;
            es.Notes                         = this.Notes;
            es.HasEverAttendedSchool         = this.HasEverAttendedSchool;
            es.CertificateYearAwarded        = this.CertificateYearAwarded;
            es.SchoolGraduationStatusId      = this.SchoolGraduationStatusId;
            es.SchoolCollegeEstablishmentId  = this.SchoolCollegeEstablishmentId;
            es.LastGradeLevelCompletedId     = this.LastGradeLevelCompletedId;
            es.CertificateIssuingAuthorityId = this.CertificateIssuingAuthorityId;
            es.HasEducationPlan              = this.HasEducationPlan;
            es.EducationPlanDetails          = this.EducationPlanDetails;
            es.SchoolCollegeEstablishment    = (SchoolCollegeEstablishment) this.SchoolCollegeEstablishment?.Clone();
            es.SchoolGraduationStatus        = (SchoolGraduationStatus) this.SchoolGraduationStatus?.Clone();
            //es.School = (School)this.School?.Clone();
            es.SchoolGradeLevel            = (SchoolGradeLevel) this.SchoolGradeLevel?.Clone();
            es.CertificateIssuingAuthority = (CertificateIssuingAuthority) this.CertificateIssuingAuthority?.Clone();

            return es;
        }

        #endregion ICloneable

        #region IEquatable<T>

        public override bool Equals(object other)
        {
            if (other == null)
                return false;

            var obj = other as EducationSection;
            return obj != null && Equals(obj);
        }

        public bool Equals(EducationSection other)
        {
            // Check whether the compared object is null.
            if (Object.ReferenceEquals(other, null)) return false;

            // Check whether the compared object references the same data.
            if (Object.ReferenceEquals(this, other)) return true;

            // Check whether the products' properties are equal.
            // We have to be careful doing comparisons on null object properties.
            if (!AdvEqual(Id, other.Id))
                return false;
            if (!AdvEqual(IsCurrentlyEnrolled, other.IsCurrentlyEnrolled))
                return false;
            if (!AdvEqual(SchoolGraduationStatusId, other.SchoolGraduationStatusId))
                return false;
            if (!AdvEqual(LastGradeLevelCompletedId, other.LastGradeLevelCompletedId))
                return false;
            if (!AdvEqual(CertificateIssuingAuthorityId, other.CertificateIssuingAuthorityId))
                return false;
            if (!AdvEqual(CertificateYearAwarded, other.CertificateYearAwarded))
                return false;
            if (!AdvEqual(HasEverAttendedSchool, other.HasEverAttendedSchool))
                return false;
            if (!AdvEqual(IsWorkingOnCertificate, other.IsWorkingOnCertificate))
                return false;
            if (!AdvEqual(LastYearAttended, other.LastYearAttended))
                return false;
            if (!AdvEqual(Notes, other.Notes))
                return false;
            if (!AdvEqual(SchoolCollegeEstablishmentId, other.SchoolCollegeEstablishmentId))
                return false;
            if (!AdvEqual(SchoolCollegeEstablishment, other.SchoolCollegeEstablishment))
                return false;
            if (!AdvEqual(SchoolGraduationStatus, other.SchoolGraduationStatus))
                return false;
            if (!AdvEqual(SchoolGradeLevel, other.SchoolGradeLevel))
                return false;
            if (!AdvEqual(CertificateIssuingAuthority, other.CertificateIssuingAuthority))
                return false;
            if (!AdvEqual(HasEducationPlan, other.HasEducationPlan))
                return false;
            if (!AdvEqual(EducationPlanDetails, other.EducationPlanDetails))
                return false;

            return true;
        }

        #endregion IEquatable<T>
    }
}
