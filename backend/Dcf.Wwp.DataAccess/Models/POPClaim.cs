using System;
using System.Collections.Generic;
using Dcf.Wwp.DataAccess.Base;

namespace Dcf.Wwp.DataAccess.Models
{
    public class POPClaim : BaseEntity
    {
        #region Properties

        public int       ParticipantId        { get; set; }
        public int       OrganizationId       { get; set; }
        public int       POPClaimTypeId       { get; set; }
        public DateTime? ClaimPeriodBeginDate { get; set; }
        public bool      IsDeleted            { get; set; }
        public string    ModifiedBy           { get; set; }
        public DateTime  ModifiedDate         { get; set; }
        public DateTime? ClaimEffectiveDate   { get; set; }
        public bool?     IsProcessed          { get; set; }

        #endregion

        #region Navigation Properties

        public virtual Participant                           Participant               { get; set; }
        public virtual Organization                          Organization              { get; set; }
        public virtual POPClaimType                          POPClaimType              { get; set; }
        public virtual ICollection<POPClaimEmploymentBridge> POPClaimEmploymentBridges { get; set; } = new List<POPClaimEmploymentBridge>();
        public virtual ICollection<POPClaimStatus>           POPClaimStatuses          { get; set; } = new List<POPClaimStatus>();
        public virtual ICollection<POPClaimActivityBridge>   POPClaimActivityBridges   { get; set; } = new List<POPClaimActivityBridge>();

        #endregion

        #region Clone

        #endregion
    }
}
