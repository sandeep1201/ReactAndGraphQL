using System;
using Dcf.Wwp.DataAccess.Base;

namespace Dcf.Wwp.DataAccess.Models
{
    public class EAAssets : BaseEntity
    {
        #region Properties

        public int      RequestId          { get; set; }
        public string   AssetType          { get; set; }
        public decimal? CurrentValue       { get; set; }
        public int?     VerificationTypeId { get; set; }
        public int?     AssetOwner         { get; set; }
        public bool     IsDeleted          { get; set; }
        public string   ModifiedBy         { get; set; }
        public DateTime ModifiedDate       { get; set; }

        #endregion

        #region Navigation Properties

        public virtual EARequest          EaRequest          { get; set; }
        public virtual EAVerificationType EaVerificationType { get; set; }
        public virtual Participant        Participant        { get; set; }

        #endregion
    }
}
