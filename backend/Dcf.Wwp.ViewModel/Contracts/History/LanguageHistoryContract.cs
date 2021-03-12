using System.Collections.Generic;


namespace Dcf.Wwp.Api.Library.Contracts.History
{
    public class LanguageHistoryContract
    {
        public List<HistoryValueContract> HomeLanguageTypeId { get; set; }
        public List<HistoryValueContract> IsAbleToReadHomeLanguage { get; set; }
        public List<HistoryValueContract> IsAbleToWriteHomeLanguage { get; set; }
        public List<HistoryValueContract> IsAbleToSpeakHomeLanguage { get; set; }
        public List<KnownLanguageHistoryContract> KnownLanguages { get; set; }
        public List<HistoryValueContract> Notes { get; set; }
        public List<HistoryValueContract> IsAbleToReadEnglish { get; set; }
        public List<HistoryValueContract> IsAbleToWriteEnglish { get; set; }
        public List<HistoryValueContract> IsAbleToSpeakEnglish { get; set; }
        public List<HistoryValueContract> NeedsInterpreter { get; set; }
        public List<HistoryValueContract> InterpreterDetails { get; set; }

        public LanguageHistoryContract()
        {
            HomeLanguageTypeId = new List<HistoryValueContract>();
            IsAbleToReadHomeLanguage = new List<HistoryValueContract>();
            IsAbleToWriteHomeLanguage = new List<HistoryValueContract>();
            IsAbleToSpeakHomeLanguage = new List<HistoryValueContract>();
            KnownLanguages = new List<KnownLanguageHistoryContract>();
            Notes = new List<HistoryValueContract>();
            IsAbleToReadEnglish = new List<HistoryValueContract>();
            IsAbleToWriteEnglish = new List<HistoryValueContract>();
            IsAbleToSpeakEnglish = new List<HistoryValueContract>();
            NeedsInterpreter = new List<HistoryValueContract>();
            InterpreterDetails = new List<HistoryValueContract>();
        }
    }


    public class KnownLanguageHistoryContract
    {
        public List<HistoryValueContract> LanguageTypeId { get; set; }
        public List<HistoryValueContract> IsAbleToRead { get; set; }
        public List<HistoryValueContract> IsAbleToWrite { get; set; }
        public List<HistoryValueContract> IsAbleToSpeak { get; set; }

        public KnownLanguageHistoryContract()
        {
            LanguageTypeId = new List<HistoryValueContract>();
            IsAbleToRead = new List<HistoryValueContract>();
            IsAbleToWrite = new List<HistoryValueContract>();
            IsAbleToSpeak = new List<HistoryValueContract>();
        }
    }
}
