using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class Worker : IWorker
    {
        ICollection<IParticipantEnrolledProgram> IWorker.ParticipantEnrolledPrograms
        {
            get => ParticipantEnrolledPrograms.Cast<IParticipantEnrolledProgram>().ToList();
            set => ParticipantEnrolledPrograms = value.Cast<ParticipantEnrolledProgram>().ToList();
        }

        ICollection<IRecentParticipant> IWorker.RecentParticipants
        {
            get => RecentParticipants.Cast<IRecentParticipant>().ToList();
            set => RecentParticipants = value.Cast<RecentParticipant>().ToList();
        }

        IOrganization IWorker.Organization
        {
            get => Organization;
            set => Organization = (Organization) value;
        }

        ICollection<IConfidentialPinInformation> IWorker.ConfidentialPinInformations
        {
            get => ConfidentialPinInformations.Cast<IConfidentialPinInformation>().ToList();
            set => ConfidentialPinInformations = value.Cast<ConfidentialPinInformation>().ToList();
        }

        ICollection<IWorkerTaskList> IWorker.WorkerTaskLists
        {
            get => WorkerTaskLists.Cast<IWorkerTaskList>().ToList();
            set => WorkerTaskLists = value.Cast<WorkerTaskList>().ToList();
        }
    }
}
