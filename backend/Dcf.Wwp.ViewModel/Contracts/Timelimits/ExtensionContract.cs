using System;
using System.Runtime.Serialization;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Api.Library.Contracts.Timelimits
{
    public class ExtensionContract : BaseModelContract
    {
        [DataMember]
        public Int32 TimelimitTypeId { get; set; }
        [DataMember]
        public Int32 SequenceId { get; set; }
        [DataMember]
        public Int32 ExtensionDecisionId { get; set; }
        [DataMember]
        public DateTimeOffset? StartDate { get; set; }
        [DataMember]
        public DateTimeOffset? EndDate { get; set; }
        [DataMember]
        public DateTimeOffset? DecisionDate { get; set; }
        [DataMember]
        public DateTime? InitialDiscussionDate { get; set; }
        [DataMember]
        public Int32? ApprovalReasonId { get; set; }
        [DataMember]
        public Int32? DenialReasonId { get; set; }
        [DataMember]
        public String ReasonDetails { get; set; }
        [DataMember]
        public Boolean DvrReferralPending { get; set; }
        [DataMember]
        public Boolean RecieivingDvrServices { get; set; }
        [DataMember]
        public Boolean PendingSSAppOrAppeal { get; set; }
        [DataMember]
        public String Notes { get; set; }
        [DataMember]
        public Boolean IsBackdated { get; set; }


        public static ExtensionContract Create(ITimeLimitExtension x)
        {
            var extContract = ExtensionContract.Create(x.Id, x.TimeLimitTypeId.GetValueOrDefault(),
                x.ExtensionSequence.GetValueOrDefault(), x.ExtensionDecisionId.GetValueOrDefault(), x.BeginMonth,
                x.EndMonth, x.DecisionDate, x.CreatedDate, x.InitialDiscussionDate, x.ApprovalReasonId, x.DenialReasonId,
                x.Details, x.IsPendingDVR.GetValueOrDefault(), x.IsReceivingDVR.GetValueOrDefault(),
                x.IsPendingSSIorSSDI.GetValueOrDefault(), x.Notes,x.IsBackDatedExtenstion.GetValueOrDefault() > 0);
            BaseModelContract.SetBaseProperties(extContract,x);
            return extContract;
        }
        private static ExtensionContract Create(Int32 id,Int32 timeLimitTypeId, Int32 sequenceId, Int32 decisionId, DateTime? beginMonth, DateTime? endMonth, DateTime? decisionDate, DateTime? createdDate, DateTime? discussionDate, Int32? approvalReasonId, Int32? denialReasonId, String reasonDetails, Boolean isPendingDvr, Boolean isReceivingDvr, Boolean isPendingSsIorSsdi, String notes, Boolean isBackdated)
        {
            var extContract = new ExtensionContract();
            extContract.TimelimitTypeId = timeLimitTypeId;
            extContract.SequenceId = sequenceId;
            extContract.ExtensionDecisionId = decisionId;
            extContract.StartDate = beginMonth;
            extContract.EndDate = endMonth;
            extContract.DecisionDate = decisionDate;
            extContract.InitialDiscussionDate = discussionDate;
            extContract.ApprovalReasonId = approvalReasonId;
            extContract.DenialReasonId = denialReasonId;
            extContract.ReasonDetails = reasonDetails;
            extContract.DvrReferralPending = isPendingDvr;
            extContract.RecieivingDvrServices = isReceivingDvr;
            extContract.PendingSSAppOrAppeal = isPendingSsIorSsdi;
            extContract.Notes = notes;
            extContract.IsBackdated = isBackdated;

            return extContract;
        }
    }
}