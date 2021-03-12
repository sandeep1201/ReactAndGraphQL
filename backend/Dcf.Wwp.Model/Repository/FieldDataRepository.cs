using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Repository;


namespace Dcf.Wwp.Model.Repository
{
    public partial class Repository : IFieldDataRepository
    {
        public IEnumerable<IYesNoSkipLookup> YesNoSkipLookups()
        {
            return _db.YesNoSkipLookups;
        }

        public IEnumerable<IYesNoUnknownLookup> YesNoUnknownLookups()
        {
            return _db.YesNoUnknownLookups;
        }

        public IEnumerable<IYesNoRefused> YesNoRefusedLookups()
        {
            return _db.YesNoRefuseds;
        }

        public IEnumerable<IYesNoRefused> AllYesNoRefusedLookups()
        {
            return _db.YesNoRefuseds.Where(x => !x.IsDeleted);
        }
    }
}