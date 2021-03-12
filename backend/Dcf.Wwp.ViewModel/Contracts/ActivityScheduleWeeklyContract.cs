using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Api.Library.Contracts
{
    public class ActivityScheduleWeeklyContract
    {

        public int WKFrequencyId { get; set; }

        public string WKFrequencyName { get; set; }
    }
}
