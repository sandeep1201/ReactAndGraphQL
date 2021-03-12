using System;
using Dcf.Wwp.DataAccess.Base;

namespace Dcf.Wwp.DataAccess.Models
{
    public class ParticipantEnrolledProgramCutOverBridge : BaseEntity
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

        #region Clone

        public ParticipantEnrolledProgramCutOverBridge Clone()
        {
            var bridge = new ParticipantEnrolledProgramCutOverBridge
                         {
                             Id                = Id,
                             IsDeleted         = IsDeleted,
                             ModifiedBy        = ModifiedBy,
                             ModifiedDate      = ModifiedDate,
                             RowVersion        = RowVersion,
                             ParticipantId     = ParticipantId,
                             EnrolledProgramId = EnrolledProgramId,
                             CutOverDate       = CutOverDate
                         };

            return bridge;
        }

        #endregion
    }
}
