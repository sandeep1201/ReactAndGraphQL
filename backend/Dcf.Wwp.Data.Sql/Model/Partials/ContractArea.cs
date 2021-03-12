using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class ContractArea : BaseEntity, IContractArea
    {
        #region Properties

        IEnrolledProgram IContractArea.EnrolledProgram
        {
            get => EnrolledProgram;
            set => EnrolledProgram = (EnrolledProgram) value;
        }

        IOrganization IContractArea.Organization
        {
            get => Organization;
            set => Organization = (Organization) value;
        }

        ICollection<IOffice> IContractArea.Offices
        {
            get => Offices.Cast<IOffice>().ToList();
            set => Offices = value.Cast<Office>().ToList();
        }

        ICollection<IAssociatedOrganization> IContractArea.AssociatedOrganizations
        {
            get => AssociatedOrganizations.Cast<IAssociatedOrganization>().ToList();
            set => AssociatedOrganizations = value.Cast<AssociatedOrganization>().ToList();
        }

        #endregion
    }
}
