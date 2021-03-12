using System;
using System.Collections.Generic;
using Dcf.Wwp.Api.Library.Rules.Infrastructure;
using NRules;
using NRules.Fluent;
using NRules.RuleModel;

namespace Dcf.Wwp.UnitTest.Repositories
{
    public class MockRuleRepository : IWWPRuleRepository
    {
        private readonly RuleRepository        _ruleRepository = new RuleRepository();
        public  readonly List<IFact>           InsertedFacts   = new List<IFact>();
        public  readonly List<IRuleDefinition> FiredRules      = new List<IRuleDefinition>();
        public           bool                  HasCreateSessionBeenCalled;

        public void Load(Action<IRuleLoadSpec> specAction)
        {
            _ruleRepository.Load(specAction);
        }

        public ISession CreateSession()
        {
            var session = _ruleRepository.Compile().CreateSession();

            session.Events.FactInsertedEvent += (sender, args) => InsertedFacts.Add(args.Fact);
            session.Events.RuleFiredEvent    += (sender, args) => FiredRules.Add(args.Rule);
            HasCreateSessionBeenCalled       =  true;

            return session;
        }

        public IEnumerable<IRuleSet> GetRuleSets()
        {
            return _ruleRepository.GetRuleSets();
        }
    }
}
