using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Api.Library.Enums;
using Dcf.Wwp.Api.Library.Extensions;
using Dcf.Wwp.Data.Sql.Model;
using DCF.Common.Exceptions;

namespace Dcf.Wwp.Api.Library.Model.Api
{
    public class CodeLevelMessageContext
    {
        public CodeLevelMessageContext()
        {
            CodesAndMessagesByLevel = new List<CodeAndMesssegeByLevel>();
        }

        public List<CodeAndMesssegeByLevel> CodesAndMessagesByLevel { get; private set; }
        public List<RuleReason>             PossibleRuleReasons     { get; set; }

        public string GetMessageByCode (string code)
        {
            var msg = PossibleRuleReasons?.SingleOrDefault(x => x.Code.TrimAndLower() == code?.TrimAndLower())?.Name;

            if (msg == null)
                throw new UserFriendlyException();

            return msg;
        }

        public void AddMessageCodeAndLevel(string message, string code, CodeLevel level)
        {
            if (CodesAndMessagesByLevel == null)
                CodesAndMessagesByLevel = new List<CodeAndMesssegeByLevel>();

            var codeAndMessageByLevel = new CodeAndMesssegeByLevel
                                        {
                                            Code    = code,
                                            Level   = level,
                                            Message = message
                                        };

            CodesAndMessagesByLevel.Add(codeAndMessageByLevel);
        }
    }
}
