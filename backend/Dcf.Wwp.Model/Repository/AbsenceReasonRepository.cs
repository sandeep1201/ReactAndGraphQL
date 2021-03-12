using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Repository;

namespace Dcf.Wwp.Model.Repository
{
    public partial class Repository : IAbsenceReasonRepository
    {
        public IEnumerable<IAbsenceReason> AbsenceReasons()
        {
            var q = from x in _db.AbsenceReasons orderby x.SortOrder select x;
            return q;
        }
    }
}
