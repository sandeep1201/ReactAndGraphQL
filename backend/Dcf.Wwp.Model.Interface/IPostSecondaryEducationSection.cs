using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Model.Interface
{
    public interface IPostSecondaryEducationSection : ICloneable, ICommonModel, IComplexModel
    {
        bool? DidAttendCollege { get; set; }
        bool? IsWorkingOnLicensesOrCertificates { get; set; }
        bool? DoesHaveDegrees { get; set; }
        string Notes { get; set; }

        ICollection<IPostSecondaryCollege> PostSecondaryColleges { get; set; }
        ICollection<IPostSecondaryCollege> AllPostSecondaryColleges { get; set; }

        ICollection<IPostSecondaryDegree> PostSecondaryDegrees { get; set; }
        ICollection<IPostSecondaryDegree> AllPostSecondaryDegrees { get; set; }

        ICollection<IPostSecondaryLicense> PostSecondaryLicenses { get; set; }
        ICollection<IPostSecondaryLicense> AllPostSecondaryLicenses { get; set; }
    }
}
