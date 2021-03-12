using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Repository;

namespace Dcf.Wwp.Model.Repository
{
    public partial class Repository : IAliasTypeRepository
    {
        public IEnumerable<IAliasType> AliasTypes()
        {
            return _db.AliasTypes.Where(x => !x.IsDeleted);
        }

    }
}

