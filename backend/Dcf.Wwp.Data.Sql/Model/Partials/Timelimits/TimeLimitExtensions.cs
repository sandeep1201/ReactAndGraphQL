using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class TimeLimitExtension : BaseCommonModel, ITimeLimitExtension
    {
        IApprovalReason ITimeLimitExtension.ApprovalReason
        {
            get { return this.ApprovalReason; }

            set { this.ApprovalReason = value as ApprovalReason; }
        }


        IDenialReason ITimeLimitExtension.DenialReason
        {
            get { return this.DenialReason; }

            set { this.DenialReason = value as DenialReason; }
        }

        IParticipant ITimeLimitExtension.Participant
        {
            get { return this.Participant; }

            set { this.Participant = value as Participant; }
        }


        //ITimeLimitType ITimeLimitExtension.TimeLimitType
        //{
        //    get { return this.TimeLimitType; }

        //    set { this.TimeLimitType = value as TimeLimitType; }
        //}
    }
}
