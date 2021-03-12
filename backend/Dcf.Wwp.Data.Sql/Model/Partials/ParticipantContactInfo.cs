using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class ParticipantContactInfo : BaseEntity, IParticipantContactInfo
    {
        #region Properties

        ICountyAndTribe IParticipantContactInfo.CountyAndTribe
        {
            get => CountyAndTribe;
            set => CountyAndTribe = (CountyAndTribe) value;
        }

        IParticipant IParticipantContactInfo.Participant
        {
            get => Participant;
            set => Participant = (Participant) value;
        }

        IAlternateMailingAddress IParticipantContactInfo.AlternateMailingAddress
        {
            get => AlternateMailingAddress;
            set => AlternateMailingAddress = (AlternateMailingAddress) value;
        }

        ICity IParticipantContactInfo.City
        {
            get => City;
            set => City = (City) value;
        }

        IAddressVerificationTypeLookup IParticipantContactInfo.AddressVerificationTypeLookup
        {
            get => AddressVerificationTypeLookup;
            set => AddressVerificationTypeLookup = (AddressVerificationTypeLookup)value;
        }

        #endregion

        #region Methods

        #endregion
    }
}
