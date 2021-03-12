using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Dcf.Wwp.Model.Repository
{
    public partial class Repository:IDegreeTypeRepository
    {
        public IDegreeType DegreeByCode(int? degreeType)
        {            
            return (from x in _db.DegreeTypes where x.Id == degreeType select x).SingleOrDefault();
        }

        public IEnumerable<IDegreeType> DegreeTypes()
        {
            var d = from x in _db.DegreeTypes orderby x.SortOrder select x;
            return d;
        }
    
    }
}
