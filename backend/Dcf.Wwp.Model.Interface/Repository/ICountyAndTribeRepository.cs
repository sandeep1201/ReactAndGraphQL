using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Dcf.Wwp.Model.Interface.Repository
{
    public interface ICountyAndTribeRepository
    {
        IEnumerable<ICountyAndTribe> GetCountyAndTribes();

        IEnumerable<ICountyAndTribe> GetTribes();
        IEnumerable<ICountyAndTribe> GetCounties();
        IEnumerable<ICountyAndTribe> WhereCountyAndTribe(Expression<Func<ICountyAndTribe, bool>> clause);
        ICountyAndTribe GetCountyOrTribe(Expression<Func<ICountyAndTribe, bool>> clause);
        ICountyAndTribe GetCountyOrTribeById(long id);
    }
}
