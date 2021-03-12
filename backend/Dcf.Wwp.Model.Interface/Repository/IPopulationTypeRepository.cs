using System.Collections.Generic;

namespace Dcf.Wwp.Model.Interface.Repository
{
    public interface IPopulationTypeRepository
    {
        IEnumerable<IPopulationType> PopulationTypes();
        IEnumerable<IPopulationTypeDto>     PopulationTypesFor(string programName, string agencyName = null);
    }
}
