using System.Collections.Generic;

namespace Dcf.Wwp.Api.Library.Contracts.History
{
    public class PostSecondaryHistoryContract
    {
        public List<HistoryValueContract> DidAttendCollege { get; set; }
        public List<HistoryValueContract> IsWorkingOnLicensesOrCertificates { get; set; }
        public List<HistoryValueContract> DoesHaveDegrees { get; set; }

        public List<PostSecondaryCollegeHistoryContract> PostSecondaryColleges { get; set; }
        public List<PostSecondaryDegreeHistoryContract> PostSecondaryDegrees { get; set; }
        public List<PostSecondaryLicenseHistoryContract> PostSecondaryLicenses { get; set; }

        public List<HistoryValueContract> Notes { get; set; }

        public PostSecondaryHistoryContract()
        {
            PostSecondaryColleges = new List<PostSecondaryCollegeHistoryContract>();
            PostSecondaryDegrees = new List<PostSecondaryDegreeHistoryContract>();
            PostSecondaryLicenses = new List<PostSecondaryLicenseHistoryContract>();
        }
    }

    public class PostSecondaryCollegeHistoryContract
    {
        public List<HistoryValueContract> SchoolCollegeEstablishmentId { get; set; }
        public List<HistoryValueContract> Location { get; set; }
        public List<HistoryValueContract> HasGraduated { get; set; }
        public List<HistoryValueContract> LastYearAttended { get; set; }
        public List<HistoryValueContract> CurrentlyAttending { get; set; }
        public List<HistoryValueContract> Semesters { get; set; }
        public List<HistoryValueContract> Credits { get; set; }
        public List<HistoryValueContract> Details { get; set; }
    }

    public class PostSecondaryDegreeHistoryContract
    {
        public List<HistoryValueContract> Name { get; set; }
        public List<HistoryValueContract> College { get; set; }
        public List<HistoryValueContract> DegreeTypeId { get; set; }
        public List<HistoryValueContract> YearAttained { get; set; }
    }

    public class PostSecondaryLicenseHistoryContract
    {
        public List<HistoryValueContract> Name { get; set; }
        public List<HistoryValueContract> Issuer { get; set; }
        public List<HistoryValueContract> AttainedDate { get; set; }
        public List<HistoryValueContract> ExpiredDate { get; set; }
        public List<HistoryValueContract> IsInProgress { get; set; }
        public List<HistoryValueContract> DoesNotExpire { get; set; }
        public List<HistoryValueContract> ValidInWIPolarLookupId { get; set; }
        public List<HistoryValueContract> LicenseTypeId { get; set; }
    }
}
