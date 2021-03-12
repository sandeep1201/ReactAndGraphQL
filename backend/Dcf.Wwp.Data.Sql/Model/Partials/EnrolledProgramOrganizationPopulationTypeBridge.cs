using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class EnrolledProgramOrganizationPopulationTypeBridge : BaseEntity, IEnrolledProgramOrganizationPopulationTypeBridge
    {
        #region Properties

        #endregion

        #region Nav Props

        IOrganization IEnrolledProgramOrganizationPopulationTypeBridge.Organization
        {
            get { return Organization; }
            set { Organization = (Organization) value; }
        }

        IEnrolledProgram IEnrolledProgramOrganizationPopulationTypeBridge.EnrolledProgram
        {
            get { return EnrolledProgram; }
            set { EnrolledProgram = (EnrolledProgram) value; }
        }

        IPopulationType IEnrolledProgramOrganizationPopulationTypeBridge.PopulationType
        {
            get { return PopulationType; }
            set { PopulationType = (PopulationType) value; }
        }

        ICollection<IDisabledPopulationType> IEnrolledProgramOrganizationPopulationTypeBridge.DisabledPopulationTypes
        {
            get { return DisabledPopulationTypes.Cast<IDisabledPopulationType>().ToList(); }

            set { DisabledPopulationTypes = value.Cast<DisabledPopulationType>().ToList(); }
        }

        #endregion
    }
}
