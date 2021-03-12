using System;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class ParticipantEnrolledProgramCutOverBridge
    {
        #region Properties

        public int      ParticipantId     { get; set; }
        public int      EnrolledProgramId { get; set; }
        public DateTime CutOverDate       { get; set; }
        public bool     IsDeleted         { get; set; }
        public string   ModifiedBy        { get; set; }
        public DateTime ModifiedDate      { get; set; }

        #endregion

        #region Nav Properties

        public virtual Participant     Participant     { get; set; }
        public virtual EnrolledProgram EnrolledProgram { get; set; }

        #endregion
    }
}
