using System;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class ParticipantContactInfo
    {
        #region Properties

        public int?      ParticipantId                   { get; set; }
        public int?      CountyOfResidenceId             { get; set; }
        public string    ZipCode                         { get; set; }
        public int?      CityAddressId                   { get; set; }
        public bool?     HomelessIndicator               { get; set; }
        public bool?     IsHouseHoldMailingAddressSame   { get; set; }
        public int?      AlternateMailingAddressId       { get; set; }
        public string    PrimaryPhoneNumber              { get; set; }
        public bool?     CanTextPrimaryPhone             { get; set; }
        public bool?     CanLeaveVoiceMailPrimaryPhone   { get; set; }
        public string    SecondaryPhoneNumber            { get; set; }
        public bool?     CanTextSecondaryPhone           { get; set; }
        public bool?     CanLeaveVoiceMailSecondaryPhone { get; set; }
        public string    EmailAddress                    { get; set; }
        public string    Notes                           { get; set; }
        public bool      IsDeleted                       { get; set; }
        public DateTime? CreatedDate                     { get; set; }
        public string    ModifiedBy                      { get; set; }
        public DateTime? ModifiedDate                    { get; set; }
        public string    StreetAddressPlaceId            { get; set; }
        public string    AddressLine1                    { get; set; }
        public string    AddressLine2                    { get; set; }
        public int?      AddressVerificationTypeLookupId { get; set; }

        #endregion

        #region Navigation Properties

        public virtual City                          City                          { get; set; }
        public virtual CountyAndTribe                CountyAndTribe                { get; set; }
        public virtual Participant                   Participant                   { get; set; }
        public virtual AlternateMailingAddress       AlternateMailingAddress       { get; set; }
        public virtual AddressVerificationTypeLookup AddressVerificationTypeLookup { get; set; }

        #endregion
    }
}
