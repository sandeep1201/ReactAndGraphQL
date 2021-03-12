using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Repository;

namespace Dcf.Wwp.Model.Repository
{
    public partial class Repository:ISymptomRepository
    {
        public IEnumerable<ISymptom> Symptoms()
        {
            return (from x in _db.Symptoms orderby x.SortOrder select x);
        }

    }
}
