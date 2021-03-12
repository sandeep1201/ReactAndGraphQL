using System;
using System.Collections.Generic;


namespace Dcf.Wwp.Model.Interface
{
    public interface IEducationSection : ICommonDelModel, ICloneable
    {
        int? SchoolGraduationStatusId { get; set; }
        int? SchoolCollegeEstablishmentId { get; set; }
        int? LastGradeLevelCompletedId { get; set; }
        int? CertificateIssuingAuthorityId { get; set; }
        int? CertificateYearAwarded { get; set; }
        bool? HasEverAttendedSchool { get; set; }
        bool? IsCurrentlyEnrolled { get; set; }
        bool? IsWorkingOnCertificate { get; set; }
        bool? HasEducationPlan { get; set; }
        string EducationPlanDetails { get; set; }
        int? LastYearAttended { get; set; }
        string Notes { get; set; }

        ICertificateIssuingAuthority CertificateIssuingAuthority { get; set; }
        ISchoolGraduationStatus SchoolGraduationStatus { get; set; }
        ISchoolCollegeEstablishment SchoolCollegeEstablishment { get; set; }
        ISchoolGradeLevel SchoolGradeLevel { get; set; }
    }
}
