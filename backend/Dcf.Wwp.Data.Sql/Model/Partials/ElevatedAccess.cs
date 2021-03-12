using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class ElevatedAccess : BaseCommonModel, IElevatedAccess
    {
        IElevatedAccessReason IElevatedAccess.ElevatedAccessReason
        {
            get { return ElevatedAccessReason; }
            set { ElevatedAccessReason = (ElevatedAccessReason) value; }
        }

        IParticipant IElevatedAccess.Participant
        {
            get { return Participant; }
            set { Participant = (Participant) value; }
        }

        IWorker IElevatedAccess.Worker
        {
            get { return Worker; }
            set { Worker = (Worker) value; }
        }
    }
}
