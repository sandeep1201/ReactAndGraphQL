using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dcf.Wwp.Model.Interface.Repository
{
    public interface IContractAreaRepository
    {
        List<IContractArea> GetContractAreasByProgramCode(string                  programCode);
        List<IContractArea> GetContractAreasByProgramCodeAndOrganizationId(string programCode, int orgId);
        List<IContractArea> GetContractArea(int                                   id);
    }
}
