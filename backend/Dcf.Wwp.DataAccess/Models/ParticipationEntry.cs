using System;
using System.Collections.Generic;
using Dcf.Wwp.DataAccess.Base;

namespace Dcf.Wwp.DataAccess.Models
{
    public class ParticipationEntry : BaseEntity
    {
        #region Properties

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

        public virtual Participant                           Participant                { get; set; }
        public virtual EmployabilityPlan                     EmployabilityPlan          { get; set; }
        public virtual Activity                              Activity                   { get; set; }
        public virtual NonParticipationReason                NonParticipationReason     { get; set; }
        public virtual GoodCauseGrantedReason                GoodCauseGrantedReason     { get; set; }
        public virtual GoodCauseDeniedReason                 GoodCauseDeniedReason      { get; set; }
        public virtual PlacementType                         PlacementType              { get; set; }
        public virtual ICollection<ParticipationMakeUpEntry> ParticipationMakeUpEntries { get; set; }

        #endregion
    }
}
