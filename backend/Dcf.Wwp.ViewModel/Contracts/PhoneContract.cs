using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dcf.Wwp.Api.Library.Contracts
{
    public class PhoneContract
    {
        public int?   Id           {  get; set; }
        public string Type         { get;  set; }
        public int?   TypeId       { get;  set; }
        public string PhoneNumber  { get;  set; }
        public bool?  CanText      { get;  set; }
        public bool?  CanVoiceMail { get;  set; }

    }
}
