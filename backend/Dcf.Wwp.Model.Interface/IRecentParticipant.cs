using System;

namespace Dcf.Wwp.Model.Interface
{
    public interface IRecentParticipant
    {
        int       Id            { get; set; }
        int       WorkerId      { get; set; }
        int       ParticipantId { get; set; }
        DateTime  LastAccessed  { get; set; }
        bool      IsDeleted     { get; set; }
        string    ModifiedBy    { get; set; }
        DateTime? ModifiedDate  { get; set; }
        byte[]    RowVersion    { get; set; }

        IParticipant Participant { get; set; }
        IWorker      Worker      { get; set; }
    }
}
