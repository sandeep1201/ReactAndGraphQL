using System;
using System.Collections.Generic;
using Dcf.Wwp.DataAccess.Base;

namespace Dcf.Wwp.DataAccess.Models
{
    public class EARequestStatus : BaseEntity
    {
        #region Properties

        public int       RequestId          { get; set; }
        public int       StatusId           { get; set; }
        public DateTime? StatusDeadLineDate { get; set; }
        public string    Notes              { get; set; }
        public bool      IsDeleted          { get; set; }
        public string    ModifiedBy         { get; set; }
        public DateTime  ModifiedDate       { get; set; }

        #endregion

        #region Navigation Properties

        public virtual EARequest                          EaRequest              { get; set; }
        public virtual EAStatus                           EaStatus               { get; set; }
        public virtual ICollection<EARequestStatusReason> EaRequestStatusReasons { get; set; }

        #endregion

        #region Clone

        #endregion
    }
}
