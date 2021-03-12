using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Model.Interface
{
    public interface IEnrolledProgram //: ICommonModel// -- does not implement RowVersion
    {
        #region Properties

        int       Id              { get; set; }
        string    ProgramCode     { get; set; }
        string    SubProgramCode  { get; set; }
        string    Name            { get; set; }
        string    ShortName       { get; set; }
        string    ProgramType     { get; set; }
        string    DescriptionText { get; set; }
        bool      IsDeleted       { get; set; }
        DateTime? ModifiedDate    { get; set; }
        string    ModifiedBy      { get; set; }

        #endregion

        #region Navigation Props

        ICollection<IParticipantEnrolledProgram>                      ParticipantEnrolledPrograms                      { get; set; }
        ICollection<IEnrolledProgramOrganizationPopulationTypeBridge> EnrolledProgramOrganizationPopulationTypeBridges { get; set; }
        ICollection<IContractArea>                                    ContractAreas                                    { get; set; }
        ICollection<IEmployabilityPlan>                               EmployabilityPlans                               { get; set; }
        ICollection<IGoalType>                                        GoalTypes                                        { get; set; }
        ICollection<IParticipationStatu>                              ParticipationStatus                              { get; set; }
        #endregion
    }
}
