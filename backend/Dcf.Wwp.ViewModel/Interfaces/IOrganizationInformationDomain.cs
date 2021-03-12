using System.Collections.Generic;
using Dcf.Wwp.Api.Library.Contracts;

namespace Dcf.Wwp.Api.Library.Interfaces
{
    public interface IOrganizationInformationDomain
    {
        #region Properties

        #endregion

        #region Methods

        OrganizationInformationContract GetIOrganizationInformation(int                                progId,                                  int orgId);
        OrganizationInformationContract UpsertIOrganizationInformation(OrganizationInformationContract organizationInformationContractContract, int id);

        #endregion
    }
}
