using System.Collections.Generic;

namespace Dcf.Wwp.Model.Interface
{
    public interface ICompletionReason : ICommonModelFinal
    {
        #region Properties

        int    EnrolledProgramId { get; set; }
        string Code              { get; set; }
        string Name              { get; set; }
        int    SortOrder         { get; set; }
        bool   IsSystemUseOnly   { get; set; }

        #endregion

        #region Navigation Properties

        IEnrolledProgram                         EnrolledProgram             { get; set; }
        ICollection<IParticipantEnrolledProgram> ParticipantEnrolledPrograms { get; set; }

        #endregion
    }
}
