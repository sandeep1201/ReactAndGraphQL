using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Api.Library.Contracts
{
    public class JRWorkPreferencesContract
    {
        public JRWorkPreferencesContract()
        {
        }

        public int          Id                                 { get; set; }
        public string       KindOfJobDetails                   { get; set; }
        public string       JobInterestDetails                 { get; set; }
        public string       TrainingNeededForJobDetails        { get; set; }
        public string       SomeOtherPlacesJobAvailableDetails { get; set; }
        public bool?        SomeOtherPlacesJobAvailableUnknown { get; set; }
        public string       SituationsToAvoidDetails           { get; set; }
        public int?         BeginHour                          { get; set; }
        public int?         BeginMinute                        { get; set; }
        public int?         BeginAmPm                          { get; set; }
        public int?         EndHour                            { get; set; }
        public int?         EndMinute                          { get; set; }
        public int?         EndAmPm                            { get; set; }
        public string       WorkScheduleDetails                { get; set; }
        public string       TravelTimeToWork                   { get; set; }
        public string       DistanceHomeToWork                 { get; set; }
        public List<int>    WorkShiftIds                       { get; set; }
        public List<string> WorkShiftNames                     { get; set; }
    }
}
