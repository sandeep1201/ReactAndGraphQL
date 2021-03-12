using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Dcf.Wwp.Api.Library.Contracts
{
    public class SsnAliasContract
    {
        public int    Id      { get; set; }
        public decimal?    Ssn     { get; set; }
        public int?    TypeId  { get; set; }
        public string Details { get; set; }

        [JsonIgnore]
        public bool IsEmpty => Ssn == null && TypeId == null && Details.IsNullOrEmpty();
    }
}
