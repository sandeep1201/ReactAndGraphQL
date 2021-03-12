using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dcf.Wwp.Api.Library.Contracts
{
    public class StatusContract
    {
        public StatusContract()
        {
            ErrorMessages = new List<string>();
        }

        public decimal? PinNumber {  get; set; }
           
        public List<string> ErrorMessages { get; set; }

    }
}
