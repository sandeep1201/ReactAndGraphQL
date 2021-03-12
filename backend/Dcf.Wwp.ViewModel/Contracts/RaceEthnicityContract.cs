using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dcf.Wwp.Api.Library.Contracts
{
    public class RaceEthnicityContract
    {
        public bool? IsWhite { get; set; }
        public bool? IsAmericanIndian { get; set; }
        public bool? IsPacificIslander { get; set; }
        public bool? IsAsian { get; set; }
        public bool? IsHispanic { get; set; }
        public bool? IsBlack { get; set; }
        public int HistorySequenceNumber { get; set; }
    }
}
