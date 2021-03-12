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
    public class ActivityContract
    {
        public ActivityContract()
        {
            Contacts          = new List<int?>();
            ActivitySchedules = new List<ActivityScheduleContract>();
        }

        public int                            Id                           { get; set; }
        public int                            ActivityTypeId               { get; set; }
        public string                         ActivityTypeName             { get; set; }
        public string                         ActivityTypeCode             { get; set; }
        public string                         Description                  { get; set; }
        public int?                           ActivityLocationId           { get; set; }
        public string                         ActivityLocationName         { get; set; }
        public string                         AdditionalInformation        { get; set; }
        public string                         ModifiedBy                   { get; set; }
        public DateTime                       ModifiedDate                 { get; set; }
        public int                            EmployabilityPlanId          { get; set; }
        public int?                           ActivityCompletionReasonId   { get; set; }
        public string                         ActivityCompletionReasonName { get; set; }
        public string                         ActivityCompletionReasonCode { get; set; }
        public string                         EndDate                      { get; set; }
        public string                         MinStartDate                 { get; set; }
        public bool                           IsCarriedOver                { get; set; }
        public string                         Program                      { get; set; }
        public List<int?>                     Contacts                     { get; set; }
        public List<ActivityScheduleContract> ActivitySchedules            { get; set; }
        public NonSelfDirectedActivity        NonSelfDirectedActivity      { get; set; }
        
    }
}
