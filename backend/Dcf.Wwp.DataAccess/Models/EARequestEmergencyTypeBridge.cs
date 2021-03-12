using System;
using Dcf.Wwp.DataAccess.Base;

namespace Dcf.Wwp.DataAccess.Models
{
    public class EARequestEmergencyTypeBridge : BaseEntity
    {
        #region Properties

        public int      RequestId             { get; set; }
        public int      EmergencyTypeId       { get; set; }
        public int?     EmergencyTypeReasonId { get; set; }
        public bool     IsDeleted             { get; set; }
        public string   ModifiedBy            { get; set; }
        public DateTime ModifiedDate          { get; set; }

        #endregion

        #region Nav Properties

        public virtual EARequest             EaRequest             { get; set; }
        public virtual EAEmergencyType       EaEmergencyType       { get; set; }
        public virtual EAEmergencyTypeReason EaEmergencyTypeReason { get; set; }

        #endregion
    }
}
