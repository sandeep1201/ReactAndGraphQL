using System.Collections.Generic;

namespace Dcf.Wwp.Model.Interface.Repository
{
    public interface IActivityTypeRepository
    {
        IEnumerable<IEnrolledProgramEPActivityTypeBridge> ActivityTypes(string options);
    }
}
