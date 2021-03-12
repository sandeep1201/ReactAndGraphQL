using System.Collections.Generic;

namespace Dcf.Wwp.Model.Interface.Repository
{
    public interface IOfficeRepository
    {
        List<IOffice>        GetOffices();
        IEnumerable<IOffice> MilwaukeeOffices();
        IEnumerable<IOffice> MilwaukeeOfficesByProgramCode(string       programCode);
        IEnumerable<IOffice> OfficesByOrganizationCode(string           code);
        IOffice              GetOfficeByNumberAndProgram(string         number,           int    programId);
        IOffice              GetOfficeByNumberAndProgramCode(int        number,           string programCode);
        IEnumerable<IOffice> GetOfficesByCountyAndProgramCode(int       countyandTribeId, string programCode);
        IEnumerable<IOffice> GetOfficesByContractAreaAndProgramCode(int contractAreaId,   string programCode);
        IOffice              GetOfficeById(int                          id);
        IOffice              GetOfficeByNumber(int                      officeNumber);
    }
}
