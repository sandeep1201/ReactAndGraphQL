using System;

namespace Dcf.Wwp.Model.Interface
{
    public interface IParticipantEnrolledProgramCutOverBridge : IHasId
    {
        int      ParticipantId     { get; set; }
        int      EnrolledProgramId { get; set; }
        DateTime CutOverDate       { get; set; }
        bool     IsDeleted         { get; set; }
        string   ModifiedBy        { get; set; }
        DateTime ModifiedDate      { get; set; }

        IParticipant     Participant     { get; set; }
        IEnrolledProgram EnrolledProgram { get; set; }
    }
}
