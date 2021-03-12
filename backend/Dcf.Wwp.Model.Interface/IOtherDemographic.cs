using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dcf.Wwp.Model.Interface
{
    public interface IOtherDemographic : ICommonDelModel
    {
        int?            ParticipantId           { get; set; }
        int?            HomeLanguageId          { get; set; }
        bool?           IsInterpreterNeeded     { get; set; }
        string          InterpreterDetails      { get; set; }
        bool?           IsRefugee               { get; set; }
        DateTime?       RefugeeEntryDate        { get; set; }
        bool?           RefugeeEntryDateUnknown { get; set; }
        int?            CountryOfOriginId       { get; set; }
        bool?           TribalIndicator         { get; set; }
        int?            TribalId                { get; set; }
        string          TribalDetails           { get; set; }
        DateTime?       CreatedDate             { get; set; }

        ILanguage       Language                { get; set; }
        IParticipant    Participant             { get; set; }
        ICountyAndTribe CountyAndTribe          { get; set; }
        ICountry        Country                 { get; set; }
    }
}
