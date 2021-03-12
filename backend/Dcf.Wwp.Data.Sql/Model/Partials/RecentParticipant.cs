using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class RecentParticipant : IRecentParticipant
    {
        IParticipant IRecentParticipant.Participant
        {
            get { return Participant; }
            set { Participant = (Participant) value; }
        }

        IWorker IRecentParticipant.Worker
        {
            get { return Worker; }
            set { Worker = (Worker) value; }
        }
    }
}
