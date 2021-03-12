using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Repository;

namespace Dcf.Wwp.Model.Repository
{
    public partial class Repository : IRuleReasonRepository
    {
        public IEnumerable<IRuleReason> GetRuleReasonsAll()
        {
            var r = _db.RuleReasons.ToList();

            return (r);
        }

        public IEnumerable<IRuleReason> GetRuleReasonsWhere(Expression<Func<IRuleReason, bool>> clause)
        {
            try
            {
                return _db.RuleReasons
                          .Where(clause)
                          .AsNoTracking()
                          .ToList();
            }
            catch (NullReferenceException )
            {
                return new List<IRuleReason>();
            }
        }
    }
}
