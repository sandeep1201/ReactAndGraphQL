using System;
using Dcf.Wwp.DataAccess.Base;

namespace Dcf.Wwp.DataAccess.Models
{
    public class EAPaymentAmount : BaseEntity
    {
        #region Properties

        public int      EmergencyTypeId  { get; set; }
        public int      MinGroupSize     { get; set; }
        public int      MaxGroupSize     { get; set; }
        public decimal  AmountPerMember  { get; set; }
        public decimal  MaxPaymentAmount { get; set; }
        public bool     IsDeleted        { get; set; }
        public string   ModifiedBy       { get; set; }
        public DateTime ModifiedDate     { get; set; }

        #endregion

        #region Navigation Properties

        public virtual EAEmergencyType EaEmergencyType { get; set; }

        #endregion

        #region Clone

        #endregion
    }
}
