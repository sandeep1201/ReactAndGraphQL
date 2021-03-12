using System;
using System.Collections.Generic;
using NRules;
using NRules.Fluent;
using NRules.RuleModel;

namespace Dcf.Wwp.Api.Library.Rules.Infrastructure
{
    public class WWPRuleRepository : IWWPRuleRepository
    {
        #region Properties

        private readonly RuleRepository _ruleRepository = new RuleRepository();

        #endregion

        public void Load(Action<IRuleLoadSpec> specAction)
        {
            _ruleRepository.Load(specAction);
        }

        public ISession CreateSession()
        {
            return _ruleRepository.Compile().CreateSession();
        }

        public IEnumerable<IRuleSet> GetRuleSets()
        {
            return _ruleRepository.GetRuleSets();
        }
    }
}
