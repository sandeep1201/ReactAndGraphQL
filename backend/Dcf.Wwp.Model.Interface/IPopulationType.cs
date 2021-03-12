using System.Collections.Generic;

namespace Dcf.Wwp.Model.Interface
{
    public interface IPopulationType : ICommonDelModel
    {
        #region Properties

        string Name      { get; set; }
        int    SortOrder { get; set; }

        #endregion

        #region Navigation Props

        ICollection<IEnrolledProgramOrganizationPopulationTypeBridge> EnrolledProgramOrganizationPopulationTypeBridges { get; set; }
        ICollection<IDisabledPopulationType>                          DisabledPopulationTypes                          { get; set; }
        ICollection<IRequestForAssistancePopulationTypeBridge>        RequestForAssistancePopulationTypeBridges        { get; set; }

        #endregion
    }
}
