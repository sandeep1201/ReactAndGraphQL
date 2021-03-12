using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class EnrolledProgramOrganizationPopulationTypeBridge
    {
        #region Properties

        public int       EnrolledProgramId { get; set; }
        public int?      OrganizationId    { get; set; }
        public int       PopulationTypeId  { get; set; }
        public bool      IsDeleted         { get; set; }
        public string    ModifiedBy        { get; set; }
        public DateTime? ModifiedDate      { get; set; }

        #endregion

        #region Navigation Properties

        public virtual EnrolledProgram                     EnrolledProgram         { get; set; }
        public virtual Organization                        Organization            { get; set; }
        public virtual PopulationType                      PopulationType          { get; set; }
        public virtual ICollection<DisabledPopulationType> DisabledPopulationTypes { get; set; }

        #endregion
    }
}
