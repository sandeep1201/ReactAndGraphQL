using System.Collections.Generic;

namespace Dcf.Wwp.Model.Interface.Repository
{
    public interface IConfidentialPinInformationRepository
    {
        IConfidentialPinInformation              NewConfidentialPinInformation(IParticipant participant);
        ICollection<IConfidentialPinInformation> GetConfidentialPinInformation(int          participantId);
        bool                                     HasConfidentialInfomation(decimal          pin);
    }
}
