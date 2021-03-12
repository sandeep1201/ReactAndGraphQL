using System;

namespace Dcf.Wwp.Model.Interface
{
    public interface ITimeLimitSummary : ICommonDelCreatedModel
    {
         Int32 Id { get; set; }
         Int32? ParticipantId { get; set; }
         Int32? FederalUsed { get; set; }
         Int32? FederalMax { get; set; }
         Int32? StateUsed { get; set; }
         Int32? StateMax { get; set; }
         Int32? CSJUsed { get; set; }
         Int32? CSJMax { get; set; }
         Int32? W2TUsed { get; set; }
         Int32? W2TMax { get; set; }
         Int32? TMPUsed { get; set; }
         Int32? TNPUsed { get; set; }
         Int32? TempUsed { get; set; }
         Int32? TempMax { get; set; }
         Int32? CMCUsed { get; set; }
         Int32? CMCMax { get; set; }
         Int32? OPCUsed { get; set; }
         Int32? OPCMax { get; set; }
         Int32? OtherUsed { get; set; }
         Int32? OtherMax { get; set; }
         Int32? OTF { get; set; }
         Int32? Tribal { get; set; }
         Int32? TJB { get; set; }
         Int32? JOBS { get; set; }
         Int32? NO24 { get; set; }
         String FactDetails { get; set; }
         Boolean? CSJExtensionDue { get; set; }
         Boolean? W2TExtensionDue { get; set; }
         Boolean? TempExtensionDue { get; set; }
         Boolean? StateExtensionDue { get; set; }

        IParticipant Participant { get; set; }
    }
}