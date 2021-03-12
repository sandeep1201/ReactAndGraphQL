using System.Collections.Generic;

namespace Dcf.Wwp.Model.Interface.Repository
{
    public interface IOrganizationRepository
    {
        IEnumerable<IOrganization> GetOrganizations();
        IOrganization              GetOrganizationByOfficeNumber(short?    officeNumber);
        IOrganization              GetOrganizationByCode(string            orgCode);
        IAssociatedOrganization    GetAssociatedOrganization(IOrganization org);
        IEnumerable<IOrganization> GetOrganizationsForProgram(string       programName);
        IEnumerable<IOrganization> GetOrganizationsByProgramId(string      programId);
    }
}
