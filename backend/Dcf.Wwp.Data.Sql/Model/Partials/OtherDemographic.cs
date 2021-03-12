using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class OtherDemographic : BaseEntity, IOtherDemographic
    {
        ILanguage IOtherDemographic.Language
        {
            get => Language;
            set => Language = (Language) value;
        }

        IParticipant IOtherDemographic.Participant
        {
            get => Participant;
            set => Participant = (Participant) value;
        }

        ICountyAndTribe IOtherDemographic.CountyAndTribe
        {
            get => CountyAndTribe;
            set => CountyAndTribe = (CountyAndTribe) value;
        }

        ICountry IOtherDemographic.Country
        {
            get => Country;
            set => Country = (Country) value;
        }
    }
}
