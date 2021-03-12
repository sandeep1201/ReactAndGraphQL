using Dcf.Wwp.Model.Interface;
using Newtonsoft.Json;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class TimeLimitSummary : BaseCommonModel, ITimeLimitSummary
    {
        [JsonIgnore]
        IParticipant ITimeLimitSummary.Participant
        {
            get { return this.Participant; }

            set { this.Participant = value as Participant; }
        }
    }
}
