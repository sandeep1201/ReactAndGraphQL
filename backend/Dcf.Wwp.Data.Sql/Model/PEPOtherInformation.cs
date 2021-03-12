using System;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class PEPOtherInformation
    {
        #region Properties

        public int?      PEPId                   { get; set; }
        public string    CompletionReasonDetails { get; set; }
        public bool      IsDeleted               { get; set; }
        public string    ModifiedBy              { get; set; }
        public DateTime? ModifiedDate            { get; set; }

        #endregion

        #region Navigation Properties

        public virtual ParticipantEnrolledProgram ParticipantEnrolledProgram { get; set; }

        #endregion
    }
}
