using System;
using System.Collections.Generic;
using NRules;
using NRules.RuleModel;

namespace Dcf.Wwp.Api.Library.Rules.Infrastructure
{
    public interface IWWPRuleRepository
    {
        void                  Load(Action<NRules.Fluent.IRuleLoadSpec> specAction);
        ISession              CreateSession();
        IEnumerable<IRuleSet> GetRuleSets();
    }
}
