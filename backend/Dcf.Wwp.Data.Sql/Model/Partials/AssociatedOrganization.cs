using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class AssociatedOrganization : BaseEntity, IAssociatedOrganization
    {
        #region Properties

        IContractArea IAssociatedOrganization.ContractArea
        {
            get => ContractArea;
            set => ContractArea = (ContractArea)value;
        }

        IOrganization IAssociatedOrganization.Organization
        {
            get => Organization;
            set => Organization = (Organization)value;
        }

        #endregion
    }
}
