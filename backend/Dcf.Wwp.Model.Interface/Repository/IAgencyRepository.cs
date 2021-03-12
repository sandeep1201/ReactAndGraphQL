using System.Collections.Generic;

namespace Dcf.Wwp.Model.Interface.Repository
{
    public interface IAgencyRepository
    {
        List<IAgency> GetAgencies();
        IAgency GetAgencyByOfficeNumber(string officeNumber);
        IEnumerable<IAgency> GetContractorsForProgram(string programName);
    }
}
