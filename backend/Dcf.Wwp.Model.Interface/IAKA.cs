using System;

namespace Dcf.Wwp.Model.Interface
{
    public interface IAKA : ICommonDelModel
    {
        int?         ParticipantId { get; set; } // This is already available in 'Participant' further down... lol
        decimal?     SSNNumber     { get; set; }
        int?         SSNTypeId     { get; set; }
        string       Details       { get; set; }
        DateTime?    CreatedDate   { get; set; }
        IParticipant Participant   { get; set; }
    }
}
