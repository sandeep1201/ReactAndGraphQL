using System;
using System.Collections.Generic;
using Dcf.Wwp.DataAccess.Base;

namespace Dcf.Wwp.DataAccess.Models
{
    public class OrganizationInformation : BaseEntity
    {
        #region Properties

        public int      EnrolledProgramId { get; set; }
        public int      OrganizationId    { get; set; }
        public bool     IsDeleted         { get; set; }
        public string   ModifiedBy        { get; set; }
        public DateTime ModifiedDate      { get; set; }

        #endregion

        #region Navigation Properties

        public virtual EnrolledProgram                   EnrolledProgram       { get; set; }
        public virtual Organization                      Organization          { get; set; }
        public virtual ICollection<OrganizationLocation> OrganizationLocations { get; set; } = new List<OrganizationLocation>();

        #endregion
    }
}
