using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class EmployabilityPlan
    {
        #region Properties

        public int       ParticipantId                 { get; set; }
        public int       EnrolledProgramId             { get; set; }
        public DateTime  BeginDate                     { get; set; }
        public DateTime  EndDate                       { get; set; }
        public bool      IsDeleted                     { get; set; }
        public string    ModifiedBy                    { get; set; }
        public DateTime  ModifiedDate                  { get; set; }
        public DateTime? CreatedDate                   { get; set; }
        public string    Notes                         { get; set; }
        public int?      EmployabilityPlanStatusTypeId { get; set; }
        public int       ParticipantEnrolledProgramId  { get; set; }
        public bool?     CanSaveWithoutActivity        { get; set; }
        public string    CanSaveWithoutActivityDetails { get; set; }
        public int?      OrganizationId                { get; set; }
        public DateTime? SubmitDate                    { get; set; }

        #endregion

        #region Navigation Properties

        public virtual EnrolledProgram                              EnrolledProgram                  { get; set; }
        public virtual Participant                                  Participant                      { get; set; }
        public virtual ICollection<SupportiveService>               SupportiveServices               { get; set; }
        public virtual ICollection<ActivitySchedule>                ActivitySchedules                { get; set; }
        public virtual ICollection<EmployabilityPlanGoalBridge>     EmployabilityPlanGoalBridges     { get; set; }
        public virtual ICollection<EmployabilityPlanActivityBridge> EmployabilityPlanActivityBridges { get; set; }
        public virtual ParticipantEnrolledProgram                   ParticipantEnrolledProgram       { get; set; }
        public virtual EmployabilityPlanStatusType                  EmployabilityPlanStatusType      { get; set; }
        public virtual Organization                                 Organization                     { get; set; }
        public virtual ICollection<EPEIBridge>                      EPEIBridges                      { get; set; }

        #endregion
    }
}
