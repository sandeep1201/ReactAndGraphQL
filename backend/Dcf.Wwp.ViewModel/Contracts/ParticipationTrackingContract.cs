using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Api.Library.Contracts
{
    public class ParticipationTrackingContract
    {
        public int                                    Id                            { get; set; }
        public int                                    EPId                          { get; set; }
        public int                                    ActivityId                    { get; set; }
        public string                                 ActivityCd                    { get; set; }
        public string                                 ActivityName                  { get; set; }
        public DateTime                               ParticipationDate             { get; set; }
        public string                                 ScheduledHours                { get; set; }
        public bool?                                  DidParticipate                { get; set; }
        public string                                 ReportedHours                 { get; set; }
        public string                                 TotalMakeupHours              { get; set; }
        public string                                 ParticipatedHours             { get; set; }
        public string                                 NonParticipatedHours          { get; set; }
        public string                                 GoodCausedHours               { get; set; }
        public int?                                   NonParticipationReasonId      { get; set; }
        public string                                 NonParticipationReasonCd      { get; set; }
        public string                                 NonParticipationReasonName    { get; set; }
        public string                                 NonParticipationReasonDetails { get; set; }
        public bool?                                  GoodCauseGranted              { get; set; }
        public int?                                   GoodCauseGrantedReasonId      { get; set; }
        public string                                 GoodCauseGrantedReasonName    { get; set; }
        public string                                 GoodCauseReasonCd             { get; set; }
        public int?                                   GoodCauseDeniedReasonId       { get; set; }
        public string                                 GoodCauseDeniedReasonName     { get; set; }
        public string                                 GoodCauseReasonDetails        { get; set; }
        public int?                                   PlacementTypeId               { get; set; }
        public string                                 PlacementTypeName             { get; set; }
        public string                                 PlacementTypeCd               { get; set; }
        public bool?                                  FormalAssessmentExists        { get; set; }
        public bool?                                  HoursSanctionable             { get; set; }
        public string                                 ModifiedBy                    { get; set; }
        public DateTime                               ModifiedDate                  { get; set; }
        public List<ParticipationMakeUpEntryContract> MakeUpEntries                 { get; set; }
        public bool                                   CanEditBasedOnDate            { get; set; }
        public bool                                   IsProcessed                   { get; set; }
        public string                                 ProcessedDate                 { get; set; }
        public bool                                   CanEditBasedOnOrg             { get; set; }
    }
}
