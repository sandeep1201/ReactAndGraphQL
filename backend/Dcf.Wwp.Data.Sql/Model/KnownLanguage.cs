using System;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class KnownLanguage
    {
        #region Properties

        public decimal?  PinNumber         { get; set; }
        public int       LanguageSectionId { get; set; }
        public int       LanguageId        { get; set; }
        public bool?     IsPrimary         { get; set; }
        public bool?     IsAbleToRead      { get; set; }
        public bool?     IsAbleToWrite     { get; set; }
        public bool?     IsAbleToSpeak     { get; set; }
        public int?      SortOrder         { get; set; }
        public bool      IsDeleted         { get; set; }
        public string    ModifiedBy        { get; set; }
        public DateTime? ModifiedDate      { get; set; }

        #endregion

        #region Navigation Properties

        public virtual Language        Language        { get; set; }
        public virtual LanguageSection LanguageSection { get; set; }

        #endregion
    }
}
