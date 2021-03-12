using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class CompletionReason
    {
        #region Properties

        public int      EnrolledProgramId { get; set; }
        public string   Code              { get; set; }
        public string   Name              { get; set; }
        public int      SortOrder         { get; set; }
        public bool     IsDeleted         { get; set; }
        public string   ModifiedBy        { get; set; }
        public DateTime ModifiedDate      { get; set; }
        public bool     IsSystemUseOnly   { get; set; }

        #endregion

        #region Navigation Properties

        public virtual EnrolledProgram                         EnrolledProgram             { get; set; }
        public virtual ICollection<ParticipantEnrolledProgram> ParticipantEnrolledPrograms { get; set; }

        #endregion
    }
}
