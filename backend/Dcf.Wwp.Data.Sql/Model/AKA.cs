using System;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class AKA
    {
        #region Properties

        public int?      ParticipantId { get; set; }
        public decimal?  SSNNumber     { get; set; }
        public int?      SSNTypeId     { get; set; }
        public string    Details       { get; set; }
        public bool      IsDeleted     { get; set; }
        public DateTime? CreatedDate   { get; set; }
        public string    ModifiedBy    { get; set; }
        public DateTime? ModifiedDate  { get; set; }

        #endregion

        #region Navigation Properties

        public virtual Participant Participant { get; set; }
        public virtual SSNType     SSNType     { get; set; } //TODO: not used

        #endregion
    }
}
