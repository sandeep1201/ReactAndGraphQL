using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Repository;


namespace Dcf.Wwp.Model.Repository
{
    public partial class Repository : ILanguageRepository
    {
        public IEnumerable<ILanguage> Languages()
        {
            var q = from x in _db.Languages where x.IsDeleted == false orderby x.SortOrder select x;
            return q;
        }

        public IEnumerable<ILanguage> AllLanguages()
        {
            var q = from x in _db.Languages orderby x.SortOrder select x;
            return q;
        }
    }
}
