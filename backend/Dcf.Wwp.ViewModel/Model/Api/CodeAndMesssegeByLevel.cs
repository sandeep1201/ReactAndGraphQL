using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dcf.Wwp.Api.Library.Enums;

namespace Dcf.Wwp.Api.Library.Model.Api
{
    public class CodeAndMesssegeByLevel
    {
        public CodeAndMesssegeByLevel()
        {
           
        }
        public CodeAndMesssegeByLevel (CodeLevel level,string code)
        {
            Level = level;
            Code = code;
        }
        public CodeLevel Level { get; set; }
        public string Code  { get; set; }
        public string Message { get; set; }
    }
}
