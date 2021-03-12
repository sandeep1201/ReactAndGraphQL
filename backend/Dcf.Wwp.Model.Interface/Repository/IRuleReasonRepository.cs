using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Dcf.Wwp.Model.Interface.Repository
{
    public interface IRuleReasonRepository
    {
        IEnumerable<IRuleReason> GetRuleReasonsAll();
        IEnumerable<IRuleReason> GetRuleReasonsWhere(Expression<Func<IRuleReason, bool>> clause);
    }
}
