using System;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class Transaction
    {
        #region Properties

        public int      ParticipantId     { get; set; }
        public int?     WorkerId          { get; set; }
        public int      OfficeId          { get; set; }
        public int      TransactionTypeId { get; set; }
        public string   Description       { get; set; }
        public DateTime EffectiveDate     { get; set; }
        public DateTime CreatedDate       { get; set; }
        public bool     IsDeleted         { get; set; }
        public string   ModifiedBy        { get; set; }
        public DateTime ModifiedDate      { get; set; }

        #endregion

        #region Nav Properties

        public virtual Participant Participant { get; set; }
        public virtual Worker      Worker      { get; set; }
        public virtual Office      Office      { get; set; }

        #endregion
    }
}
