using System;

namespace Dcf.Wwp.Model.Interface
{
    public interface IParticipantContactInfo : ICommonDelModel
    {
        #region Properties

        int?      ParticipantId                   { get; set; }
        int?      CountyOfResidenceId             { get; set; }
        string    StreetAddressPlaceId            { get; set; }
        string    ZipCode                         { get; set; }
        int?      CityAddressId                   { get; set; }
        bool?     HomelessIndicator               { get; set; }
        bool?     IsHouseHoldMailingAddressSame   { get; set; }
        int?      AlternateMailingAddressId       { get; set; }
        string    PrimaryPhoneNumber              { get; set; }
        bool?     CanTextPrimaryPhone             { get; set; }
        bool?     CanLeaveVoiceMailPrimaryPhone   { get; set; }
        string    SecondaryPhoneNumber            { get; set; }
        bool?     CanTextSecondaryPhone           { get; set; }
        bool?     CanLeaveVoiceMailSecondaryPhone { get; set; }
        string    EmailAddress                    { get; set; }
        string    Notes                           { get; set; }
        DateTime? CreatedDate                     { get; set; }
        string    AddressLine1                    { get; set; }
        string    AddressLine2                    { get; set; }
        int?      AddressVerificationTypeLookupId { get; set; }

        #endregion

        #region Navigation Properties

        IAlternateMailingAddress       AlternateMailingAddress       { get; set; }
        ICity                          City                          { get; set; }
        ICountyAndTribe                CountyAndTribe                { get; set; }
        IParticipant                   Participant                   { get; set; }
        IAddressVerificationTypeLookup AddressVerificationTypeLookup { get; set; }

        #endregion
    }
}
