using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dcf.Wwp.Api.Library.Contracts
{
    public class ActivityReportContract
    {
        public int                                  Id                            { get; set; }
        public string                               ActivityTypeName              { get; set; }
        public string                               ActivityDescription           { get; set; }
        public string                               ActivityLocationName          { get; set; }
        public string                               ActivityPhone                 { get; set; }
        public string                               ActivityContact               { get; set; }
        public string                               ActivityAdditionalInformation { get; set; }
        public bool                                 hasContacts                   { get; set; }
        public bool                                 IsSelfDirected                { get; set; }
        public bool                                 hasSchedules                  { get; set; }
        public bool                                 hasAdditionalInfo             { get; set; }
        public List<ContactReportContract>          Contacts                      { get; set; }
        public List<ActivityScheduleReportContract> ActivitySchedules             { get; set; }
    }
}
