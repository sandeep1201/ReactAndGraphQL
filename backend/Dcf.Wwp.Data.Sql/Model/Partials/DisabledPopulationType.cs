using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class DisabledPopulationType : BaseCommonModel, IDisabledPopulationType
    {
        IEnrolledProgramOrganizationPopulationTypeBridge IDisabledPopulationType.EnrolledProgramOrganizationPopulationTypeBridge
        {
            get { return EnrolledProgramOrganizationPopulationTypeBridge; }
            set { EnrolledProgramOrganizationPopulationTypeBridge = (EnrolledProgramOrganizationPopulationTypeBridge) value; }
        }

        IPopulationType IDisabledPopulationType.PopulationType
        {
            get { return PopulationType; }
            set { PopulationType = (PopulationType) value; }
        }
    }
}
