using System;
using System.Linq.Expressions;

namespace Dcf.Wwp.Model.Interface.Repository
{
    public interface INonCustodialChildRepository
    {
        INonCustodialChild NewNonCustodialChild(INonCustodialCaretaker caretaker, string user);
        INonCustodialChild GetNonCustodialChild(int id);
        INonCustodialChild GetNonCustodialChild(Expression<Func<INonCustodialChild, bool>> clause);
    }
}