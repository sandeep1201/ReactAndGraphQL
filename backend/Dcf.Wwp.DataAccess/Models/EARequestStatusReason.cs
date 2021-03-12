using System;
using Dcf.Wwp.DataAccess.Base;

namespace Dcf.Wwp.DataAccess.Models
{
    public class EARequestStatusReason : BaseEntity
    {
        #region Properties

        public int       RequestStatusId { get; set; }
        public int       StatusReasonId  { get; set; }
        public bool      IsDeleted       { get; set; }
        public string    ModifiedBy      { get; set; }
        public DateTime? ModifiedDate    { get; set; }

        #endregion

        #region Nav Properties

        public virtual EARequestStatus EaRequestStatus { get; set; }
        public virtual EAStatusReason  StatusReason    { get; set; }

        #endregion
    }
}
