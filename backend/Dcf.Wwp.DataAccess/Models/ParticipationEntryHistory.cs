using System;

namespace Dcf.Wwp.DataAccess.Models
{
    public class ParticipationEntryHistory
    {
        #region Properties

        public int       Id                            { get; set; }
        public int       ParticipantId                 { get; set; }
        public int       EPId                          { get; set; }
        public int       ActivityId                    { get; set; }
        public decimal?  CaseNumber                    { get; set; }
        public DateTime  ParticipationDate             { get; set; }
        public decimal   ScheduledHours                { get; set; }
        public decimal?  ReportedHours                 { get; set; }
        public decimal?  TotalMakeupHours              { get; set; }
        public decimal?  ParticipatedHours             { get; set; }
        public decimal?  NonParticipatedHours          { get; set; }
        public decimal?  GoodCausedHours               { get; set; }
        public int?      NonParticipationReasonId      { get; set; }
        public string    NonParticipationReasonDetails { get; set; }
        public bool?     GoodCauseGranted              { get; set; }
        public int?      GoodCauseGrantedReasonId      { get; set; }
        public int?      GoodCauseDeniedReasonId       { get; set; }
        public string    GoodCauseReasonDetails        { get; set; }
        public int?      PlacementTypeId               { get; set; }
        public bool?     FormalAssessmentExists        { get; set; }
        public bool?     HoursSanctionable             { get; set; }
        public bool      IsProcessed                   { get; set; }
        public DateTime? ProcessedDate                 { get; set; }
        public bool      IsDeleted                     { get; set; }
        public string    ModifiedBy                    { get; set; }
        public DateTime  ModifiedDate                  { get; set; }

        #endregion

        #region Navigation Properties

        public virtual EmployabilityPlan EmployabilityPlan { get; set; }
        public virtual Activity          Activity          { get; set; }

        #endregion
    }
}
