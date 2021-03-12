using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class WorkHistorySection
    {
        #region Properties

        public int       ParticipantId            { get; set; }
        public int?      EmploymentStatusTypeId   { get; set; }
        public bool?     HasVolunteered           { get; set; }
        public string    NonFullTimeDetails       { get; set; }
        public string    Notes                    { get; set; }
        public string    PreventionFactors        { get; set; }
        public bool      IsDeleted                { get; set; }
        public string    ModifiedBy               { get; set; }
        public DateTime? ModifiedDate             { get; set; }
        public int?      HasCareerAssessment      { get; set; }
        public string    HasCareerAssessmentNotes { get; set; }

        #endregion

        #region Navigation Properties

        public virtual Participant                                                   Participant                                       { get; set; }
        public virtual EmploymentStatusType                                          EmploymentStatusType                              { get; set; }
        public virtual ICollection<EmploymentInformation>                            EmploymentInformations                            { get; set; }
        public virtual ICollection<WorkHistorySectionEmploymentPreventionTypeBridge> WorkHistorySectionEmploymentPreventionTypeBridges { get; set; } = new List<WorkHistorySectionEmploymentPreventionTypeBridge>();
        public virtual YesNoUnknownLookup                                            YesNoUnknownLookup                                { get; set; }

        #endregion
    }
}
