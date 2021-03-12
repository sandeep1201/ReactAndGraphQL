using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Repository;

namespace Dcf.Wwp.Model.Repository
{
    public partial class Repository : IDeleteReasonByRepeaterRepository
    {
        public IEnumerable<IDeleteReasonByRepeater> DeleteReasonsByRepeater(string repeater)
        {
            return (from drr in _db.DeleteReasonByRepeaters where drr.Repeater == repeater select drr).ToList();
        }
    }
}
