using System.Collections.Generic;

namespace Dcf.Wwp.Model.Interface.Repository
{
    public interface IGenderTypeRepository
    {
        IEnumerable<IGenderType> GenderTypes();
    }
}
