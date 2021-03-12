using System;
using System.Collections.Generic;
using Dcf.Wwp.DataAccess.Base;

namespace Dcf.Wwp.DataAccess.Models
{
    public class EmployabilityPlan : BaseEntity
    {
        #region Properties

        public int       ParticipantId                 { get; set; }
        public int       EnrolledProgramId             { get; set; }
        public DateTime  BeginDate                     { get; set; }
        public DateTime  EndDate                       { get; set; }
        public bool      IsDeleted                     { get; set; }
        public DateTime? CreatedDate                   { get; set; }
        public int?      EmployabilityPlanStatusTypeId { get; set; }
        public string    ModifiedBy                    { get; set; }
        public DateTime  ModifiedDate                  { get; set; }
        public string    Notes                         { get; set; }
        public int       ParticipantEnrolledProgramId  { get; set; }
        public bool?     CanSaveWithoutActivity        { get; set; }
        public string    CanSaveWithoutActivityDetails { get; set; }
        public int?      OrganizationId                { get; set; }
        public DateTime? SubmitDate                    { get; set; }
        public DateTime? DateSigned                    { get; set; }

        #endregion

        #region Navigation Properties

        public virtual Participant                                        Participant                           { get; set; }
        public virtual ParticipantEnrolledProgram                         ParticipantEnrolledProgram            { get; set; }
        public virtual Organization                                       Organization                          { get; set; }
        public virtual EnrolledProgram                                    EnrolledProgram                       { get; set; }
        public virtual ICollection<SupportiveService>                     SupportiveServices                    { get; set; } = new List<SupportiveService>();
        public virtual ICollection<EmployabilityPlanActivityBridge>       EmploybilityPlanActivityBridges       { get; set; } = new List<EmployabilityPlanActivityBridge>();
        public virtual ICollection<EmployabilityPlanGoalBridge>           EmployabilityPlanGoalBridges          { get; set; } = new List<EmployabilityPlanGoalBridge>();
        public virtual EmployabilityPlanStatusType                        EmployabilityPlanStatusType           { get; set; }
        public virtual ICollection<EmployabilityPlanEmploymentInfoBridge> EmploybilityPlanEmploymentInfoBridges { get; set; } = new List<EmployabilityPlanEmploymentInfoBridge>();
        public virtual ICollection<ParticipationEntry>                    ParticipationEntries                  { get; set; } = new List<ParticipationEntry>();
        public virtual ICollection<ParticipationEntryHistory>             ParticipationEntryHistories           { get; set; }
        public virtual ICollection<CFParticipationEntry>                  CFParticipationEntries                { get; set; } = new List<CFParticipationEntry>();

        #endregion
    }
}
