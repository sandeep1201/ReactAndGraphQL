using System.Collections.Generic;

namespace Dcf.Wwp.Model.Interface
{
    public interface IWPOrganizationRepository
    {
        IEnumerable<IWPOrganization> GetWPOrganizations();
    }
}
