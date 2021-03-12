using System;
using Dcf.Wwp.DataAccess.Base;

namespace Dcf.Wwp.DataAccess.Models
{
    public class EAVerificationTypeBridge : BaseEntity
    {
        #region Properties

        public int      VerificationTypeId { get; set; }
        public bool     IsIncome           { get; set; }
        public bool     IsAsset            { get; set; }
        public bool     IsVehicle          { get; set; }
        public bool     IsVehicleValue     { get; set; }
        public bool     IsDeleted          { get; set; }
        public string   ModifiedBy         { get; set; }
        public DateTime ModifiedDate       { get; set; }

        #endregion

        #region Navigation Properties

        public virtual EAVerificationType EaVerificationType { get; set; }

        #endregion

        #region Clone

        #endregion
    }
}
