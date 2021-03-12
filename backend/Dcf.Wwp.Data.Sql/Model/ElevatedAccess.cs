using System;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class ElevatedAccess
    {
        #region Properties

        public int       WorkerId               { get; set; }
        public int       ParticipantId          { get; set; }
        public DateTime  AccessCreateDate       { get; set; }
        public int?      ElevatedAccessReasonId { get; set; }
        public string    Details                { get; set; }
        public bool      IsDeleted              { get; set; }
        public string    ModifiedBy             { get; set; }
        public DateTime? ModifiedDate           { get; set; }

        #endregion

        #region Navigation properties

        public virtual Participant          Participant          { get; set; }
        public virtual Worker               Worker               { get; set; }
        public virtual ElevatedAccessReason ElevatedAccessReason { get; set; }

        #endregion
    }
}
