using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class TimeLimit : BaseCommonModel, ITimeLimit
    {
        IChangeReason ITimeLimit.ChangeReason
        {
            get { return this.ChangeReason; }

            set { this.ChangeReason = value as ChangeReason; }
        }

        IParticipant ITimeLimit.Participant
        {
            get { return this.Participant; }

            set { this.Participant = value as Participant; }
        }

        ITimeLimitState ITimeLimit.TimeLimitState
        {
            get { return this.TimeLimitState; }

            set { this.TimeLimitState = value as TimeLimitState; }
        }

        ITimeLimitType ITimeLimit.TimeLimitType
        {
            get { return this.TimeLimitType; }

            set { this.TimeLimitType = value as TimeLimitType; }
        }
    }
}
