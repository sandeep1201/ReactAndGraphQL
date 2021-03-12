using System;
using System.Linq;
using System.Linq.Expressions;
using Dcf.Wwp.Data.Sql.Model;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Repository;

namespace Dcf.Wwp.Model.Repository
{
    public partial class Repository : INonCustodialChildRepository
    {
        public INonCustodialChild NewNonCustodialChild(INonCustodialCaretaker caretaker, string user)
        {
            var r = new NonCustodialChild
                    {
                        NonCustodialCaretaker = caretaker as NonCustodialCaretaker,
                        ModifiedBy            = user,
                        ModifiedDate          = DateTime.Now
                    };

            _db.NonCustodialChilds.Add(r);

            return (r);
        }

        public INonCustodialChild GetNonCustodialChild(int id)
        {
            var r = _db.NonCustodialChilds.FirstOrDefault(i => i.Id == id);

            return (r);
        }

        public INonCustodialChild GetNonCustodialChild(Expression<Func<INonCustodialChild, bool>> clause)
        {
            var r = _db.NonCustodialChilds.FirstOrDefault(clause);
                       
            return (r);
        }
    }
}
