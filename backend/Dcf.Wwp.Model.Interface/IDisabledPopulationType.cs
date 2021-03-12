namespace Dcf.Wwp.Model.Interface
{
    public interface IDisabledPopulationType : ICommonDelModel
    {
        #region Properties

        int EnrolledProgramOrganizationPopulationTypeBridgeId { get; set; }
        int PopulationTypeId                                  { get; set; }

        #endregion

        #region Navigation Props

        IPopulationType                                  PopulationType                                  { get; set; }
        IEnrolledProgramOrganizationPopulationTypeBridge EnrolledProgramOrganizationPopulationTypeBridge { get; set; }

        #endregion
    }
}
