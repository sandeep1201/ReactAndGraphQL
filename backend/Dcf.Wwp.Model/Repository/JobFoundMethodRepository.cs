using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Repository;

namespace Dcf.Wwp.Model.Repository
{
   public partial class Repository:IJobFoundMethodRepository
    {
       public IEnumerable<IJobFoundMethod> JobFoundMethods()
       {
           var q = from x in _db.JobFoundMethods orderby x.SortOrder select x;
           return q;
       }

       public IJobFoundMethod JobFoundMethodByName(string name)
       {
           var q = (from x in _db.JobFoundMethods where x.Name.ToLower() == name.ToLower() select x).FirstOrDefault();
           return q;
       }
    }
}
