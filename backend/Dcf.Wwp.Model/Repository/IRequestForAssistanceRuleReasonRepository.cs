using System;
using System.Linq;
using Dcf.Wwp.Data.Sql.Model;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Repository;

namespace Dcf.Wwp.Model.Repository
{
    public partial class Repository : IRequestForAssistanceRuleReasonRepository
    {
        public IRequestForAssistanceRuleReason NewRfaEligibility(IRequestForAssistance rfa, string ruleCode, string user)
        {
            var t = _db.RuleReasons.FirstOrDefault(i => i.Code == ruleCode);

            var row = new RequestForAssistanceRuleReason
                      {
                          RequestForAssistance = (RequestForAssistance)rfa,
                          RuleReasonId         = t.Id,
                          IsDeleted            = false,
                          ModifiedBy           = user,
                          ModifiedDate         = DateTime.Now
                      };

            _db.RequestForAssistanceRuleReasons.Add(row);

            return (row);
        }

        public void DeleteAllRfaEligibilityRows(int rfaId)
        {
            var oldrows = _db.RequestForAssistanceRuleReasons.Where(i => i.RequestForAssistanceId == rfaId).ToList();

            if (oldrows.Any())
            {
                _db.RequestForAssistanceRuleReasons.RemoveRange(oldrows);
            }
        }
    }
}
