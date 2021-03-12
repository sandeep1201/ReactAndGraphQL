using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class PopulationType : BaseCommonModel, IPopulationType
    {
        ICollection<IEnrolledProgramOrganizationPopulationTypeBridge> IPopulationType.EnrolledProgramOrganizationPopulationTypeBridges
        {
            get => EnrolledProgramOrganizationPopulationTypeBridges.Cast<IEnrolledProgramOrganizationPopulationTypeBridge>().ToList();

            set => EnrolledProgramOrganizationPopulationTypeBridges = value.Cast<EnrolledProgramOrganizationPopulationTypeBridge>().ToList();
        }

        ICollection<IDisabledPopulationType> IPopulationType.DisabledPopulationTypes
        {
            get => DisabledPopulationTypes.Cast<IDisabledPopulationType>().ToList();

            set => DisabledPopulationTypes = value.Cast<DisabledPopulationType>().ToList();
        }

        ICollection<IRequestForAssistancePopulationTypeBridge> IPopulationType.RequestForAssistancePopulationTypeBridges
        {
            get => RequestForAssistancePopulationTypeBridges.Cast<IRequestForAssistancePopulationTypeBridge>().ToList();

            set => RequestForAssistancePopulationTypeBridges = value.Cast<RequestForAssistancePopulationTypeBridge>().ToList();
        }
    }
}
