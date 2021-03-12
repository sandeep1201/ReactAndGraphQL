using System;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class TimeLimitSummary
    {
        #region Properties

        public int?      ParticipantId     { get; set; }
        public int?      FederalUsed       { get; set; }
        public int?      FederalMax        { get; set; }
        public int?      StateUsed         { get; set; }
        public int?      StateMax          { get; set; }
        public int?      CSJUsed           { get; set; }
        public int?      CSJMax            { get; set; }
        public int?      W2TUsed           { get; set; }
        public int?      W2TMax            { get; set; }
        public int?      TMPUsed           { get; set; }
        public int?      TNPUsed           { get; set; }
        public int?      TempUsed          { get; set; }
        public int?      TempMax           { get; set; }
        public int?      CMCUsed           { get; set; }
        public int?      CMCMax            { get; set; }
        public int?      OPCUsed           { get; set; }
        public int?      OPCMax            { get; set; }
        public int?      OtherUsed         { get; set; }
        public int?      OtherMax          { get; set; }
        public int?      OTF               { get; set; }
        public int?      Tribal            { get; set; }
        public int?      TJB               { get; set; }
        public int?      JOBS              { get; set; }
        public int?      NO24              { get; set; }
        public string    FactDetails       { get; set; }
        public bool?     CSJExtensionDue   { get; set; }
        public bool?     W2TExtensionDue   { get; set; }
        public bool?     TempExtensionDue  { get; set; }
        public bool?     StateExtensionDue { get; set; }
        public DateTime? CreatedDate       { get; set; }
        public bool      IsDeleted         { get; set; }
        public string    ModifiedBy        { get; set; }
        public DateTime? ModifiedDate      { get; set; }

        #endregion

        #region Navigation Properties

        public virtual Participant Participant { get; set; }

        #endregion
    }
}
