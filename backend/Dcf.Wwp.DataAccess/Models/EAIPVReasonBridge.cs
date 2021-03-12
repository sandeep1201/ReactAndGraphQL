using System;
using Dcf.Wwp.DataAccess.Base;

namespace Dcf.Wwp.DataAccess.Models
{
    public class EAIPVReasonBridge : BaseEntity
    {
        #region Properties

        public int      IPVId        { get; set; }
        public int      ReasonId     { get; set; }
        public bool     IsDeleted    { get; set; }
        public string   ModifiedBy   { get; set; }
        public DateTime ModifiedDate { get; set; }

        #endregion

        #region Navigation Properties

        public virtual EAIPV       EaIpv  { get; set; }
        public virtual EAIPVReason Reason { get; set; }

        #endregion
    }
}
