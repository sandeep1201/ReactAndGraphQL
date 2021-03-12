using System;

namespace Dcf.Wwp.Model.Interface
{
    public interface IConfidentialPinInformation : IHasId
    {
        #region Properties

        int?     ParticipantId  { get; set; }
        bool?    IsConfidential { get; set; }
        int?     WorkerId       { get; set; }
        bool     IsDeleted      { get; set; }
        decimal? PinNumber      { get; set; }
        string   ModifiedBy     { get; set; }
        DateTime ModifiedDate   { get; set; }

        #endregion

        #region Nav props

        IParticipant Participant { get; set; }
        IWorker      Worker      { get; set; }

        #endregion
    }
}
