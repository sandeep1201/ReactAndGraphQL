using System;
using System.Collections.Generic;
using Dcf.Wwp.Api.Library.Contracts.ActionNeeded;

namespace Dcf.Wwp.Api.Library.Contracts.InformalAssessment
{
    public class TransportationSectionContract : BaseInformalAssessmentContract
    {
        #region Properties

        // TODO: Rename to TransportationMethodIds (need to coordinate with NG code)

        public string               MethodDetails                     { get; set; }
        public int?                 IsVehicleInsuredId                { get; set; }
        public string               IsVehicleInsuredName              { get; set; }
        public string               VehicleInsuredDetails             { get; set; }
        public int?                 IsVehicleRegistrationCurrentId    { get; set; }
        public string               IsVehicleRegistrationCurrentName  { get; set; }
        public string               VehicleRegistrationCurrentDetails { get; set; }
        public bool?                HasValidDriversLicense            { get; set; }
        public int?                 DriversLicenseStateId             { get; set; }
        public string               DriversLicenseStateName           { get; set; }
        public DateTime?            DriversLicenseExpirationDate      { get; set; }
        public string               DriversLicenseDetails             { get; set; }
        public int?                 DriversLicenseInvalidReasonId     { get; set; }
        public string               DriversLicenseInvalidReasonName   { get; set; }
        public string               DriversLicenseInvalidDetails      { get; set; }
        public bool?                HadCommercialDriversLicense       { get; set; }
        public bool?                IsCommercialDriversLicenseActive  { get; set; }
        public string               CommercialDriversLicenseDetails   { get; set; }
        public string               Notes                             { get; set; }
        public ActionNeededContract ActionNeeded                      { get; set; }
        public List<int>            Methods                           { get; set; }
        public List<string>         TransportationMethods             { get; set; }
        public string               IsVehicleInsured                  { get; set; } // never used
        public string               IsVehicleRegistrationCurrent      { get; set; } // never used

        #endregion

        #region Methods

        public TransportationSectionContract ()
        {
            Methods               = new List<int>();
            TransportationMethods = new List<string>();
        }

        #endregion
    }
}
