using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class LanguageSection
    {
        #region Properties

        public int?      ParticipantId        { get; set; }
        public decimal?  PinNumber            { get; set; }
        public bool?     IsAbleToReadEnglish  { get; set; }
        public bool?     IsAbleToWriteEnglish { get; set; }
        public bool?     IsAbleToSpeakEnglish { get; set; }
        public bool?     IsNeedingInterpreter { get; set; }
        public string    InterpreterDetails   { get; set; }
        public string    Notes                { get; set; }
        public bool      IsDeleted            { get; set; }
        public string    ModifiedBy           { get; set; }
        public DateTime? ModifiedDate         { get; set; }

        #endregion

        #region Navigation Properties

        public virtual Participant                Participant    { get; set; }
        public virtual ICollection<KnownLanguage> KnownLanguages { get; set; } = new List<KnownLanguage>();

        #endregion
    }
}
