using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Repository;

namespace Dcf.Wwp.Model.Repository
{
    public partial class Repository:IAccommodationRepository
    {
       public IEnumerable<IAccommodation> Accommodations()
        {
            return (from x in _db.Accommodations orderby x.SortOrder select x);
        }
    }
}
