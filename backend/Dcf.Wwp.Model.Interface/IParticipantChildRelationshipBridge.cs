using System;

namespace Dcf.Wwp.Model.Interface
{
    public interface IParticipantChildRelationshipBridge : ICommonDelModel
    {
        Int32? ParticipantId { get; set; }
        Int32? ChildId { get; set; }
        Int32? RelationshipId { get; set; }

        //IChild Child { get; set; }
        //IParticipant Participant { get; set; }
        //IRelationship Relationship { get; set; }
    }
}