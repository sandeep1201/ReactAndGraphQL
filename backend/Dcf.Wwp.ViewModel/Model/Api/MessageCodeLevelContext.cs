using System;
using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Api.Library.Enums;
using Dcf.Wwp.Api.Library.Extensions;
using Dcf.Wwp.Data.Sql.Model;
using Dcf.Wwp.Model.Interface;
using DCF.Common.Exceptions;
using DCF.Core.Exceptions;

namespace Dcf.Wwp.Api.Library.Model.Api
{
    public class MessageCodeLevelContext
    {
        public MessageCodeLevelContext()
        {
            CodesAndMesssegesByLevel = new List<CodeAndMesssegeByLevel>();
        }

        public List<CodeAndMesssegeByLevel> CodesAndMesssegesByLevel { get; set;}

        public List<IRuleReason> PossibleRuleReasons { get; set; }

        public string GetMessegeByCode (string code)
        {
            var msg = PossibleRuleReasons?.SingleOrDefault(x => x.Code.TrimAndLower() == code?.TrimAndLower())?.Name;

            if (msg == null)
                throw new UserFriendlyException();

            return msg;
        }

        public void AddMessegeCodeAndLevel(string messege, string code, CodeLevel level)
        {
            if(CodesAndMesssegesByLevel == null)
                CodesAndMesssegesByLevel = new List<CodeAndMesssegeByLevel>();

            var codeAndMesssegeByLevel = new CodeAndMesssegeByLevel
            {
                Code = code,
                Level = level,
                Message = messege
            };

            CodesAndMesssegesByLevel.Add(codeAndMesssegeByLevel);
        }
    }
}
