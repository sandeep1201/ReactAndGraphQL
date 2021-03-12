using System;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class ConfidentialPinInformation
    {
        #region Properties

        public int?     ParticipantId  { get; set; }
        public decimal? PinNumber      { get; set; }
        public bool?    IsConfidential { get; set; }
        public int?     WorkerId       { get; set; }
        public bool     IsDeleted      { get; set; }
        public string   ModifiedBy     { get; set; }
        public DateTime ModifiedDate   { get; set; }

        #endregion

        #region Navigation Properties

        public virtual Participant Participant { get; set; }
        public virtual Worker      Worker      { get; set; }

        #endregion
    }
}
