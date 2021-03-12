using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class EnrolledProgram : BaseCommonModel, IEnrolledProgram
    {
        ICollection<IEnrolledProgramOrganizationPopulationTypeBridge> IEnrolledProgram.EnrolledProgramOrganizationPopulationTypeBridges
        {
            get { return EnrolledProgramOrganizationPopulationTypeBridges.Cast<IEnrolledProgramOrganizationPopulationTypeBridge>().ToList(); }

            set { EnrolledProgramOrganizationPopulationTypeBridges = value.Cast<EnrolledProgramOrganizationPopulationTypeBridge>().ToList(); }
        }

        ICollection<IParticipantEnrolledProgram> IEnrolledProgram.ParticipantEnrolledPrograms
        {
            get { return ParticipantEnrolledPrograms.Cast<IParticipantEnrolledProgram>().ToList(); }

            set { ParticipantEnrolledPrograms = value.Cast<ParticipantEnrolledProgram>().ToList(); }
        }

        ICollection<IContractArea> IEnrolledProgram.ContractAreas
        {
            get { return ContractAreas.Cast<IContractArea>().ToList(); }

            set { ContractAreas = value.Cast<ContractArea>().ToList(); }
        }

        ICollection<IEmployabilityPlan> IEnrolledProgram.EmployabilityPlans
        {
            get { return EmployabilityPlans.Cast<IEmployabilityPlan>().ToList(); }

            set { EmployabilityPlans = value.Cast<EmployabilityPlan>().ToList(); }
        }

        ICollection<IGoalType> IEnrolledProgram.GoalTypes
        {
            get { return GoalTypes.Cast<IGoalType>().ToList(); }

            set { GoalTypes = value.Cast<GoalType>().ToList(); }
        }

        ICollection<IParticipationStatu> IEnrolledProgram.ParticipationStatus
        {
            get { return ParticipationStatus.Cast<IParticipationStatu>().ToList(); }

            set { ParticipationStatus = value.Cast<ParticipationStatu>().ToList(); }
        }
    }
}
