using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Repository;

namespace Dcf.Wwp.Model.Repository
{
    public partial class Repository : IBenefitsOfferedTypeRepository
    {
        public IEnumerable<IBenefitsOfferedType> BenefitsOfferedTypes()
        {
            var b = from x in _db.BenefitsOfferedTypes orderby x.SortOrder select x;
            return b;
        }
    }
}
