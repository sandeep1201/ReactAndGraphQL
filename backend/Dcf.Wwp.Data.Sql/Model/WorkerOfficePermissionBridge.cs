using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class WorkerOfficePermissionBridge
    {
        #region Properties

        public string    MFUserId     { get; set; }
        public short?    OfficeNumber { get; set; }
        public DateTime? UpdatedDate  { get; set; }
        public int?      WorkerId     { get; set; }
        public int?      OfficeId     { get; set; }
        public bool?     IsDeleted    { get; set; }
        public string    ModifiedBy   { get; set; }
        public DateTime? ModifiedDate { get; set; }

        [Column("Updated_SA_USER_ID")]
        public int? UpdatedSAUserId { get; set; }

        #endregion

        #region Navigation Properties

        public virtual Office Office { get; set; }
        public virtual Worker Worker { get; set; }

        #endregion
    }
}
