using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class HousingSection
    {
        #region Properties

        public int       ParticipantId                 { get; set; }
        public int?      HousingSituationId            { get; set; }
        public string    CurrentHousingDetails         { get; set; }
        public DateTime? CurrentHousingBeginDate       { get; set; }
        public DateTime? CurrentHousingEndDate         { get; set; }
        public decimal?  CurrentMonthlyAmount          { get; set; }
        public bool?     IsCurrentAmountUnknown        { get; set; }
        public bool?     HasCurrentEvictionRisk        { get; set; }
        public bool?     HasBeenEvicted                { get; set; }
        public bool?     IsCurrentMovingToHistory      { get; set; }
        public bool?     HasUtilityDisconnectionRisk   { get; set; }
        public string    UtilityDisconnectionRiskNotes { get; set; }
        public bool?     HasDifficultyWorking          { get; set; }
        public string    DifficultyWorkingNotes        { get; set; }
        public string    Notes                         { get; set; }
        public int?      OriginId                      { get; set; }
        public bool      IsDeleted                     { get; set; }
        public string    ModifiedBy                    { get; set; }
        public DateTime? ModifiedDate                  { get; set; }

        #endregion

        #region Navigation Properties

        public virtual Participant                 Participant      { get; set; }
        public virtual HousingSituation            HousingSituation { get; set; }
        public virtual ICollection<HousingHistory> HousingHistories { get; set; } = new List<HousingHistory>();

        #endregion
    }
}
