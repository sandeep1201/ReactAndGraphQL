using System.Collections.Generic;

namespace Dcf.Wwp.Api.Library.Contracts.InformalAssessment
{
    public class PostSecondarySectionContract : BaseInformalAssessmentContract
    {
        public bool? HasAttendedCollege { get; set; }

        public List<PostSecondaryCollegeContract> PostSecondaryColleges { get; set; }

        public bool? HasDegree { get; set; }

        public List<PostSecondaryDegreeContract> PostSecondaryDegrees { get; set; }

        public bool? IsWorkingOnLicensesOrCertificates { get; set; }

        public List<PostSecondaryLicenseContract> PostSecondaryLicenses { get; set; }

        public string PostSecondaryNotes { get; set; }
    }
}
