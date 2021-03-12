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
    public class ActivityScheduleFrequencyContract
    {
        public int                            Id                 { get; set; }
        public int?                           ActivityScheduleId { get; set; }
        public int?                           WKFrequencyId      { get; set; }
        public string                         WKFrequencyName    { get; set; }
        public int?                           MRFrequencyId      { get; set; }
        public string                         MRFrequencyName    { get; set; }
        public string ShortName { get; set; }
    }
}
