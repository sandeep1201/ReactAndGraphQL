using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class AlternateMailingAddress : BaseEntity, IAlternateMailingAddress
    {
        ICollection<IParticipantContactInfo> IAlternateMailingAddress.ParticipantContactInfoes
        {
            get => ParticipantContactInfoes.Cast<IParticipantContactInfo>().ToList();
            set => ParticipantContactInfoes = value.Cast<ParticipantContactInfo>().ToList();
        }

        ICity IAlternateMailingAddress.City
        {
            get => City;
            set => City = (City) value;
        }

        IState IAlternateMailingAddress.State
        {
            get => State;
            set => State = (State) value;
        }

        IAddressVerificationTypeLookup IAlternateMailingAddress.AddressVerificationTypeLookup
        {
            get => AddressVerificationTypeLookup;
            set => AddressVerificationTypeLookup = (AddressVerificationTypeLookup)value;
        }
    }
}
