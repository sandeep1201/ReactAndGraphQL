using System;
using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Model.Interface;


namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class TransportationSection : BaseCommonModel, ITransportationSection, IEquatable<TransportationSection>
    {
        ICollection<ITransportationSectionMethodBridge> ITransportationSection.AllTransportationSectionMethodBridges
        {
            get => TransportationSectionMethodBridges.Cast<ITransportationSectionMethodBridge>().ToList();
            set => TransportationSectionMethodBridges = value.Cast<TransportationSectionMethodBridge>().ToList();
        }

        ICollection<ITransportationSectionMethodBridge> ITransportationSection.TransportationSectionMethodBridges
        {
            get => (from x in TransportationSectionMethodBridges where x.IsDeleted == false select x).Cast<ITransportationSectionMethodBridge>().ToList();
            set => TransportationSectionMethodBridges = value.Cast<TransportationSectionMethodBridge>().ToList();
        }

        IYesNoUnknownLookup ITransportationSection.IsVehicleRegistrationCurrentYesNoUnknownLookup
        {
            get => IsVehicleRegistrationCurrentYesNoUnknownLookup;

            set => IsVehicleRegistrationCurrentYesNoUnknownLookup = (YesNoUnknownLookup) value;
        }

        IYesNoUnknownLookup ITransportationSection.IsVehicleInsuredYesNoUnknownLookup
        {
            get => IsVehicleInsuredYesNoUnknownLookup;

            set => IsVehicleInsuredYesNoUnknownLookup = (YesNoUnknownLookup) value;
        }

        IState ITransportationSection.State
        {
            get => State;

            set => State = (State) value;
        }

        IDriversLicenseInvalidReasonType ITransportationSection.DriversLicenseInvalidReasonType
        {
            get => DriversLicenseInvalidReasonType;

            set => DriversLicenseInvalidReasonType = (DriversLicenseInvalidReasonType) value;
        }

        #region ICloneable

        public object Clone()
        {
            var clone = new TransportationSection()
                        {
                            Id                                = Id,
                            ParticipantId                     = ParticipantId,
                            TransporationDetails              = TransporationDetails,
                            IsVehicleInsuredId                = IsVehicleInsuredId,
                            VehicleInsuredDetails             = VehicleInsuredDetails,
                            IsVehicleRegistrationCurrentId    = IsVehicleRegistrationCurrentId,
                            VehicleRegistrationCurrentDetails = VehicleRegistrationCurrentDetails,
                            HasValidDrivingLicense            = HasValidDrivingLicense,
                            DriversLicenseStateId             = DriversLicenseStateId,
                            DriversLicenseExpirationDate      = DriversLicenseExpirationDate,
                            DriversLicenseDetails             = DriversLicenseDetails,
                            DriversLicenseInvalidReasonId     = DriversLicenseInvalidReasonId,
                            DriversLicenseInvalidDetails      = DriversLicenseInvalidDetails,
                            HadCommercialDriversLicense       = HadCommercialDriversLicense,
                            IsCommercialDriversLicenseActive  = IsCommercialDriversLicenseActive,
                            CommercialDriversLicenseDetails   = CommercialDriversLicenseDetails,
                            Notes                             = Notes,
                            IsDeleted                         = IsDeleted,

                            TransportationSectionMethodBridges = TransportationSectionMethodBridges.Select(x => (TransportationSectionMethodBridge) x.Clone()).ToList()
                        };

            return clone;
        }

        #endregion ICloneable

        #region IEquatable<T>

        public override bool Equals(object other)
        {
            if (other == null)
                return false;

            var obj = other as TransportationSection;
            return obj != null && Equals(obj);
        }

        public bool Equals(TransportationSection other)
        {
            //Check whether the compared object is null.
            if (ReferenceEquals(other, null)) return false;

            //Check whether the compared object references the same data.
            if (ReferenceEquals(this, other)) return true;

            //Check whether the products' properties are equal.

            if (!AdvEqual(Id, other.Id))
                return false;
            if (!AdvEqual(ParticipantId, other.ParticipantId))
                return false;
            if (!AdvEqual(TransporationDetails, other.TransporationDetails))
                return false;
            if (!AdvEqual(IsVehicleInsuredId, other.IsVehicleInsuredId))
                return false;
            if (!AdvEqual(VehicleInsuredDetails, other.VehicleInsuredDetails))
                return false;
            if (!AdvEqual(IsVehicleRegistrationCurrentId, other.IsVehicleRegistrationCurrentId))
                return false;
            if (!AdvEqual(VehicleRegistrationCurrentDetails, other.VehicleRegistrationCurrentDetails))
                return false;
            if (!AdvEqual(HasValidDrivingLicense, other.HasValidDrivingLicense))
                return false;
            if (!AdvEqual(DriversLicenseStateId, other.DriversLicenseStateId))
                return false;
            if (!AdvEqual(DriversLicenseExpirationDate, other.DriversLicenseExpirationDate))
                return false;
            if (!AdvEqual(DriversLicenseDetails, other.DriversLicenseDetails))
                return false;
            if (!AdvEqual(DriversLicenseInvalidReasonId, other.DriversLicenseInvalidReasonId))
                return false;
            if (!AdvEqual(DriversLicenseInvalidDetails, other.DriversLicenseInvalidDetails))
                return false;
            if (!AdvEqual(HadCommercialDriversLicense, other.HadCommercialDriversLicense))
                return false;
            if (!AdvEqual(IsCommercialDriversLicenseActive, other.IsCommercialDriversLicenseActive))
                return false;
            if (!AdvEqual(CommercialDriversLicenseDetails, other.CommercialDriversLicenseDetails))
                return false;
            if (!AdvEqual(Notes, other.Notes))
                return false;
            if (!AdvEqual(IsDeleted, other.IsDeleted))
                return false;

            if (AreBothNotNull(TransportationSectionMethodBridges, other.TransportationSectionMethodBridges) && !TransportationSectionMethodBridges.OrderBy(x => x.Id).SequenceEqual(other.TransportationSectionMethodBridges.OrderBy(x => x.Id)))
                return false;

            return true;
        }

        #endregion IEquatable<T>
    }
}
