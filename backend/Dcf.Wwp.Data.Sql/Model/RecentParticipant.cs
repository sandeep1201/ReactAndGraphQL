using System;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class RecentParticipant
    {
        #region Properties

        public int       Id            { get; set; }
        public int       WorkerId      { get; set; }
        public int       ParticipantId { get; set; }
        public DateTime  LastAccessed  { get; set; }
        public bool      IsDeleted     { get; set; }
        public string    ModifiedBy    { get; set; }
        public DateTime? ModifiedDate  { get; set; }
        public byte[]    RowVersion    { get; set; }

        #endregion

        #region Navigation Properties

        public virtual Worker      Worker      { get; set; }
        public virtual Participant Participant { get; set; }

        #endregion
    }
}
