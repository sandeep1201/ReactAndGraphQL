using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class ConfidentialPinInformation : BaseEntity, IConfidentialPinInformation
    {
        IParticipant IConfidentialPinInformation.Participant
        {
            get { return Participant; }
            set { Participant = (Participant) value; }
        }

        IWorker IConfidentialPinInformation.Worker
        {
            get { return Worker; }
            set { Worker = (Worker) value; }
        }
    }
}
