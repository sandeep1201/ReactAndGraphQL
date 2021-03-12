using System;
using System.Collections.Generic;
using Dcf.Wwp.DataAccess.Base;

namespace Dcf.Wwp.DataAccess.Models
{
    public class Organization : BaseEntity
    {
        #region Properties

        public string    EntsecAgencyCode { get; set; }
        public string    AgencyName       { get; set; }
        public string    DB2AgencyName    { get; set; }
        public DateTime? ActivatedDate    { get; set; }
        public DateTime? InActivatedDate  { get; set; }
        public bool      IsDeleted        { get; set; }
        public string    ModifiedBy       { get; set; }
        public DateTime? ModifiedDate     { get; set; }

        #endregion

        #region Navigation Properties

        public virtual ICollection<Worker>           Workers           { get; set; } = new List<Worker>();
        public virtual ICollection<ContractArea>     ContractAreas     { get; set; } = new List<ContractArea>();
        public virtual ICollection<POPClaim>         POPClaims         { get; set; } = new List<POPClaim>();
        public virtual ICollection<POPClaimHighWage> POPClaimHighWages { get; set; } = new List<POPClaimHighWage>();

        #endregion

        #region Methods

        public override string ToString() => $"Id: {Id} / {AgencyName} / {EntsecAgencyCode} / {DB2AgencyName}";

        #endregion
    }
}
