using System;

namespace Dcf.Wwp.Model.Interface
{
    public interface IElevatedAccess : ICommonModel
    {
        #region Properties

        int      WorkerId               { get; set; }
        int      ParticipantId          { get; set; }
        DateTime AccessCreateDate       { get; set; }
        int?     ElevatedAccessReasonId { get; set; }
        string   Details                { get; set; }
        bool     IsDeleted              { get; set; }

        #endregion

        #region Navigation Properties

        IElevatedAccessReason ElevatedAccessReason { get; set; }
        IParticipant          Participant          { get; set; }
        IWorker               Worker               { get; set; }

        #endregion
    }
}
