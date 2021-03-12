using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dcf.Wwp.Data.Sql.Model;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Model.Repository
{
    public partial class Repository
    {
        public IConvictionType ConvictionTypeById(int? id)
        {
            var conventiontype = (from ct in _db.ConvictionTypes where ct.Id  == id select ct).FirstOrDefault() ;
            return conventiontype;
        }

        public IEnumerable<IConvictionType> ConvictionTypes()
        {
            var convictiontypes = (from ct in _db.ConvictionTypes orderby ct.SortOrder select ct);
            return convictiontypes;
        }
    }
}
