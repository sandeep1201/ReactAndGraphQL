using System;


namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class JobTypeLeavingReasonBridge
    {
        #region Properties

        public int      Id              { get; set; }
        public int      JobTypeId       { get; set; }
        public int      LeavingReasonId { get; set; }
        public bool     IsDeleted       { get; set; }
        public string   ModifiedBy      { get; set; }
        public DateTime ModifiedDate    { get; set; }

        #endregion

        #region Navigation Properties

        public virtual JobType       JobType       { get; set; }
        public virtual LeavingReason LeavingReason { get; set; }

        #endregion
    }
}
