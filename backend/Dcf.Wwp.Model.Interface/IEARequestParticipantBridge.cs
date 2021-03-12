using System;

namespace Dcf.Wwp.Model.Interface
{
    public interface IEARequestParticipantBridge : ICommonModelFinal, ICloneable
    {
        int          ParticipantId      { get; set; }
        int          EARequestId        { get; set; }
        int?         EAIndividualTypeId { get; set; }
        int?         EARelationTypeId   { get; set; }
        bool?        IsIncluded         { get; set; }
        DateTime?    SSNAppliedDate     { get; set; }
        int?         SSNExemptTypeId    { get; set; }
        IParticipant Participant        { get; set; }
    }
}
