using System.Collections.Generic;
using Dcf.Wwp.Api.Library.Contracts;

namespace Dcf.Wwp.Api.Library.Interfaces
{
    public interface IParticipantActivityDomain
    {
        #region Properties

        #endregion

        #region Methods

        ParticipantActivitiesWebService GetParticipantActivitiesByPins(string pins);

        #endregion
    }
}
