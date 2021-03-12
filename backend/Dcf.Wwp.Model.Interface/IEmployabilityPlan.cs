using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dcf.Wwp.Model.Interface
{
    public interface IEmployabilityPlan : ICommonModelFinal, ICloneable
    {
        int       Id                            { get; set; }
        int       ParticipantId                 { get; set; }
        int       EnrolledProgramId             { get; set; }
        DateTime  BeginDate                     { get; set; }
        DateTime  EndDate                       { get; set; }
        DateTime? CreatedDate                   { get; set; }
        int       ParticipantEnrolledProgramId  { get; set; }
        bool?     CanSaveWithoutActivity        { get; set; }
        string    CanSaveWithoutActivityDetails { get; set; }
        int?      EmployabilityPlanStatusTypeId { get; set; }
        int?      OrganizationId                { get; set; }
        DateTime? SubmitDate                    { get; set; }


        string Notes { get; set; }

        //ICollection<IActivity>          Activities         { get; set; }
        IEnrolledProgram EnrolledProgram { get; set; }

        IParticipant Participant { get; set; }

        //ICollection<IGoal>              Goals              { get; set; }
        ICollection<ISupportiveService>               SupportiveServices               { get; set; }
        ICollection<IEmployabilityPlanActivityBridge> EmployabilityPlanActivityBridges { get; set; }
        ICollection<IEmployabilityPlanGoalBridge>     EmployabilityPlanGoalBridges     { get; set; }
        IParticipantEnrolledProgram                   ParticipantEnrolledProgram       { get; set; }
        IEmployabilityPlanStatusType                  EmployabilityPlanStatusType      { get; set; }
        IOrganization                                 Organization                     { get; set; }
        ICollection<IEPEIBridge>                      EPEIBridges                      { get; set; }
    }
}
