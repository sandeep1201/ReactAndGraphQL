using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Api.Library.Contracts
{
    public class DrugScreeningContract
    {
        public int                               Id                        { get; set; }
        public int                               ParticipantId             { get; set; }
        public bool                              UsedNonRequiredDrugs      { get; set; }
        public bool                              AbusedMoreDrugs           { get; set; }
        public bool                              CannotStopAbusingDrugs    { get; set; }
        public bool                              HadBlackoutsFromDrugs     { get; set; }
        public bool                              FeelGuiltyAboutDrugs      { get; set; }
        public bool                              SpouseComplaintOnDrugs    { get; set; }
        public bool                              NeglectedFamilyForDrugs   { get; set; }
        public bool                              IllegalActivitiesForDrugs { get; set; }
        public bool                              SickFromStoppingDrugs     { get; set; }
        public bool                              MedicalProblemsFromDrugs  { get; set; }
        public int?                              DrugScreeningStatusTypeId { get; set; }
        public string                            Details                   { get; set; }
        public bool                              IsDeleted                 { get; set; }
        public string                            ModifiedBy                { get; set; }
        public DateTime                          ModifiedDate              { get; set; }
        public List<DrugScreeningStatusContract> DrugScreeningStatuses     { get; set; }
    }

    public class DrugScreeningStatusContract
    {
        public int      Id                             { get; set; }
        public int      DrugScreeningStatusTypeId      { get; set; }
        public string   DrugScreeningStatusName        { get; set; }
        public string   DrugScreeningStatusDisplayName { get; set; }
        public DateTime DrugScreeningStatusDate        { get; set; }
        public string   Details                        { get; set; }
        public string   ModifiedBy                     { get; set; }
        public DateTime ModifiedDate                   { get; set; }
    }
}
