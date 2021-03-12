using System;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class TimeLimitExtension
    {
        #region Properties

        public int?      ParticipantId         { get; set; }
        public decimal?  PinNum                { get; set; }
        public int?      ExtensionDecisionId   { get; set; }
        public int?      TimeLimitTypeId       { get; set; }
        public DateTime? DecisionDate          { get; set; }
        public DateTime? InitialDiscussionDate { get; set; }
        public int?      ApprovalReasonId      { get; set; }
        public int?      DenialReasonId        { get; set; }
        public string    Details               { get; set; }
        public bool?     IsPendingDVR          { get; set; }
        public bool?     IsReceivingDVR        { get; set; }
        public bool?     IsPendingSSIorSSDI    { get; set; }
        public DateTime? BeginMonth            { get; set; }
        public DateTime? EndMonth              { get; set; }
        public int?      ExtensionSequence     { get; set; }
        public int?      IsBackDatedExtenstion { get; set; }
        public int?      DeleteReasonId        { get; set; }
        public string    Notes                 { get; set; }
        public DateTime? CreatedDate           { get; set; }
        public bool      IsDeleted             { get; set; }
        public string    ModifiedBy            { get; set; }
        public DateTime? ModifiedDate          { get; set; }

        #endregion

        #region Navigation Properties

        public virtual Participant       Participant       { get; set; }
        public virtual ExtensionDecision ExtensionDecision { get; set; }
        public virtual TimeLimitType     TimeLimitType     { get; set; }
        public virtual ApprovalReason    ApprovalReason    { get; set; }
        public virtual DeleteReason      DeleteReason      { get; set; }
        public virtual DenialReason      DenialReason      { get; set; }

        #endregion
    }
}
