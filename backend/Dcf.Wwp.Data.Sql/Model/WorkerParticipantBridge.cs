namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class WorkerParticipantBridge
    {
        #region Properties

        public int  Id            { get; set; }
        public int? WorkerId      { get; set; }
        public int? ParticipantId { get; set; }

        #endregion

        #region Navigation Properties

        public virtual Participant Participant { get; set; }
        public virtual Worker      Worker      { get; set; }

        #endregion
    }
}
