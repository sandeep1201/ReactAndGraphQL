using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class TransportationSection
    {
        #region Properties

        public int       ParticipantId                     { get; set; }
        public string    TransporationDetails              { get; set; }
        public int?      IsVehicleInsuredId                { get; set; }
        public string    VehicleInsuredDetails             { get; set; }
        public int?      IsVehicleRegistrationCurrentId    { get; set; }
        public string    VehicleRegistrationCurrentDetails { get; set; }
        public bool?     HasValidDrivingLicense            { get; set; }
        public int?      DriversLicenseStateId             { get; set; }
        public DateTime? DriversLicenseExpirationDate      { get; set; }
        public string    DriversLicenseDetails             { get; set; }
        public int?      DriversLicenseInvalidReasonId     { get; set; }
        public string    DriversLicenseInvalidDetails      { get; set; }
        public bool?     HadCommercialDriversLicense       { get; set; }
        public bool?     IsCommercialDriversLicenseActive  { get; set; }
        public string    CommercialDriversLicenseDetails   { get; set; }
        public string    Notes                             { get; set; }
        public bool      IsDeleted                         { get; set; }
        public string    ModifiedBy                        { get; set; }
        public DateTime? ModifiedDate                      { get; set; }

        #endregion

        #region Navigation Properties

        public virtual Participant                                    Participant                                    { get; set; }
        public virtual YesNoUnknownLookup                             IsVehicleInsuredYesNoUnknownLookup             { get; set; }
        public virtual YesNoUnknownLookup                             IsVehicleRegistrationCurrentYesNoUnknownLookup { get; set; }
        public virtual State                                          State                                          { get; set; }
        public virtual DriversLicenseInvalidReasonType                DriversLicenseInvalidReasonType                { get; set; }
        public virtual ICollection<TransportationSectionMethodBridge> TransportationSectionMethodBridges             { get; set; } = new List<TransportationSectionMethodBridge>();

        #endregion
    }
}
