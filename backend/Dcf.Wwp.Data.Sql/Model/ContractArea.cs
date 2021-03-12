using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class ContractArea
    {
        #region Properties

        public string    ContractAreaName  { get; set; }
        public int?      OrganizationId    { get; set; }
        public int?      EnrolledProgramId { get; set; }
        public DateTime? ActivatedDate     { get; set; }
        public DateTime? InActivatedDate   { get; set; }
        public bool      IsDeleted         { get; set; }
        public string    ModifiedBy        { get; set; }
        public DateTime? ModifiedDate      { get; set; }

        #endregion

        #region Navigation Properties

        [JsonIgnore]
        public virtual Organization Organization { get; set; }

        [JsonIgnore]
        public virtual EnrolledProgram EnrolledProgram { get; set; }

        [JsonIgnore]
        public virtual ICollection<Office> Offices { get; set; }

        public virtual ICollection<AssociatedOrganization> AssociatedOrganizations { get; set; }

        #endregion
    }
}
