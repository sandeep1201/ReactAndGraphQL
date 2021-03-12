using System;
using Dcf.Wwp.DataAccess.Base;

namespace Dcf.Wwp.DataAccess.Models
{
    public class POPClaimEmploymentBridge : BaseEntity
    {
        #region Properties

        public int       POPClaimId              { get; set; }
        public int       EmploymentInformationId { get; set; }
        public bool      IsPrimary               { get; set; }
        public decimal   HoursWorked             { get; set; }
        public decimal   Earnings                { get; set; }
        public bool      IsDeleted               { get; set; }
        public string    ModifiedBy              { get; set; }
        public DateTime? ModifiedDate            { get; set; }

        #endregion

        #region Nav Properties

        public virtual POPClaim              POPClaim              { get; set; }
        public virtual EmploymentInformation EmploymentInformation { get; set; }

        #endregion
    }
}
