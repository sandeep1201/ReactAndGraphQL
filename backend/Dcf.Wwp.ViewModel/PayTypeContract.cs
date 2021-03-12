using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Dcf.Wwp.Api.Library
{
   public class PayTypeContract
    {
        public PayTypeContract()
        {
            PayTypes = new List<int>();
        }

        [DataMember(Name = "payTypes")]
        public List<int> PayTypes { get; set; }
    }
}
