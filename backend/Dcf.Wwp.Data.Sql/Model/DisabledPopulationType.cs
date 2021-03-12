using System;
using Newtonsoft.Json;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class DisabledPopulationType
    {
        #region Properties

        public int       EnrolledProgramOrganizationPopulationTypeBridgeId { get; set; }
        public int       PopulationTypeId                                  { get; set; }
        public bool      IsDeleted                                         { get; set; }
        public string    ModifiedBy                                        { get; set; }
        public DateTime? ModifiedDate                                      { get; set; }

        #endregion

        #region Navigation Properties

        [JsonIgnore]
        public virtual EnrolledProgramOrganizationPopulationTypeBridge EnrolledProgramOrganizationPopulationTypeBridge { get; set; }

        [JsonIgnore]
        public virtual PopulationType PopulationType { get; set; }

        #endregion
    }
}
