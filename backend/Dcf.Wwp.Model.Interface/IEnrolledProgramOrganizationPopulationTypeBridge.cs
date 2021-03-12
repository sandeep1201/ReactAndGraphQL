using System.Collections.Generic;

namespace Dcf.Wwp.Model.Interface
{
    public interface IEnrolledProgramOrganizationPopulationTypeBridge : ICommonDelModel
    {
        #region Properties

        int  EnrolledProgramId { get; set; }
        int? OrganizationId    { get; set; }
        int  PopulationTypeId  { get; set; }

        #endregion

        #region Navigation Props

        IOrganization                        Organization            { get; set; }
        IEnrolledProgram                     EnrolledProgram         { get; set; }
        IPopulationType                      PopulationType          { get; set; }
        ICollection<IDisabledPopulationType> DisabledPopulationTypes { get; set; }

        #endregion
    }
}
