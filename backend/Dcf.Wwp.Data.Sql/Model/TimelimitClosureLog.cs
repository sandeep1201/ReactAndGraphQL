using System;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class TimelimitClosureLog : BaseEntity
    {
        #region Properties

        public int       ParticipantId       { get; set; }
        public DateTime  TargetDate          { get; set; }
        public int       MaxedTimelimitTypes { get; set; }
        public DateTime? CreatedDate         { get; set; }
        public string    ModifiedBy          { get; set; }
        public DateTime? ModifiedDate        { get; set; }

        #endregion

        #region Navigation Properties

        public virtual Participant Participant { get; set; }

        #endregion
    }
}
