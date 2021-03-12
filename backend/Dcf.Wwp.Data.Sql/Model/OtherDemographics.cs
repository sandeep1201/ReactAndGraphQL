using System;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class OtherDemographic
    {
        #region Properties

        public int?      ParticipantId           { get; set; }
        public int?      HomeLanguageId          { get; set; }
        public bool?     IsInterpreterNeeded     { get; set; }
        public string    InterpreterDetails      { get; set; }
        public bool?     IsRefugee               { get; set; }
        public DateTime? RefugeeEntryDate        { get; set; }
        public bool?     RefugeeEntryDateUnknown { get; set; }
        public int?      CountryOfOriginId       { get; set; }
        public bool?     TribalIndicator         { get; set; }
        public int?      TribalId                { get; set; }
        public string    TribalDetails           { get; set; }
        public bool      IsDeleted               { get; set; }
        public DateTime? CreatedDate             { get; set; }
        public string    ModifiedBy              { get; set; }
        public DateTime? ModifiedDate            { get; set; }

        #endregion

        #region Navigation Properties

        public virtual Participant    Participant    { get; set; }
        public virtual Language       Language       { get; set; }
        public virtual CountyAndTribe CountyAndTribe { get; set; }
        public virtual Country        Country        { get; set; }

        #endregion
    }
}
