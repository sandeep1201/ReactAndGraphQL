using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class EnrolledProgram
    {
        #region Properties

        public string    ProgramCode     { get; set; }
        public string    SubProgramCode  { get; set; }
        public string    ProgramType     { get; set; }
        public string    DescriptionText { get; set; }
        public string    Name            { get; set; }
        public string    ShortName       { get; set; }
        public int       SortOrder       { get; set; }
        public bool      IsDeleted       { get; set; }
        public string    ModifiedBy      { get; set; }
        public DateTime? ModifiedDate    { get; set; }

        #endregion

        #region Navigation Properties

        public virtual ICollection<ParticipantEnrolledProgram> ParticipantEnrolledPrograms { get; set; }
        public virtual ICollection<EmployabilityPlan>          EmployabilityPlans          { get; set; }
        public virtual ICollection<ParticipationStatu>         ParticipationStatus         { get; set; }

        [JsonIgnore]
        public virtual ICollection<ContractArea> ContractAreas { get; set; }

        [JsonIgnore]
        public virtual ICollection<CompletionReason> CompletionReasons { get; set; }

        [JsonIgnore]
        public virtual ICollection<EnrolledProgramOrganizationPopulationTypeBridge> EnrolledProgramOrganizationPopulationTypeBridges { get; set; }

        [JsonIgnore]
        public virtual ICollection<GoalType> GoalTypes { get; set; }

        [JsonIgnore]
        public virtual ICollection<RequestForAssistance> RequestForAssistances { get; set; }

        [JsonIgnore]
        public virtual ICollection<EnrolledProgramEPActivityTypeBridge> EnrolledProgramEPActivityTypeBridges { get; set; }

        #endregion
    }
}
