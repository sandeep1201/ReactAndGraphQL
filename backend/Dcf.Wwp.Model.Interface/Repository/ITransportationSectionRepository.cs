using System.Collections.Generic;

namespace Dcf.Wwp.Model.Interface.Repository
{
    public interface ITransportationSectionRepository
    {
        ITransportationSection NewTransportationSection(int participantId, string user);

        IEnumerable<IDriversLicenseInvalidReasonType> DriversLicenseInvalidReasonTypes();
        IEnumerable<IDriversLicenseInvalidReasonType> AllDriversLicenseInvalidReasonTypes();

        IEnumerable<ITransportationType> TransportationTypes();
    }
}
