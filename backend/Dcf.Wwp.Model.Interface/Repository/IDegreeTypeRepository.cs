using System.Collections.Generic;

namespace Dcf.Wwp.Model.Interface.Repository
{
    public interface IDegreeTypeRepository
    {
        IDegreeType DegreeByCode(int? degreeType);

        IEnumerable<IDegreeType> DegreeTypes();
    }
}
