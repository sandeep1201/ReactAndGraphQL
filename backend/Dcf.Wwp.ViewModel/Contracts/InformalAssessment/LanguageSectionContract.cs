using System.Collections.Generic;


namespace Dcf.Wwp.Api.Library.Contracts.InformalAssessment
{

    public class LanguageSectionContract : BaseInformalAssessmentContract
    {
        public int HomeLanguageId { get; set; }
        public int? HomeLanguageTypeId { get; set; }
        public string HomeLanguageName { get; set; }
        public bool? IsAbleToReadHomeLanguage { get; set; }
        public bool? IsAbleToWriteHomeLanguage { get; set; }
        public bool? IsAbleToSpeakHomeLanguage { get; set; }

        public bool? IsAbleToReadEnglish { get; set; }
        public bool? IsAbleToWriteEnglish { get; set; }
        public bool? IsAbleToSpeakEnglish { get; set; }

        public List<KnownLanguageContract> KnownLanguages { get; set; }

        public bool? IsNeedingInterpreter { get; set; }
        public string InterpreterDetails { get; set; }
        public string Notes { get; set; }

        public LanguageSectionContract()
        {
            // Initialize the contract with an empty list of KnownLanguages
            // to make it easier for consumers of the API to handle an
            // initial state.
            KnownLanguages = new List<KnownLanguageContract>();
        }
    }
}
