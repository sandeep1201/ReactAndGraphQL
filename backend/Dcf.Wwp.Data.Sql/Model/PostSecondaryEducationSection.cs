using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class PostSecondaryEducationSection
    {
        #region Properties

        public int?      ParticipantId                     { get; set; }
        public bool?     DidAttendCollege                  { get; set; }
        public bool?     IsWorkingOnLicensesOrCertificates { get; set; }
        public bool?     DoesHaveDegrees                   { get; set; }
        public string    Notes                             { get; set; }
        public string    ModifiedBy                        { get; set; }
        public DateTime? ModifiedDate                      { get; set; }

        #endregion

        #region Navigation Properties

        public virtual Participant                       Participant           { get; set; }
        public virtual ICollection<PostSecondaryCollege> PostSecondaryColleges { get; set; } = new List<PostSecondaryCollege>();
        public virtual ICollection<PostSecondaryDegree>  PostSecondaryDegrees  { get; set; } = new List<PostSecondaryDegree>();
        public virtual ICollection<PostSecondaryLicense> PostSecondaryLicenses { get; set; } = new List<PostSecondaryLicense>();

        #endregion
    }
}
