using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class PopulationType
    {
        #region Properties

        public string    Name         { get; set; }
        public int       SortOrder    { get; set; }
        public bool      IsDeleted    { get; set; }
        public string    ModifiedBy   { get; set; }
        public DateTime? ModifiedDate { get; set; }

        #endregion

        #region Navigation Properties

        public virtual ICollection<DisabledPopulationType>                          DisabledPopulationTypes                          { get; set; }
        public virtual ICollection<EnrolledProgramOrganizationPopulationTypeBridge> EnrolledProgramOrganizationPopulationTypeBridges { get; set; }
        public virtual ICollection<RequestForAssistancePopulationTypeBridge>        RequestForAssistancePopulationTypeBridges        { get; set; }

        #endregion
    }
}
