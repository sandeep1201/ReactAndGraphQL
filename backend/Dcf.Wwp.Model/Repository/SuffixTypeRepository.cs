using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Repository;

namespace Dcf.Wwp.Model.Repository
{
    public partial class Repository : ISuffixTypeRepository
    {
        public IEnumerable<ISuffixType> SuffixTypes()
        {
            return _db.SuffixTypes.Where(x => !x.IsDeleted);
        }


        public ISuffixType GetSuffixTypeById(int? id)
        {
            return _db.SuffixTypes.FirstOrDefault(x => x.Id == id);
        }

        public ISuffixType GetSuffixTypeByName(string code)
        {
            return _db.SuffixTypes.FirstOrDefault(x => x.Code == code);
        }
    }
}

