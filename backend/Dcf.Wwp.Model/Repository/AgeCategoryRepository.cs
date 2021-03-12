using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Model.Interface;


namespace Dcf.Wwp.Model.Repository
{
    public partial class Repository
    {
        public int AgeCategoryByName(string name)
        {
            return (from x in _db.AgeCategories where x.AgeRange == name select x.Id).FirstOrDefault();
        }

        public IEnumerable<IAgeCategory> AllAgeCategories()
        {
            return from x in _db.AgeCategories select x;
        }
    }
}
