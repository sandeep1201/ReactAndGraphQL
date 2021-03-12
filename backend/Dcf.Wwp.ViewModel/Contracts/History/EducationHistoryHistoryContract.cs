using System.Collections.Generic;

namespace Dcf.Wwp.Api.Library.Contracts.History
{
    public class EducationHistoryHistoryContract
    {
        public List<HistoryValueContract> SchoolGraduationStatusId { get; set; }
        public List<HistoryValueContract> Location { get; set; }
        public List<HistoryValueContract> SchoolCollegeEstablishmentId { get; set; }
        public List<HistoryValueContract> LastGradeLevelCompletedId { get; set; }
        public List<HistoryValueContract> CertificateIssuingAuthorityId { get; set; }
        public List<HistoryValueContract> CertificateYearAwarded { get; set; }
        public List<HistoryValueContract> HasEverAttendedSchool { get; set; }
        public List<HistoryValueContract> IsCurrentlyEnrolled { get; set; }
        public List<HistoryValueContract> IsWorkingOnCertificate { get; set; }
        public List<HistoryValueContract> LastYearAttended { get; set; }
        public List<HistoryValueContract> HasEducationPlan { get; set; }
        public List<HistoryValueContract> EducationPlanDetails { get; set; }
        public List<HistoryValueContract> Notes { get; set; }
    }
}