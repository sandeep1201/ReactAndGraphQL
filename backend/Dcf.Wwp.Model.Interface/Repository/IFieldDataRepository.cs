using System.Collections.Generic;

namespace Dcf.Wwp.Model.Interface.Repository
{
    public interface IFieldDataRepository
    {
        IEnumerable<IYesNoSkipLookup> YesNoSkipLookups();
        IEnumerable<IYesNoUnknownLookup> YesNoUnknownLookups();
        IEnumerable<IYesNoRefused> AllYesNoRefusedLookups();
        IEnumerable<IYesNoRefused> YesNoRefusedLookups();
    }
}