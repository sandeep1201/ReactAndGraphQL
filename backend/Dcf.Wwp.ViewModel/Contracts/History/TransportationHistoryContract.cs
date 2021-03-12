using System.Collections.Generic;

namespace Dcf.Wwp.Api.Library.Contracts.History
{
    public class TransportationHistoryContract
    {
        public List<HistoryValueContract> TransporationDetails { get; set; }
        public List<HistoryValueContract> IsVehicleInsuredId { get; set; }
        public List<HistoryValueContract> VehicleInsuredDetails { get; set; }
        public List<HistoryValueContract> IsVehicleRegistrationCurrentId { get; set; }
        public List<HistoryValueContract> VehicleRegistrationCurrentDetails { get; set; }
        public List<HistoryValueContract> HasValidDrivingLicense { get; set; }
        public List<HistoryValueContract> DriversLicenseStateId { get; set; }
        public List<HistoryValueContract> DriversLicenseExpirationDate { get; set; }
        public List<HistoryValueContract> DriversLicenseDetails { get; set; }
        public List<HistoryValueContract> DriversLicenseInvalidReasonId { get; set; }
        public List<HistoryValueContract> HadCommercialDriversLicense { get; set; }
        public List<HistoryValueContract> IsCommercialDriversLicenseActive { get; set; }
        public List<HistoryValueContract> CommercialDriversLicenseDetails { get; set; }
        public List<HistoryValueContract> Notes { get; set; }
    }
}