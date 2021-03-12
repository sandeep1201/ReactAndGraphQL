using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class Organization
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

        public virtual ICollection<Worker>                                          Workers                                          { get; set; }
        public virtual ICollection<ContractArea>                                    ContractAreas                                    { get; set; }
        public virtual ICollection<EnrolledProgramOrganizationPopulationTypeBridge> EnrolledProgramOrganizationPopulationTypeBridges { get; set; }

        #endregion
    }
}
