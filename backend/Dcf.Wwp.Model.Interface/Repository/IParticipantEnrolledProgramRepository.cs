using System;

namespace Dcf.Wwp.Model.Interface.Repository
{
    public interface IParticipantEnrolledProgramRepository
    {
        IParticipantEnrolledProgram              NewPep(IRequestForAssistance   rfa, string user);
        IParticipantEnrolledProgram              GetPepById(int                 id);
        ISP_MostRecentFEPFromDB2_Result          GetMostRecentFepDetails(string pin);
    }
}
