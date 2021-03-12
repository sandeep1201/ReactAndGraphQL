using System;

namespace Dcf.Wwp.Model.Interface
{
    public interface ITimeLimitExtension : ICommonDelCreatedModel
    {
        Int32? ParticipantId { get; set; }
        Int32? ExtensionDecisionId { get; set; }
        Int32? TimeLimitTypeId { get; set; }
        DateTime? InitialDiscussionDate { get; set; }
        Int32? ApprovalReasonId { get; set; }
        Int32? DenialReasonId { get; set; }
        String Details { get; set; }
        Boolean? IsPendingDVR { get; set; }
        Boolean? IsReceivingDVR { get; set; }
        Boolean? IsPendingSSIorSSDI { get; set; }
        DateTime? BeginMonth { get; set; }
        DateTime? EndMonth { get; set; }
        Int32? ExtensionSequence { get; set; }
        Int32? DeleteReasonId { get; set; }
        String Notes { get; set; }
        IApprovalReason ApprovalReason { get; set; }
        IDenialReason DenialReason { get; set; }
        IParticipant Participant { get; set; }
        //ITimeLimitType TimeLimitType { get; set; }
        DateTime? DecisionDate { get; set; }
        Int32? IsBackDatedExtenstion { get; set; }
    }
}