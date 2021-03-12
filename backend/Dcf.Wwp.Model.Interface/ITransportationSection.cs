using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dcf.Wwp.Model.Interface
{
    public interface ITransportationSection : ICommonDelModel, ICloneable
    {
        string TransporationDetails { get; set; }
        Nullable<int> IsVehicleInsuredId { get; set; }
        string VehicleInsuredDetails { get; set; }
        Nullable<int> IsVehicleRegistrationCurrentId { get; set; }
        string VehicleRegistrationCurrentDetails { get; set; }
        Nullable<bool> HasValidDrivingLicense { get; set; }
        Nullable<int> DriversLicenseStateId { get; set; }
        Nullable<System.DateTime> DriversLicenseExpirationDate { get; set; }
        string DriversLicenseDetails { get; set; }
        Nullable<int> DriversLicenseInvalidReasonId { get; set; }
        string DriversLicenseInvalidDetails { get; set; }
        Nullable<bool> HadCommercialDriversLicense { get; set; }
        Nullable<bool> IsCommercialDriversLicenseActive { get; set; }
        string CommercialDriversLicenseDetails { get; set; }
        string Notes { get; set; }

        IYesNoUnknownLookup IsVehicleInsuredYesNoUnknownLookup { get; set; }
        IYesNoUnknownLookup IsVehicleRegistrationCurrentYesNoUnknownLookup { get; set; }

        IState State { get; set; }

        IDriversLicenseInvalidReasonType DriversLicenseInvalidReasonType { get; set; }

        ICollection<ITransportationSectionMethodBridge> TransportationSectionMethodBridges { get; set; }
        ICollection<ITransportationSectionMethodBridge> AllTransportationSectionMethodBridges { get; set; }

    }
}
