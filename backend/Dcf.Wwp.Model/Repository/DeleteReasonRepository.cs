using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Repository;

namespace Dcf.Wwp.Model.Repository
{
    public partial class Repository : IDeleteReasonRepository
    {
        public IDeleteReason DeleteReasonByName(string name)
        {
            return (from x in _db.DeleteReasons where x.Name == name select x).SingleOrDefault();
        }

        public IDeleteReason DeleteReasonById(int id)
        {
            return (from x in _db.DeleteReasons where x.Id == id select x).SingleOrDefault();
        }
    }
}
